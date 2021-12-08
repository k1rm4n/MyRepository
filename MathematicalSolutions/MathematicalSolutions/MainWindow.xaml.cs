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

namespace MathematicalSolutions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public int a = 0;
        public int b = 0;

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            a = random.Next(0, 100);
            first_number.Content = a.ToString();

            b = random.Next(0, 100);
            second_number.Content = b.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int res = a - b;
            if (textBox.Text == res.ToString())
                MessageBox.Show($"Правильно! \nПравильный результат: {res}");
            else
                MessageBox.Show($"Вы ошиблись! \nПравильный результат: {res}");
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            double x = Convert.ToDouble(place.Text);
            double res = Math.Log(Math.Cos(x)) / Math.Log(1 + Math.Pow(x, 2));
            res_textBox.Text = res.ToString();

        }
    }
}
