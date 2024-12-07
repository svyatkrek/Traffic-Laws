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
using Traffic_Laws.Entity;
using Traffic_Laws.src;

namespace Traffic_Laws
{
    /// <summary>
    /// Логика взаимодействия для Rules.xaml
    /// </summary>
    public partial class RulesWindow : Window
    {
		private RulesService Service = new();
		private const string AppendixesFlag = "appendixes";
		private const string RulesFlag = "rules";

		public RulesWindow()
        {
            InitializeComponent();
			CreateChapterMenu();


		}

		private void CreateChapterMenu()
		{
			int count = Service.Data.Rules.Count + Service.Data.Appendixes.Count;

			for (int i = 0; i < count; i++)
			{
				int customIndex;
				string flag;

				Border border = new()
				{
					BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
					BorderThickness = new Thickness(0, 0, 0, 3)
				};

				if (i >= Service.Data.Rules.Count)
				{
					customIndex = i - Service.Data.Rules.Count;
					flag = AppendixesFlag;

					Button button = new()
					{
						FontSize = 20,
						Content = Service.Data.Appendixes[customIndex].Name,
						Name = string.Format("ButtonMenu_{0}_{1}", flag, customIndex.ToString()),
						Background = new SolidColorBrush(Color.FromArgb(0, 163, 184, 217)),
						Foreground = Brushes.White,
					};
					border.Child = button;
				}
				else
				{
					customIndex = i;
					flag = RulesFlag;
					Button button = new()
					{
						FontSize = 20,
						Content = Service.Data.Rules[customIndex].Name,
						Name = string.Format("ButtonMenu_{0}_{1}", flag, customIndex.ToString()),
						Background = new SolidColorBrush(Color.FromArgb(0, 163, 184, 217)),
						Foreground = Brushes.White,
					};
					border.Child = button;
				}	
				//button.Click += new RoutedEventHandler(PaginationButton_Click);
				RulesPanel.Children.Add(border);
				
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
