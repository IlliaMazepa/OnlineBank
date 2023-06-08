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
    /// Interaction logic for NewPassword.xaml
    /// </summary>
    public partial class NewPassword : Window
    {
        ClientsDB clientsDB = new ClientsDB();
        public NewPassword()
        {
            InitializeComponent();
            
            clientsDB.DownloadFromDb();
        }

        private void btnFindPass_Click(object sender, RoutedEventArgs e)
        {
            lblMess.Opacity = 0;
            int cor = 0;
            lblMess.Foreground = new SolidColorBrush(Colors.Red);
            tbSurName.Background = new SolidColorBrush(Colors.Transparent);
            tbName.Background = new SolidColorBrush(Colors.Transparent);
            tbNumber.Background = new SolidColorBrush(Colors.Transparent);
            if (tbName.Text != "" && tbSurName.Text != "" && tbNumber.Text.Length == 13)
            {
                
                foreach(Client client in clientsDB.clients)
                {
                    if (client.Name == tbName.Text.Trim()) cor++;
                    else
                    {
                        lblMess.Content = "Wrong fields";
                        lblMess.Opacity = 1;
                        tbName.Background = new SolidColorBrush(Colors.Red);
                    }
                    if(client.SurName == tbSurName.Text.Trim()) cor++;
                    else
                    {
                        lblMess.Content = "Wrong fields";
                        lblMess.Opacity = 1;
                        tbSurName.Background = new SolidColorBrush(Colors.Red);
                    }
                    if (client.Number == tbNumber.Text) cor++;
                    else
                    {
                        lblMess.Content = "Wrong fields";
                        lblMess.Opacity = 1;
                        tbNumber.Background = new SolidColorBrush(Colors.Red);
                    }
                    if (cor == 3)
                    {
                        lblMess.Content = $"Your password: {client.Password}";
                        lblMess.Opacity = 1;
                        lblMess.Foreground = new SolidColorBrush(Colors.Green);
                        break;
                    }
                }
            }
            else
            {
                lblMess.Content = "You have empty field";
                lblMess.Opacity = 1;
            }
            

        }

        private void tbNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsAllowedInput(e.Text))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private bool IsAllowedInput(string input)
        {
            // Check if the input is a digit or the "+" character
            return input.All(c => char.IsDigit(c) || c == '+');
        }

        private void btnLog_Click(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new LogIn();
            logIn.Show();
            Close();
        }
    }
}
