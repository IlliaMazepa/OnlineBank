using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
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
    /// Interaction logic for DepositWindow.xaml
    /// </summary>
    public partial class DepositWindow : Window
    {
        ClientsDB clientsDB;
        Timer timer;
        public DepositWindow(ClientsDB _clientsDB)
        {
            InitializeComponent();
            try
            {
                clientsDB = _clientsDB;
                Loaded += DepositWindow_Loaded;
                
                
                //UpdateJars();
                //MessageBox.Show(clientsDB.transactions.Capacity.ToString());
                UpdateJars();

            }
            catch 
            {

            }
            
            
        }

        private void DepositWindow_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        private void TimerCallback(object state)
        {
            // Run the MessageBox on the UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                //clientsDB.DownLoadTransactions();
                //clientsDB.DownLoadJars();
                UpdateJars();
                

            });
        }

        private void UpdateJars()
        {
            lbJars.Items.Clear();
           clientsDB.DownLoadTransactionsJar();
            clientsDB.DownLoadJars();
            foreach (Jar jar in clientsDB.jars)
            {
                jar.transactions.Clear();
                
                
                foreach (MTransaction item in clientsDB.Jtrans)
                {
                    if(item.TName.Trim()==jar.IDName.Trim())jar.transactions.Add(item);
                }
                ListBoxItem listBoxItem = new ListBoxItem();
                string creatorName = "";
                foreach(Client client in clientsDB.clients)
                {
                    if (client.Number == jar.CreatorNumber)
                    {
                        creatorName = client.Name + $" {client.SurName}";
                        break;
                    }
                    else
                    {
                        creatorName = "No info";
                    }
                }
                jar.Balance = 0;
                foreach (MTransaction transaction in jar.transactions)
                {
                    jar.Balance += transaction.Sum;
                }
                clientsDB.ChangeBalanceJar(jar.CreatorNumber, jar.Balance);

                listBoxItem.Content = $"Creator name: {creatorName}\tDescription: {jar.Description}\tCurrent balance: {jar.Balance}";
                listBoxItem.MouseLeftButtonUp += listBoxItem_MouseLeftButtonUp;
                lbJars.Items.Add(listBoxItem);
            }
        }

        private void listBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            JarPay jarPay = new JarPay(lbJars.ItemContainerGenerator.IndexFromContainer((ListBoxItem)sender), clientsDB);
            jarPay.Show();
        }

        private void btnCreateDep_Click(object sender, RoutedEventArgs e)
        {
            int temp = -1;
            foreach(Jar jar in clientsDB.jars)
            {
                if (jar.CreatorNumber == clientsDB.ActiveNumber)
                {
                    temp = 10;
                    MessageBox.Show("You already have Deposit");
                    break;
                }
            }
            if (temp == -1)
            {
                Jar NJ = new Jar(clientsDB.ActiveNumber,$"Jar{clientsDB.ActiveNumber}raJ",0,tbDes.Text,null);
                clientsDB.AddJar(NJ);
            }
            
        }
    }
}
