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
using System.Windows.Media.Effects;
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

        public string checkState = "";
        public int modeStart;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Rules_Button_Click(object sender, RoutedEventArgs e)
        {
			RulesWindow rules = new();
            rules.Show();
            this.Hide();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
			Button? button = e.Source as Button;

            if (button.Name == checkState)
            {
                checkState = "";
				button.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
			}
            else
            {
				Button oldActiveButton = (Button)this.FindName(checkState);
                if (oldActiveButton != null)
                    oldActiveButton.Background = button.Background;
                

				button.Background = new SolidColorBrush(Color.FromArgb(85, 0, 0, 0));

				checkState = button.Name;
			}

        }

		private void ButtonStart_Click(object sender, RoutedEventArgs e)
		{
			Button? button = e.Source as Button;

            if (button.Content.ToString() == "Экзамен")
                modeStart = 1;
            else
                modeStart = 2;

            if (checkState != "")
            {
                int category = Convert.ToInt32(checkState.Split('_')[1]);
                if (category != 3)
                {
					ExWindow exam = new(modeStart, category);
					exam.Show();
					this.Hide();
				}
                else
                {
                    StatisticsWindow statisticsWindow = new(modeStart);
                    statisticsWindow.Show();
                    this.Hide();
                }
                
			}

		}

		private void DataWindow_Closing(object sender, EventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}

// Для экзаменов берем tickets, рандомный билет из 40, смотреть на выбор категории
// Для зачета берем topics, открывается окно со всеми темами, человек выберает вопрос и будут вопросы по этому топику, если в теме больше 10 вопросов, то рандомно 10 вопросов, если меньше, то все
// Если больше 1 ошибки, то зачет или экзамен не сдан