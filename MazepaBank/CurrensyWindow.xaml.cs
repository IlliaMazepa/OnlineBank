using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
using System.Xml.Linq;

namespace MazepaBank
{
    /// <summary>
    /// Interaction logic for CurrensyWindow.xaml
    /// </summary>
    public partial class CurrensyWindow : Window
    {
        private string _fileName = "Exchanges.xml";
        private XDocument _xDocument;
        private double _rate;
        public CurrensyWindow()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double sum = double.Parse(tbSum.Text.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                lblRes.Content = $"Result {sum * _rate} grn.";

                //OpenWeather openWeather = new OpenWeather();
                //lblResult.Text = openWeather.GetWeather();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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

                Close();
            }

            _xDocument = XDocument.Load(_fileName);
            foreach (XElement item in _xDocument.Element("exchange").Elements("currency"))
            {
                cbCur.Items.Add(item.Element("txt").Value);
            }
            cbCur.SelectedIndex = 0;
        }

        private void cbCur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                XElement element = _xDocument.Element("exchange").Elements("currency").First(el => el.Element("txt").Value == cbCur.SelectedItem.ToString());

                _rate = double.Parse(element.Element("rate").Value.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

            }
            catch { }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnConvert.IsEnabled = !string.IsNullOrWhiteSpace(tbSum.Text);

        }
    }
}
