using System;
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

namespace App1
{
    /// <summary>
    /// Логика взаимодействия для redactWnd.xaml
    /// </summary>
    public partial class redactWnd : Window
    {
        String operation_name;
        public int seleted_item;
        public redactWnd()
        {
            InitializeComponent();
        }
        public redactWnd(String pn, int si = -1)
        {
            InitializeComponent();

            operation_name = pn;
            redact_btn.Content = pn;
            seleted_item = si;
            if (si >= 0)
            {
                fio_tb.Text          = MainWindow.users_db[si].fio;
                post_cb.Text         = MainWindow.users_db[si].post;
                login_tb.Text        = MainWindow.users_db[si].login;
                password_tb.Text     = MainWindow.users_db[si].password;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WorkSpace wnd = this.Owner as WorkSpace;
            switch (operation_name)
            {
                case "Добавить":
                    bool isDuplicat = false;
                    for (int i = 0; i < MainWindow.users_db.Count; i++)
                        if (MainWindow.users_db[i].login == login_tb.Text)
                        {
                            isDuplicat = true;
                            break;
                        }
                    if (!isDuplicat)
                    {
                        MainWindow.users_db.Add(new DataRow(fio_tb.Text, post_cb.Text, login_tb.Text, password_tb.Text));
                        wnd.usersGrid.Items.Add(MainWindow.users_db[MainWindow.users_db.Count - 1]);
                    }
                    break;
                case "Изменить":
                    MainWindow.users_db[this.seleted_item] = new DataRow(fio_tb.Text, post_cb.Text, login_tb.Text, password_tb.Text);
                    wnd.usersGrid.Items[this.seleted_item] = MainWindow.users_db[this.seleted_item];
                    break;
                default:
                    break;
            }
            wnd.usersGrid.Items.Refresh();
            this.Close();
            
        }

        private void post_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
