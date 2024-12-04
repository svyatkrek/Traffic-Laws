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

namespace Traffic_Laws
{
	/// <summary>
	/// Логика взаимодействия для FinalWindow.xaml
	/// </summary>
	public partial class FinalWindow : Window
	{
		public FinalWindow(string type, int amountAnswer, int amountUncorrectAnswer, int amountCorrectAnswer, int _time)
		{
			InitializeComponent();
			string time = String.Concat((_time / 60).ToString(), " минут ", (_time - (_time / 60 * 60)).ToString(), " секунд");
			timeLabel.Content += time;
			if (type == "tickets")
			{
				mainLabel.Content += "Экзамен";
				if (amountAnswer - amountCorrectAnswer <= 1)
					imageFinal.Source = new BitmapImage(new Uri("images/passed.png", UriKind.Relative));
				else
				{
					imageFinal.Source = new BitmapImage(new Uri("images/unpassed.jpg", UriKind.Relative));
					_time = Int32.MaxValue;
				}
			}
			else
			{
				if (amountAnswer - amountCorrectAnswer <= 1)
					imageFinal.Source = new BitmapImage(new Uri("images/passed2.jpg", UriKind.Relative));
				else
				{
					imageFinal.Source = new BitmapImage(new Uri("images/unpassed2.png", UriKind.Relative));
					_time = Int32.MaxValue;
				}
				mainLabel.Content += "Зачет";
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
	}
}
