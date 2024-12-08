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

namespace Traffic_Laws.Windows
{
    /// <summary>
    /// Логика взаимодействия для FactWindow.xaml
    /// </summary>
    public partial class FactWindow : Window
    {
		private FactService Service = new();
		public FactWindow()
        {
            InitializeComponent();
			FillInFact();

		}

		private void FillInFact()
		{
			IFact item = Service.GetFact();
			ImageFact.Source = new BitmapImage(new Uri(item.ImageURL ?? String.Empty));
			TextBlockFact.Text = item.Text;
		}

		private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
		{
			FillInFact();
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
