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
    /// Interaction logic for JarPay.xaml
    /// </summary>
    public partial class JarPay : Window
    {
        Jar jar;
        ClientsDB clientsDB;
        public JarPay(int index,ClientsDB _clientsDB)
        {
            InitializeComponent();
            clientsDB = _clientsDB;
            jar = clientsDB.jars[index];
            lblInfo.Content = jar.Description;
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(tbSum.Text) <= clientsDB.ActiveClient().Balance)
            {
                clientsDB.AddTrans(new MTransaction(clientsDB.ActiveNumber,"-",$"Jar{jar.CreatorNumber}raJ","", Convert.ToDouble(tbSum.Text)));

                clientsDB.ChangeBalance(clientsDB.ActiveNumber, clientsDB.ActiveClient().Balance-Convert.ToDouble(tbSum.Text));
                MessageBox.Show("Thank you for donate");
                Close();
            }
            else
            {
                MessageBox.Show("not enough money");
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
            return input.All(c => char.IsDigit(c));
        }
    }
}
