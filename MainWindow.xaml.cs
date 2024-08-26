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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Traffic_Laws
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

        private void Rules_Button_Click(object sender, RoutedEventArgs e)
        {
            Rules rules = new();
            rules.Show();
            this.Hide();
        }

        private void RadioButton_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            bool? isCheck1 = r1b.IsChecked;
            if (isCheck1 == true)
            {

                RadioButton rb = e.Source as RadioButton;
                rb.Foreground = Brushes.Green;
            }
        }

        private void b_category_button_Checked(object sender, RoutedEventArgs e)
        {
            bool? isCheck1 = b_category_button.IsChecked;
            bool? isCheck2 = c_category_button.IsChecked;
            if(isCheck1 == true && isCheck2 == false)
            {
                RadioButton rb = e.Source as RadioButton;
                rb.Foreground = Brushes.Green;
            }
            else
            {
                c_category_button.IsChecked = false;
                RadioButton rb = e.Source as RadioButton;
                rb.Foreground = Brushes.Green;
            }
        }

        private void c_category_button_Checked(object sender, RoutedEventArgs e)
        {
            bool? isCheck1 = b_category_button.IsChecked;
            bool? isCheck2 = c_category_button.IsChecked;
            if (isCheck1 == false && isCheck2 == true)
            {
                RadioButton rb = e.Source as RadioButton;
                rb.Foreground = Brushes.Green;
            }
            else
            {
                b_category_button.IsChecked = false;
                RadioButton rb = e.Source as RadioButton;
                rb.Foreground = Brushes.Green;
            }
        }
    }
}

// Для экзаменов берем tickets, рандомный билет из 40, смотреть на выбор категории
// Для зачета берем topics, открывается окно со всеми темами, человек выберает вопрос и будут вопросы по этому топику, если в теме больше 10 вопросов, то рандомно 10 вопросов, если меньше, то все
// Если больше 1 ошибки, то зачет или экзамен не сдан