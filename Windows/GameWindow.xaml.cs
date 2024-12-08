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

namespace Traffic_Laws.Windows
{
	/// <summary>
	/// Логика взаимодействия для GameWindow.xaml
	/// </summary>
	public partial class GameWindow : Window
	{
		public GameWindow()
		{
			InitializeComponent();
			WebView.Source = new Uri("https://igroutka.ru/loader/game/45766/");
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
				this.DragMove();
		}
	}
}
