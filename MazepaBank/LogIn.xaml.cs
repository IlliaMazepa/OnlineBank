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

namespace MazepaBank
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        ClientsDB clientsDB = new ClientsDB();

        public LogIn()
        {
            InitializeComponent();
            clientsDB.DownloadFromDb();
        }



        private void lblSignUp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SignUp SignUpWindow = new SignUp(clientsDB);
            Close();
            SignUpWindow.Show();
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            if (Checker())
            {
                MainWindow mainWindow = new MainWindow(clientsDB);
                Close();
                mainWindow.Show();
            }
            else
            {
                lblWPas.Opacity = 1;
            }




        }

        private bool Checker()
        {
            foreach(Client client in clientsDB.clients)
            {
                if (client.Number == tbNumber.Text.ToString() && client.Password.Trim() == tbPassword.Password.ToString().Trim())
                {
                    clientsDB.ActiveNumber = client.Number;
                    clientsDB.DownLoadTransactions();

                    return true;

                }
                
            }
            return false;
        }

        string TermDateGen()
        {
            string str = "";
            str = DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString().Substring(2);
            if (str.Length == 5) return str;
            else return $"0{str}";
        }

        //private void RadioButton_Click(object sender, RoutedEventArgs e)
        //{
        //    rbPasShow.IsChecked = !rbPasShow.IsChecked;
        //    if (rbPasShow.IsChecked==true)
        //    {
        //        tbPassword.Opacity = 0;
        //        tbPasswordSee.Text = tbPassword.Password;
        //        tbPasswordSee.Opacity = 1;
        //    }
        //    else
        //    {
        //        tbPassword.Opacity = 1;
        //        tbPassword.Password = tbPasswordSee.Text;
        //        tbPasswordSee.Opacity = 0;
        //    }

        //}

        private void ForPAss_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NewPassword newPassword = new NewPassword();
            newPassword.Show();
            Close();
        }


        private void btnLogIn_MouseEnter(object sender, MouseEventArgs e)
        {
            if (tbPassword.Password.Length < 8)
            {

                if (btnLogIn.Margin == new Thickness(90, 90, 90, 0))
                {
                    btnLogIn.Margin = new Thickness(90, 40, 90, 0);
                }
                else if (btnLogIn.Margin == new Thickness(90, 120, 90, 0))
                {
                    btnLogIn.Margin = new Thickness(90, 40, 90, 0);
                }
                else if (btnLogIn.Margin == new Thickness(90, 40, 90, 0))
                {
                    btnLogIn.Margin = new Thickness(90, 120, 90, 0);
                }
            }
            else
            {
                btnLogIn.Margin = new Thickness(90, 90, 90, 0);
            }
        }
    }
}
