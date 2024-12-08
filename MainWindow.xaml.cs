using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using Traffic_Laws.src;
using Traffic_Laws.Windows;

namespace Traffic_Laws
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		public string selectedCategory = string.Empty;
        private const string ticketsButtonName = "button_tickets";
		private const string topicsButtonName = "button_topics";
		private const string statisticsButtonName = "checkButtonStatistics";
		public MainWindow()
        {
            InitializeComponent();
			InitializeComboBox();
			LaunchingDynamicElements();

		}

		private async void LaunchingDynamicElements()
        {
			Image img = (Image)this.FindName("dynamicImage");
            int position = -100;
            int angle = 0;
			RotateTransform rotateTransform = new();
			while (true)
            {
                if (position < 20)
                    position += 5;
                angle += 5;
                if (angle > 360)
                    angle -= 360;
                
                img.Margin = new Thickness(position, 0, 0, 10);
                rotateTransform.Angle = angle;
                rotateTransform.CenterX = 50;
                rotateTransform.CenterY = 50;
                img.RenderTransform = rotateTransform;
				await Task.Run(() => Thread.Sleep(50));
			}
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

            if (button.Name == selectedCategory)
            {
				selectedCategory = string.Empty;
				button.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
			}
            else
            {
				Button oldActiveButton = (Button)this.FindName(selectedCategory);
                if (oldActiveButton != null)
                    oldActiveButton.Foreground = button.Foreground;
                

				button.Foreground = new SolidColorBrush(Color.FromRgb( 13, 180, 185));
				selectedCategory = button.Name;
			}
			InitializeComboBox();
		}
		private void InitializeComboBox()
		{
			ComboBox ticketsComboBox = (ComboBox)this.FindName(ticketsButtonName);
			ComboBox topicsComboBox = (ComboBox)this.FindName(topicsButtonName);
			ticketsComboBox.Items.Clear();
			topicsComboBox.Items.Clear();
			ticketsComboBox.IsEnabled = true;
			topicsComboBox.IsEnabled = true;
			TextBoxExam.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
			TextBoxTest.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
			if (selectedCategory != statisticsButtonName)
			{

				if (selectedCategory != String.Empty)
				{
					List<string> tickets_list = Service.GetFileNamesByParams(ticketsButtonName.Split('_')[1], selectedCategory);
					List<string> topics_list = Service.GetFileNamesByParams(topicsButtonName.Split('_')[1], selectedCategory);
					ticketsComboBox.Items.Add("Любой");
					topicsComboBox.Items.Add("Любой");
					foreach (string ticket in tickets_list)
					{
						string tmp = WebUtility.UrlDecode(ticket.Split('.')[0].Split("/")[^1]);
						ticketsComboBox.Items.Add(tmp);
					}
					foreach (string topic in topics_list)
					{
						string tmp = WebUtility.UrlDecode(topic.Split('.')[0].Split("/")[^1]);
						topicsComboBox.Items.Add(tmp);
					}
				}
				else
				{
					ticketsComboBox.IsEnabled = false;
					topicsComboBox.IsEnabled = false;
					TextBoxExam.Foreground = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0));
					TextBoxTest.Foreground = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0));
				}
			}
			else
			{
				ticketsComboBox.Items.Add("Посмотреть статистику экзаменов");
				topicsComboBox.Items.Add("Посмотреть статистику зачетов");
			}

		}

		private void ComboBoxStart_Click(object sender, SelectionChangedEventArgs e)
		{
			ComboBox? comboBox = e.Source as ComboBox;
			string type = comboBox.Name.Split("_")[1];
			string selectedFile = String.Empty;
			if (comboBox.SelectedIndex != 0) 
				selectedFile = comboBox.SelectedItem.ToString();

				
			if (selectedCategory != statisticsButtonName)
			{
				ExWindow exam = new(type, selectedCategory, selectedFile ?? String.Empty);
				exam.Show();
				this.Hide();
			}
			else
			{
				StatisticsWindow statisticsWindow = new(type);
				statisticsWindow.Show();
				this.Hide();
			}
		}
		private void FactButton_Click(object sender, RoutedEventArgs e)
		{
			FactWindow factWindow = new();
			factWindow.Show();
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
