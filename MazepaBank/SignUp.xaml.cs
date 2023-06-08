using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        ClientsDB clientsDB;
        List<TextBox> textBoxes = new List<TextBox>();
        public SignUp(ClientsDB _clientsDB)
        {
            InitializeComponent();
            clientsDB = _clientsDB;
            textBoxes.Add(tbName);
            textBoxes.Add(tbSurName);
            textBoxes.Add(tbNumber);
            textBoxes.Add(tbPasswordFirst);
            textBoxes.Add(tbPasswordSecond);
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox box in textBoxes)
            {
                
                    box.Background = new SolidColorBrush(Colors.DimGray);
                
            }
            int temp = -1;
            foreach (TextBox box in textBoxes)
            {
                if (box.Text == "")
                {
                    box.Background = new SolidColorBrush(Colors.Red);
                    temp += 1;
                }
            }

            foreach (Client client in clientsDB.clients)
            {
                if (tbNumber.Text == client.Number)
                {
                    MessageBox.Show("The client with this number already exist");
                    tbNumber.Text = "+380";
                    temp += 1;
                    break;
                }
            }
            if (tbNumber.Text.Length != 13 || Regex.Matches(tbNumber.Text, "\\+").Count != 1)
            {
                tbNumber.Background = new SolidColorBrush(Colors.Red);
                temp += 1;
            }
            if (tbPasswordFirst.Text != tbPasswordSecond.Text || tbPasswordFirst.Text.Length < 8 || tbPasswordSecond.Text.Length < 8)
            {
                tbPasswordFirst.Background = new SolidColorBrush(Colors.Red);
                tbPasswordSecond.Background = new SolidColorBrush(Colors.Red);
                temp += 2;
            }

            if (temp != -1) MessageBox.Show("You didn`t fill all fields!!!");


            if (temp == -1)
            {
                try
                {
                    Client NC = new Client(tbName.Text.Trim(), tbSurName.Text.Trim(), tbThirdName.Text.Trim(), tbPasswordFirst.Text.Trim(), tbNumber.Text, CardGenerat());
                    clientsDB.clients.Add(NC);
                    clientsDB.ActiveNumber = tbNumber.Text;
                    clientsDB.AddNewClient($"INSERT INTO Clients (Name, SurName, ThirdName, Number, Password, CardNumber, CVCode, TerminateDate, Balance) VALUES ('{NC.Name}', '{NC.SurName}', '{NC.ThirdName}', '{NC.Number}', '{NC.Password}', '{NC.CardNumber}', '{NC.CVCode}', '{NC.TerminateDate}', {NC.Balance})");
                    MainWindow mainWindow = new MainWindow(clientsDB);
                    Close();
                    mainWindow.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
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

        private string CardGenerat()
        {
            string card="";
            Random random = new Random();
            int a = -1;
            for(; ; )
            {
                for(; card.Length<16;)
                {
                    card+=random.Next(0,10).ToString();
                }
                foreach(Client client in clientsDB.clients)
                {
                    if (client.CardNumber.Trim() == card)
                    {
                        a = 10;
                    }
                }
                if (a == -1) break;
            }
            return card;
        }
    }
}
