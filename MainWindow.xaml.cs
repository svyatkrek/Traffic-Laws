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
        public string activeButtonCheck = "";
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



		private void ButtonCheck_Click(object sender, RoutedEventArgs e)
		{
            Button button = e.Source as Button;
            button.Foreground = Brushes.Green;

            if (checkedButton1.Content.ToString() == activeButtonCheck) {
                checkedButton1.Foreground = Brushes.White;
            }
			if (checkedButton2.Content.ToString() == activeButtonCheck)
			{
				checkedButton2.Foreground = Brushes.White;
			}
			if (checkedButton3.Content.ToString() == activeButtonCheck)
			{
				checkedButton3.Foreground = Brushes.White;
			}
            activeButtonCheck = button.Content.ToString();

		}
	}
}

// Для экзаменов берем tickets, рандомный билет из 40, смотреть на выбор категории
// Для зачета берем topics, открывается окно со всеми темами, человек выберает вопрос и будут вопросы по этому топику, если в теме больше 10 вопросов, то рандомно 10 вопросов, если меньше, то все
// Если больше 1 ошибки, то зачет или экзамен не сдан