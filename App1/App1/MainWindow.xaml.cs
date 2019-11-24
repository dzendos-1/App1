using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace App1
{
    public partial class MainWindow : Window
    {
        public static String saveFileName = "Users.db";
        public static String histFileName = "log.hs";
        public static List<DataRow> users_db = new List<DataRow>();
        public System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        public struct security
        {
            public int count_of_trying;
            public DateTime bloking_time;
        }

        static security sec;
        public void EnableWindow(object sender, EventArgs e)
        {
            timer.Stop();
            sec.count_of_trying = 0;
            XmlSerializer xmlsr = new XmlSerializer(typeof(security));
            using (FileStream fs = new FileStream("check.hs", FileMode.Create)) xmlsr.Serialize(fs, sec);

            this.IsEnabled = true;

        }

        public MainWindow()
        {
            InitializeComponent();
            XmlSerializer xmlsr = new XmlSerializer(typeof(List<DataRow>));
            using (FileStream fs = new FileStream(saveFileName, FileMode.Open)) MainWindow.users_db = (List<DataRow>)xmlsr.Deserialize(fs);

            XmlSerializer xmlsr2 = new XmlSerializer(typeof(security));
            using (FileStream fs2 = new FileStream("check.hs", FileMode.Open)) sec = (security)xmlsr2.Deserialize(fs2);

            if(sec.count_of_trying == 3 && new TimeSpan(0, 5, 0) - (DateTime.Now - sec.bloking_time) < new TimeSpan(0, 5, 0))
            {
                this.IsEnabled = false;
                timer.Tick += EnableWindow;
                timer.Interval = new TimeSpan(0, 5, 0) - (DateTime.Now - sec.bloking_time);
                timer.Start();
                MessageBox.Show("Вы все еще заблокированы!", "Внимание.", MessageBoxButton.OK, MessageBoxImage.Warning);

            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isCorrect = false;
            var macAddr = (
                            from nic in NetworkInterface.GetAllNetworkInterfaces()
                            where nic.OperationalStatus == OperationalStatus.Up
                            select nic.GetPhysicalAddress().ToString()
                          ).FirstOrDefault();
            for (int i = 0; i < users_db.Count; i++)
            {
                if(users_db[i].login == login_txtBox.Text && users_db[i].password == password_txtBox.Password)
                {
                    isCorrect = true;
                    sec.count_of_trying = 0;
                   
                    //Записываем лог
                    FileStream fileStream = new FileStream(histFileName, FileMode.OpenOrCreate);
                    StreamWriter streamWriter = new StreamWriter(fileStream);
                    string msg = DateTime.Now.ToString() + ": " + users_db[i].fio + " зашел в систему. MAC-адрес: " + macAddr.ToString();
                    streamWriter.BaseStream.Seek(msg.Length, SeekOrigin.End);
                    streamWriter.WriteLine(msg);
                    streamWriter.Close();
                    //открываем окно
                    WorkSpace wnd = new WorkSpace(i);
                    wnd.Show();
                    this.Close();
                    break;
                }
            }
            if (!isCorrect)
            {
                sec.count_of_trying++;
                sec.bloking_time = DateTime.Now;
                //записываем лог
                FileStream fileStream = new FileStream(histFileName, FileMode.OpenOrCreate);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                String msg = DateTime.Now.ToString() + ": попытка входа в систему. MAC-адрес: " + macAddr.ToString();
                streamWriter.BaseStream.Seek(msg.Length, SeekOrigin.End);
                streamWriter.WriteLine(msg);
                streamWriter.Close();


                if (sec.count_of_trying == 3)
                {
                    MessageBox.Show("Вы заблокированы!", "Внимание.", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.IsEnabled = false;
                    timer.Tick += EnableWindow;
                    timer.Interval = new TimeSpan(0, 5, 0);
                    timer.Start();
                } else 
                    MessageBox.Show("Логин или пароль неверны!", "Внимание.", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            XmlSerializer xmlsr = new XmlSerializer(typeof(security));
            using (FileStream fs = new FileStream("check.hs", FileMode.Create)) xmlsr.Serialize(fs, sec);

        }
    }
}
