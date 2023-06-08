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
    /// Interaction logic for Transaction.xaml
    /// </summary>
    public partial class MWTransaction : Window
    {
        ClientsDB clientsDB;
        Client activeClient;
        Client recepient;
        string P2P = "P2P";
        public MWTransaction(ClientsDB _clientsDB)
        {
            InitializeComponent();
            clientsDB = _clientsDB;
            activeClient = clientsDB.ActiveClient();
            tbSender.Text = $"{activeClient.CardNumber.Substring(0, 4)} {activeClient.CardNumber.Substring(4, 4)} {activeClient.CardNumber.Substring(8, 4)} {activeClient.CardNumber.Substring(12)}";
        }

        private void btnSendMoney_Click(object sender, RoutedEventArgs e)
        {
            lblMes.Opacity = 0;
            string RecCard = tbRecipient.Text.Replace(" ", "");
            
            int r = -1;
            if (RecCard.Length == 16 && tbSender.Text.Replace(" ", "")==activeClient.CardNumber&& tbSender.Text.Replace(" ", "")!=RecCard)
            {
                foreach(Client client in clientsDB.clients)
                {
                    if(client.CardNumber == RecCard)
                    {
                        r = 10;
                        recepient = client;
                        break;
                    }
                }
                if (r == 10)
                {
                    if (Convert.ToDouble(tbSum.Text) > 1)
                    {
                        if(activeClient.Balance>= Convert.ToDouble(tbSum.Text))
                        {
                            MakeTransaction();
                        }
                        else
                        {
                            lblMes.Content = "Not enough balance";
                            lblMes.Opacity = 1;
                        }
                    }
                    else
                    {
                        lblMes.Content = "Incorrect summ";
                        lblMes.Opacity = 1;
                    }
                }
                else
                {
                    lblMes.Content = "Wrong card number";
                    lblMes.Opacity = 1;
                }
            }
            else
            {
                lblMes.Content = "Wrong card number";
                lblMes.Opacity = 1;
            }
        }

        private void tbRecipient_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsAllowedInput(e.Text))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private bool IsAllowedInput(string input)
        {
            return input.All(c => char.IsDigit(c)|| c == ' ');
        }
        private bool MIsAllowedInput(string input)
        {
            return input.All(c => char.IsDigit(c) || c == ',');
            
        }

        private void tbSum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!MIsAllowedInput(e.Text))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private void MakeTransaction()
        {
            if (ConfirmT())
            {
                MTransaction send = new MTransaction(activeClient.Number, "-", P2P, tbDescription.Text.Trim(), Convert.ToDouble(tbSum.Text));
                MTransaction rec = new MTransaction(recepient.Number, "+", P2P, tbDescription.Text.Trim(), Convert.ToDouble(tbSum.Text));
                clientsDB.transactions.Add(send); ;
                clientsDB.transactions.Add(rec);
                recepient.Balance += Convert.ToDouble(tbSum.Text);
                activeClient.Balance -= Convert.ToDouble(tbSum.Text);
                clientsDB.ChangeBalance(recepient.Number, recepient.Balance);
                clientsDB.ChangeBalance(activeClient.Number, activeClient.Balance);
                clientsDB.AddTrans(rec,send);
                //MessageBox.Show("Transaction successefuly");
                Close();
            }
            else
            {
                lblMes.Content = "Transaction has been canceled";
                lblMes.Opacity = 1;
            }
        }

        private bool ConfirmT()
        {
            if (MessageBox.Show($"\nSender:\n{activeClient.Name}  {activeClient.SurName}\nSumm: {Convert.ToDouble(tbSum.Text)}\n\nRecepient:\n{recepient.Name}  {recepient.SurName}", "Transaction conformation", MessageBoxButton.OKCancel) == MessageBoxResult.OK) return true;
            else return false;
        }
    }
}
