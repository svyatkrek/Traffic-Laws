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
using System.Xml.Schema;
using Traffic_Laws.Entity;
using Traffic_Laws.src;

namespace Traffic_Laws.Windows
{
    /// <summary>
    /// Логика взаимодействия для VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {
		private VideoService? Service = new();
        public VideoWindow()
        {
            InitializeComponent();
			CreateComponents();


		}

		private void CreateComponents()
		{
			for (int i = 0; i < Service.Data.Videos.Count; i++)
			{
				Button button = new()
				{
					Background = new SolidColorBrush(Colors.Transparent),
					BorderBrush = new SolidColorBrush(Colors.Transparent),
					Style = (Style)this.FindResource("NoColorHover"),
					Name = String.Concat("ButtonVideo_", i.ToString())

				};
				button.Click += new RoutedEventHandler(ButtonVideo_Click);
				Border border = new()
				{
					CornerRadius = new CornerRadius(25),
					Background = new ImageBrush(new BitmapImage(new Uri(Service.Data.Videos[i].ImageURL ?? ""))),
					Margin = new Thickness(15, 15, 15, 15),
					Child = button,
				};
				Border border2 = new()
				{
					Width = 365,
					Height = 255,
					CornerRadius = new CornerRadius(25),
					Background = new SolidColorBrush(Color.FromRgb(57, 83, 174)),
					Margin = new Thickness(15, 15, 15, 15),
					Child = border,
				};
				TextBlock textBlock = new()
				{
					Text = Service.Data.Videos[i].Name,
					Background = new SolidColorBrush(Colors.Transparent),
					TextAlignment = TextAlignment.Center,
					TextWrapping = TextWrapping.Wrap,
					Foreground = new SolidColorBrush(Colors.Black),
					FontSize = 30,
					FontFamily = (FontFamily)this.FindResource("AlumniSansRegular"),
					Margin = new Thickness(0,0,0,20),
				};
				StackPanel stackPanel = new()
				{
					Width = 400,
					Orientation = Orientation.Vertical,
				};
				stackPanel.Children.Add(border2);
				stackPanel.Children.Add(textBlock);
				MainPanel.Children.Add(stackPanel);
			}

		}

		private void ButtonVideo_Click(object sender, EventArgs e)
		{
			Button button = sender as Button;
			int id =  Convert.ToInt32(button.Name.Split('_')[1]);
			VideoGrid.Visibility = Visibility.Visible;
			VideoElement.Source = new Uri(Service.Data.Videos[id].VideoURL ?? "");
			VideoElement.ZoomFactor = 1.6;

		}

		private void SimulateKeyPress(Key key)
		{
			var keyDownEvent = new KeyEventArgs(Keyboard.PrimaryDevice,
												PresentationSource.FromVisual(this),
												0,
												key)
			{
				RoutedEvent = Keyboard.KeyDownEvent
			};

			InputManager.Current.ProcessInput(keyDownEvent);
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

		private void Menu_Button_Click(object sender, RoutedEventArgs e)
		{
			MainWindow menu = new();
			menu.Show();
			this.Hide();
		}

		private void ExitVideoButton_Click(object sender, RoutedEventArgs e)
		{
			VideoGrid.Visibility = Visibility.Collapsed;
			VideoElement.CoreWebView2?.Navigate("about:blank");
		}
	}
}
