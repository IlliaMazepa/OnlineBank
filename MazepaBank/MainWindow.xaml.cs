using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace MazepaBank
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClientsDB clientsDB = null;
        Client activeClient = null;
        private Timer timer;
        private string _fileName = "Exchanges.xml";
        private XDocument _xDocument;
        

        public MainWindow(ClientsDB _clientsDB)
        {
            InitializeComponent();
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange", _fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tbCurrency.Text = "Error";
                //Close();
            }
            _xDocument = XDocument.Load(_fileName);
            foreach (XElement item in _xDocument.Element("exchange").Elements("currency"))
            {
                if(item.Element("cc").Value=="USD")tbCurrency.Text = double.Parse(item.Element("rate").Value.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)).ToString();
            }
            
            try
            {
                clientsDB = _clientsDB;
                activeClient = clientsDB.ActiveClient();
                btnLog.Content = $"Hello {activeClient.Name}";
                tbCardNumber.Text = $"{activeClient.CardNumber.Substring(0, 4)} {activeClient.CardNumber.Substring(4, 4)} {activeClient.CardNumber.Substring(8, 4)} {activeClient.CardNumber.Substring(12)}"; 
                tbBalance.Text = Math.Round(activeClient.Balance, 2).ToString();
                UpdateTL();


                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogIn logIn = new LogIn();
                Close();
                logIn.Show();
            }
            Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Create a new timer that triggers every 10 seconds
            timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        void UpdateTL()
        {
            lbTrans.Items.Clear();
            foreach (MTransaction transaction in clientsDB.transactions)
            {
                ListBoxItem boxItem = new ListBoxItem();
                boxItem.Content = transaction.ToString();
                if (transaction.Type == "+") boxItem.Foreground = new SolidColorBrush(Colors.Green);
                else boxItem.Foreground = new SolidColorBrush(Colors.Red);
                boxItem.MouseLeftButtonUp += BoxItem_MouseLeftButtonDown;
                lbTrans.Items.Add(boxItem);

            }
        }

        private void BoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TransactioDetails transactioDetails = new TransactioDetails(lbTrans.ItemContainerGenerator.IndexFromContainer((ListBoxItem)sender), clientsDB);
            transactioDetails.Show();
        }

        private void TimerCallback(object state)
        {
            // Run the MessageBox on the UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                clientsDB.DownloadFromDb();
                clientsDB.DownLoadTransactions();
                clientsDB.ActiveNumber = activeClient.Number;
                activeClient = clientsDB.ActiveClient();
                tbBalance.Text = activeClient.Balance.ToString();
                UpdateTL();
            });
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void brnSend_Click(object sender, RoutedEventArgs e)
        {
            MWTransaction transaction = new MWTransaction(clientsDB);
            transaction.Show();
        }

        private void btnLog_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Do you want Log out?","Confirm action", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                LogIn log = new LogIn();
                log.Show();
                Close();
            }
            
        }

        private void btnCurr_Click(object sender, RoutedEventArgs e)
        {
            CurrensyWindow currensy = new CurrensyWindow();
            currensy.Show();
        }

        private void btnCredit_Click(object sender, RoutedEventArgs e)
        {
            CreditWindow credit = new CreditWindow(clientsDB);
            credit.Show();
        }

        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            DepositWindow depositWindow = new DepositWindow(clientsDB);
            depositWindow.Show();
        }
    }
}
