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
    /// Interaction logic for TransactioDetails.xaml
    /// </summary>
    public partial class TransactioDetails : Window
    {
        MTransaction transaction = null;
        public TransactioDetails(int index, ClientsDB dB)
        {
            InitializeComponent();

            transaction = dB.transactions[index];
            if (transaction.Type == "+") lblType.Content = "Income";
            else lblType.Content = "Loss";
            lblName.Content = transaction.TName;
            lblDes.Content = transaction.Description;
            lblSum.Content = transaction.Sum.ToString() + " grn";
        }
    }
}
