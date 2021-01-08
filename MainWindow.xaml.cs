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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Globalization;

namespace Value_omregner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double valute1;
        double valute2;
        double totalvaluta;
        double endvalue;
        string resultat;
        string values;
        public class Valuta
        {
            public string code;
            public double rate;
            public override string ToString()
            {
                return $"{code}";
            }
        }
        List<Valuta> valutas = new List<Valuta>();
        public MainWindow()
        {

            InitializeComponent();
            var doc = new XmlDocument();
            doc.Load(@"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");

            XmlNodeList nodes = doc.SelectNodes("//*[@currency]");

            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var valuta = new Valuta()
                    {
                        code = node.Attributes["currency"].Value,
                        rate = double.Parse(node.Attributes["rate"].Value, NumberStyles.Any, new CultureInfo("en-Us"))
                    };
                    valutas.Add(valuta);
                }

                valutas.Add(new Valuta { code = "EUR", rate = 1 });
            }
            comboxleftside.ItemsSource = valutas;
            comboxrightside.ItemsSource = valutas;
        }

        private void valutaleftside_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (comboxleftside.Text.Length == 0 || comboxrightside.Text.Length == 0)
            {
                if (comboxleftside.Text.Length == 0 && comboxrightside.Text.Length !=0)
                {
                    MessageBox.Show("Du skal vælge en Valuta i den første Comobox");
                }
                else if (comboxrightside.Text.Length == 0 && comboxleftside.Text.Length != 0)
                {
                    MessageBox.Show("Du skal vælge en Valuta i den anden Comobox");
                }
                else
                {
                    MessageBox.Show("Du skal vælge en Valuta i den første og anden Comobox");
                }
            }
            else
            {
                if (valutaleftside.Text.Length != 0)
                {

                    if (comboxrightside.Text.Length != 0 && comboxleftside.Text.Length != 0)
                    {
                        foreach (Valuta va in valutas)
                        {
                            if (comboxleftside.Text == va.code && comboxleftside.Text.Length != 0)
                            {
                                valute1 = va.rate;

                            }
                        }
                        foreach (Valuta va in valutas)
                        {
                            if (comboxrightside.Text == va.code && comboxrightside.Text.Length != 0)
                            {
                                valute2 = va.rate;

                            }
                        }
                        totalvaluta = valute2 / valute1;
                        endvalue = totalvaluta * Convert.ToDouble(valutaleftside.Text);
                        resultat = Convert.ToString(Math.Round(endvalue, 2));
                        valutarightside.Text = resultat;
                        textbox.Text = $"{comboxleftside.Text} -> {comboxrightside.Text}";
                    }
                }
                else
                {
                    MessageBox.Show("Du mangler at indtaste en value");
                }
            }
        }
    }
}
