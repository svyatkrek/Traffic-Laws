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
	/// Логика взаимодействия для StatisticsWindow.xaml
	/// </summary>
	public partial class StatisticsWindow : Window
	{
		public StatisticsWindow(string type)
		{
			InitializeComponent();
			Logs logs = new();
			if (type == "tickets")
				amountLabel.Content += "экзаменов: ";
			else
				amountLabel.Content += "зачетов: ";
			if ( logs.Load(type) )
			{
				amountLabel.Content += logs.AmountAttempts;
				amountUncorrectLabel.Content += logs.AmountUncorrect;
				amountCorrectLabel.Content += logs.AmountCorrect;
				timeLabel.Content += logs.BestTime;
			}
			else
			{
				mainLabel.Content = "Нет информации :c";
				amountLabel.Content = string.Empty;
				amountUncorrectLabel.Content = string.Empty;
				amountCorrectLabel.Content = string.Empty;
				timeLabel.Content = string.Empty;
			}
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


		private void hideButton_Click(object sender, RoutedEventArgs e)
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
