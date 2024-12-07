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
using Traffic_Laws.src;

namespace Traffic_Laws
{
    /// <summary>
    /// Логика взаимодействия для FinalWindow.xaml
    /// </summary>
    public partial class FinalWindow : Window
	{

		private const string ExamString = "ЭКЗАМЕН";
		private const string TestString = "ЗАЧЕТ";
		private const string PathToImagePassed = "/images/ExamPassed.png";
		private const string PathToImageFailed = "/images/ExamFailed.png";
		public FinalWindow(string type, int amountAnswer, int amountUncorrectAnswer, int amountCorrectAnswer, int _time)
		{
			InitializeComponent();
			string time = String.Concat((_time / 60).ToString(), " минут ", (_time - (_time / 60 * 60)).ToString(), " секунд");
			timeLabel.Content += time;
			if (amountAnswer - amountUncorrectAnswer <= 1)
			{
				imageFinal.Source = new BitmapImage(new Uri(PathToImagePassed, UriKind.Relative));
				passedLabel.Visibility = Visibility.Visible;
			}
			else
			{
				imageFinal.Source = new BitmapImage(new Uri(PathToImageFailed, UriKind.Relative));
				_time = Int32.MaxValue;
				unPassedLabel.Visibility = Visibility.Visible;
			}
			if (type == "tickets")
			{
				mainLabel.Content = string.Format(mainLabel.Content.ToString(), ExamString);
				passedLabel.Content = string.Format(passedLabel.Content.ToString(), ExamString);
				unPassedLabel.Content = string.Format(unPassedLabel.Content.ToString(), ExamString);
			}
			else
			{
				mainLabel.Content = string.Format(mainLabel.Content.ToString(), TestString);
				passedLabel.Content = string.Format(passedLabel.Content.ToString(), TestString);
				unPassedLabel.Content = string.Format(unPassedLabel.Content.ToString(), TestString);
			}

			

			amountLabel.Content += amountAnswer.ToString();
			amountUncorrectLabel.Content += amountUncorrectAnswer.ToString();
			amountCorrectLabel.Content += amountCorrectAnswer.ToString();
				

			Logs logs = new();
			logs.Save(type, amountCorrectAnswer.ToString(), amountUncorrectAnswer.ToString(), _time.ToString());
		}

		private void Menu_Button_Click(object sender, RoutedEventArgs e)
		{
			MainWindow menu = new();
			menu.Show();
			this.Hide();
		}

		private void DataWindow_Closing(object sender, EventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void HideButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.WindowState = WindowState.Minimized;
		}

		private void Drag(object sender, RoutedEventArgs e)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				this.DragMove();
			}
		}
	}
}
