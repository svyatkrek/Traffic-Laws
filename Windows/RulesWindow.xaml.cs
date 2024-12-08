using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using static System.Net.Mime.MediaTypeNames;

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
		private int CurrentOpenIndexAppendix = -1;
		private Button? SelectedButton = null;
		public RulesWindow()
        {
            InitializeComponent();
			CreateChapterMenu();
		}

		private void DrawingTextBlock(StackPanel parentElement, int fontSize, string text, FontWeight fontWeight)
		{
			if (text != String.Empty)
			{
				TextBlock textBlock = new()
				{
					FontSize = fontSize,
					TextWrapping = TextWrapping.Wrap,
					FontWeight = fontWeight,
					Text = text,
					Foreground = new SolidColorBrush(Colors.Black),
					Margin = new Thickness(0, 0, 0, 15),
				};
				parentElement.Children.Add(textBlock);
			}
		}

		private void DrawingRulesPageByIndex(int index)
		{
			PagePanel.Children.Clear();
			DrawingTextBlock(PagePanel, 42, Service.Data.Rules[index].Title ?? String.Empty, FontWeights.Bold);
			DrawingTextBlock(PagePanel, 20, Service.Data.Rules[index].Text ?? String.Empty, FontWeights.Normal);
		}

		private void DrawingAppendixesPageByIndex(int index, int indexSubItem)
		{
			PagePanel.Children.Clear();
			DrawingTextBlock(PagePanel, 42, Service.Data.Appendixes[index].Items[indexSubItem].Title ?? String.Empty, FontWeights.Bold);

			for (int i = 0; i < Service.Data.Appendixes[index].Items[indexSubItem].Contents.Count; ++i)
			{
				DrawingTextBlock(PagePanel, 20, Service.Data.Appendixes[index].Items[indexSubItem].Contents[i].Text ?? String.Empty, FontWeights.Normal);

				if (Service.Data.Appendixes[index].Items[indexSubItem].Contents[i].ImageURL != String.Empty)
				{
					System.Windows.Controls.Image image = new()
					{
						Source = new BitmapImage(new Uri(Service.Data.Appendixes[index].Items[indexSubItem].Contents[i].ImageURL ?? String.Empty)),
						Stretch = Stretch.None,
						Margin = new Thickness(0,0,0,15)
					};
					PagePanel.Children.Add(image);
				}
			}
		}

		private void MakeVisibleByIndex(int index, Visibility visibility)
		{
			List<Border> borders = RulesPanel.Children.OfType<Border>().ToList();

			foreach (Border border in borders)
				if (Convert.ToInt32(border.Name.Split("_")[2]) == index && border.Name.Split("_")[1] == AppendixesFlag && Convert.ToInt32(border.Name.Split("_")[3]) != 1000)
				{
					border.Visibility = visibility;
					if (visibility == Visibility.Visible && Convert.ToInt32(border.Name.Split("_")[3]) == 0)
					{
						Button firstSubItem = (Button)border.Child;
						firstSubItem.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
					}
				}
			
		}

		private void ChapterMenu_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (SelectedButton != null)
				SelectedButton.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
			
			button.Background = new SolidColorBrush(Color.FromRgb(85, 207, 214));
			SelectedButton = button;

			string flag = button.Name.Split("_")[1];
			int index = Convert.ToInt32(button.Name.Split("_")[2]);
			int indexSubItem = Convert.ToInt32(button.Name.Split("_")[3]);

			if (flag == AppendixesFlag && indexSubItem == 1000)
				if (CurrentOpenIndexAppendix != index)
				{
					MakeVisibleByIndex(index, Visibility.Visible);
					MakeVisibleByIndex(CurrentOpenIndexAppendix, Visibility.Collapsed);
					CurrentOpenIndexAppendix = index;
				}
				else
				{
					MakeVisibleByIndex(CurrentOpenIndexAppendix, Visibility.Collapsed);
					CurrentOpenIndexAppendix = -1;
				}
			else
				if (flag == RulesFlag)
					DrawingRulesPageByIndex(index);
				else
					DrawingAppendixesPageByIndex(index, indexSubItem);
		}

		private void CreateChapterMenuItem(int fontSize, string flag, string text, int customIndex, int appendixesIndex)
		{
			Visibility visibility = Visibility.Visible;
			if (appendixesIndex != 1000)
				visibility = Visibility.Collapsed;

			TextBlock textBlock = new()
			{
				FontSize = fontSize,
				Foreground = Brushes.White,
				Background = new SolidColorBrush(Color.FromArgb(0, 163, 184, 217)),
				Text = text,
				TextWrapping = TextWrapping.Wrap,
				TextAlignment = TextAlignment.Center,
				Margin = new Thickness(10, 0, 10, 0),
			};
			Button button = new()
			{
				Name = string.Format("ButtonMenu_{0}_{1}_{2}", flag, customIndex.ToString(), appendixesIndex.ToString()),
				Background = new SolidColorBrush(Color.FromArgb(0, 163, 184, 217)),
				Content = textBlock
			};
			Border border = new()
			{
				Name = string.Format("BorderButtonMenu_{0}_{1}_{2}", flag, customIndex.ToString(), appendixesIndex.ToString()),
				BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
				BorderThickness = new Thickness(0, 0, 0, 3),
				Child = button,
				Visibility = visibility
			};
			button.Click += new RoutedEventHandler(ChapterMenu_Click);
			RulesPanel.Children.Add(border);
		}

		private void CreateChapterMenu()
		{
			for (int i = 0; i < Service.Data.Rules.Count + Service.Data.Appendixes.Count; i++)
			{
				int customIndex;
				string flag;
				string text;
				if (i >= Service.Data.Rules.Count)
				{
					customIndex = i - Service.Data.Rules.Count;
					flag = AppendixesFlag;
					text = Service.Data.Appendixes[customIndex].Name;
				}
				else
				{
					customIndex = i;
					flag = RulesFlag;
					text = Service.Data.Rules[customIndex].Name;
				}

				CreateChapterMenuItem(20, flag, text ?? String.Empty, customIndex, 1000);

				if (flag == AppendixesFlag)
					for (int j = 0; j < Service.Data.Appendixes[customIndex].Items.Count; j++)
						CreateChapterMenuItem(16, flag, Service.Data.Appendixes[customIndex].Items[j].Name ?? String.Empty, customIndex, j);
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
			System.Windows.Application.Current.Shutdown();
		}
		private void HideButton_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized;
		}

		private void Drag(object sender, RoutedEventArgs e)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
				this.DragMove();
		}
	}
}
