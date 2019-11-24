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
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace App1
{
    /// <summary>
    /// Логика взаимодействия для WorkSpace.xaml
    /// </summary> 

    public partial class WorkSpace : Window
    {
        public WorkSpace()
        {
            InitializeComponent();
        }

        public WorkSpace (int index)
        {
            InitializeComponent();
            for (int i = 0; i < MainWindow.users_db.Count; i++)
                usersGrid.Items.Add(MainWindow.users_db[i]);

            Username_lbl.Content += MainWindow.users_db[index].fio;
            StreamReader sr = new StreamReader(MainWindow.histFileName);
            while (!sr.EndOfStream)
                hist_block.Text += sr.ReadLine() + "\n";

            if (MainWindow.users_db[index].post == "Администратор")
            {
                Edit_page.Visibility = Visibility.Visible;
                histpg_tb.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            workProgress_pb.Value += 10;
            if (workProgress_pb.Value == workProgress_pb.Maximum) {
                MessageBox.Show("Хорошая работа. Продолжайте в том же духе.", "Информация.", MessageBoxButton.OK, MessageBoxImage.Information);
                workProgress_pb.Value = 0;
            }
        }

        private void usersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addUser_btn_Click(object sender, RoutedEventArgs e)
        {
            Window ChildWindow = new redactWnd("Добавить");
            ChildWindow.Owner = this;
            ChildWindow.ShowDialog();
        }

        private void changeUser_btn_Click(object sender, RoutedEventArgs e)
        {
            Window ChildWindow = new redactWnd("Изменить", usersGrid.SelectedIndex);
            ChildWindow.Owner = this;
            ChildWindow.ShowDialog();

        }

        private void deleteUser_btn_Click(object sender, RoutedEventArgs e)
        {
            if (usersGrid.SelectedIndex >= 0)
            {
                MainWindow.users_db.RemoveAt(usersGrid.SelectedIndex);
                usersGrid.Items.RemoveAt(usersGrid.SelectedIndex);
                usersGrid.Items.Refresh();
            }
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlsr = new XmlSerializer(typeof(List<DataRow>));
            using (FileStream fs = new FileStream(MainWindow.saveFileName, FileMode.Create)) xmlsr.Serialize(fs, MainWindow.users_db);
            MessageBox.Show("Сохранено!", "Успех.", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void exit_btn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wnd = new MainWindow();
            wnd.Show();
            this.Close();
        }
    }
}
