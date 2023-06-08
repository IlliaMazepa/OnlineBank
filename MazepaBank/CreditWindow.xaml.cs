using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for CreditWindow.xaml
    /// </summary>
    public partial class CreditWindow : Window
    {
        Credit credit;
        ClientsDB clientsDB;
        private Timer timer;
        public CreditWindow(ClientsDB _clientsDB)
        {
            InitializeComponent();
            try
            {
                clientsDB = _clientsDB;
                credit = _clientsDB.credit;
            }
            catch
            {

            }
            
            
            Loaded += CreditWindow_Loaded;
        }

        private void CreditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Create a new timer that triggers every 10 seconds
            timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        private void TimerCallback(object state)
        {
            // Run the MessageBox on the UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                clientsDB.credit = null;
                clientsDB.DownLoadCredits();
                credit = clientsDB.credit;
                if (credit != null)
                {
                    lblCurrentCredit.Content = credit.Balance;
                }
                else
                {
                    lblCurrentCredit.Content = 0;
                }
                UpdateCredit();
            });
        }



        private void btnFullSumm_Click(object sender, RoutedEventArgs e)
        {
            if (clientsDB.ActiveClient().Balance >= credit.Balance)
            {
                tbCreditPaySumm.Text = credit.Balance.ToString();
            }
            else
            {
                MessageBox.Show("Not enough balance");
            }
        }

        private void btnPayCredit_Click(object sender, RoutedEventArgs e)
        {
            if (clientsDB.ActiveClient().Balance >= Convert.ToDouble(tbCreditPaySumm.Text))
            {
                
                credit.Balance -= Convert.ToDouble(tbCreditPaySumm.Text);
                clientsDB.ChangeBalance(clientsDB.ActiveNumber,clientsDB.ActiveClient().Balance - Convert.ToDouble(tbCreditPaySumm.Text));
                credit.LastDate = DateTime.Now.Date;
                clientsDB.AddTrans(new MTransaction(clientsDB.ActiveNumber, "-", "Credit payment", $"Payment for credit\nDate: {DateTime.Now.Date}", Convert.ToDouble(tbCreditPaySumm.Text)));
                lblCurrentCredit.Content = credit.Balance.ToString();
                if (credit.Balance > 0)
                {
                    clientsDB.ChangeCreditBalance(clientsDB.ActiveNumber, credit);
                }
                else
                {
                    clientsDB.DelCred(clientsDB.ActiveNumber);
                    credit = null;
                }
            }
            else
            {
                MessageBox.Show("Not enough balance");
            }
        }

        private void btnTakeLoan_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(tbCreditTakeSumm.Text) >= 100&& Convert.ToDouble(tbCreditTakeSumm.Text)<10000000)
            {
                if(Convert.ToInt32(tbCreditTakePeriod.Text)>0&& Convert.ToInt32(tbCreditTakePeriod.Text) <= 60)
                {
                    double per = 0;
                    if (Convert.ToInt32(tbCreditTakePeriod.Text) > 0 && Convert.ToInt32(tbCreditTakePeriod.Text) < 13) per = 12;
                    else if (Convert.ToInt32(tbCreditTakePeriod.Text) > 12 && Convert.ToInt32(tbCreditTakePeriod.Text) < 25) per = 10;
                    else if (Convert.ToInt32(tbCreditTakePeriod.Text) > 24 && Convert.ToInt32(tbCreditTakePeriod.Text) < 61) per = 8;
                    lblCreditPercent.Content = per;
                    //Thread.Sleep(2000);
                    credit = new Credit(clientsDB.ActiveNumber, Convert.ToDouble(tbCreditTakeSumm.Text), DateTime.Now.Date, per, DateTime.Now.Date);
                    clientsDB.AddCredit(credit);
                    clientsDB.ChangeBalance(clientsDB.ActiveNumber, clientsDB.ActiveClient().Balance + credit.Balance);
                    clientsDB.AddTrans(new MTransaction(clientsDB.ActiveNumber,"+","Credit",$"Credit:\nSumm:{credit.Balance}\nLast payment: {credit.LastDate.ToShortDateString()}\nPErcent: {credit.Percent}", credit.Balance));

                }
                else
                {
                    btnTakeLoan.Content = "Wrong period(0-60)";
                    btnTakeLoan.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                btnTakeLoan.Content = "Wrong summ(100-10kk)";
                btnTakeLoan.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void tbCreditTakeSumm_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsAllowedInput(e.Text))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private bool IsAllowedInput(string input)
        {
            // Check if the input is a digit or the "+" character
            return input.All(c => char.IsDigit(c));
        }

        private void UpdateCredit()
        {
            if (Convert.ToDouble(lblCurrentCredit.Content) > 0)
            {
                PayCreditBlock.IsEnabled = true;
                PayCreditBlock.Opacity = 1;
                btnPayCredit.Opacity = 1;
                btnPayCredit.IsEnabled = true;
                TakeCreditBlock.IsEnabled = false;
                TakeCreditBlock.Opacity = 0;
            }
            else
            {
                PayCreditBlock.IsEnabled = false;
                PayCreditBlock.Opacity = 0;
                btnPayCredit.Opacity = 0;
                btnPayCredit.IsEnabled = false;
                TakeCreditBlock.IsEnabled = true;
                TakeCreditBlock.Opacity = 1;
            }
        }
    }
}
