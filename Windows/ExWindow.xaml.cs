using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Traffic_Laws.Entity;
using Traffic_Laws.src;

namespace Traffic_Laws
{
    /// <summary>
    /// Логика взаимодействия для ExWindow.xaml
    /// </summary>
    public partial class ExWindow : Window
    {
		private readonly Service ServiceQuestion = new();
		private readonly DispatcherTimer Timer = new();
		private readonly string Type;
		private int Time = 0;
		private int QuestionNumber;
		private int AmountQuestion;

		public ExWindow(string _typeWindow, string _category, string selectedFile)
        {
			InitializeComponent();
			Type = _typeWindow;
			if (selectedFile == "")
				ServiceQuestion.InitData(Type, _category, true);
			else
				ServiceQuestion.InitData(Type, _category, false, selectedFile);
			TimerStart();
			QuestionNumber = 0;
			DrawPagination();
			DrawQuestion();

		}

        private void TimerStart()
        {
			Timer.Tick += new EventHandler(TimerTick);
			Timer.Interval = new TimeSpan(0, 0, 1);
			Timer.Start();
		}

		private void TimerTick(object? sender, EventArgs e)
		{
			Time++;
			TimeSpan span = new(0, 0, Time);
			labelTime.Content = span.ToString(@"mm\:ss");

		}


		private void DrawPagination()
		{
			AmountQuestion = ServiceQuestion.Data.Count;
			for (int i = 0; i < AmountQuestion; i++)
			{
				int margin = 20;
				if (i == 0 || i == AmountQuestion)
					margin = 0;
				Button button = new()
				{
					Width = 45,
					Height = 70,
					FontSize = 34,
					FontWeight = FontWeights.Bold,
					HorizontalAlignment = HorizontalAlignment.Left,
					VerticalAlignment = VerticalAlignment.Top,
					Content = i + 1,
					Margin =  new Thickness(margin, 0,0,0),
					Name = String.Concat("pagination_", i.ToString()),
					Background = new SolidColorBrush(Color.FromRgb(163, 184, 217)),
					Foreground = Brushes.White,
				};
				button.Click += new RoutedEventHandler(PaginationButton_Click);
				PaginationGrid.Children.Add(button);
			}
		}

		private void PaginationButton_Click(object sender, RoutedEventArgs e)
		{
			Button? paginationButton = e.Source as Button;
			Button? paginationButtonOld = (Button)PaginationGrid.Children[QuestionNumber];
			paginationButtonOld.Background = new SolidColorBrush(Color.FromRgb(163, 184, 217));
			QuestionNumber = Convert.ToInt32(paginationButton.Name.Split('_')[1]);
			DrawQuestion();
		}

		private void ButtonAnswer_Click(object sender, RoutedEventArgs e)
		{
			if (!ServiceQuestion.QuestionsState[QuestionNumber].IsAnswer)
			{
				Grid selectedGrid = stackPanelChecked.Children
					.OfType<Grid>()
					.FirstOrDefault(
						r => r.Children
						.OfType<RadioButton>()
						.First().IsChecked == true
					);
				if (selectedGrid != null) {
					RadioButton selectedButton = selectedGrid.Children
						.OfType<RadioButton>()
						.First();

					ServiceQuestion.QuestionsState[QuestionNumber].IsAnswer = true;

					int answerNumber = Convert.ToInt32(selectedButton.Name.Split("_")[1]) - 1;
					string answerTextBlockName = selectedButton.Name.Replace("Check", "Text");
					TextBlock answerTextBlock = (TextBlock)this.FindName(answerTextBlockName);
					Button paginatinButton = (Button)PaginationGrid.Children[QuestionNumber];
					if (ServiceQuestion.Data[QuestionNumber].Answers[answerNumber].IsCorrect)
					{
						ServiceQuestion.QuestionsState[QuestionNumber].IsAnswerCorrect = true;
						paginatinButton.Foreground = Brushes.Green;
						answerTextBlock.Foreground = Brushes.Green;
					}
					else
					{
						paginatinButton.Foreground = Brushes.Red;
						answerTextBlock.Foreground = Brushes.Red;

						for (int i = 0; i < ServiceQuestion.Data[QuestionNumber].Answers.Count; ++i)
						{
							if (ServiceQuestion.Data[QuestionNumber].Answers[i].IsCorrect)
							{
								string answerTextBlockCorrectName = "answerText_" + (i + 1).ToString();
								TextBlock textBlock = (TextBlock)this.FindName(answerTextBlockCorrectName);
								textBlock.Foreground = Brushes.Green;
							}
						}
					}

				}
			}
		}

		private void PaginationButtonMain_Click(object sender, RoutedEventArgs e)
		{
			if (QuestionNumber != AmountQuestion - 1)
			{
				Button? paginationButton = e.Source as Button;
				Button? paginationButtonOld = (Button)PaginationGrid.Children[QuestionNumber];
				paginationButtonOld.Background = new SolidColorBrush(Color.FromRgb(163, 184, 217));
				if (paginationButton.Name == "button_next")
					QuestionNumber += 1;
				else
					if (QuestionNumber != 0)
						QuestionNumber -= 1;

				DrawQuestion();
			}
			else
				ButtonResult_Click(sender, e);
		}

		private void ButtonResult_Click(object sender, RoutedEventArgs e)
		{
			int amountCorrect = 0;
			int amountUncorrect = 0;
			for (int i = 0; i < AmountQuestion; ++i)
			{
				if (ServiceQuestion.QuestionsState[i].IsAnswer)
				{
					if (ServiceQuestion.QuestionsState[i].IsAnswerCorrect)
						++amountCorrect;
					else
						++amountUncorrect;
				}
			}
			FinalWindow window = new(Type, AmountQuestion, amountUncorrect, amountCorrect, Time);
			window.Show();
			this.Hide();
		}
		private void DrawQuestion()
        {
			CheckedAndButtonClear();
			questionLabel.Text = ServiceQuestion.Data[QuestionNumber].QuestionText;
			string imageURL = "/" + ServiceQuestion.Data[QuestionNumber].ImageURL.Replace("./", "");
			if (imageURL.Contains("no_image"))
			{
				imagePanel.Visibility = Visibility.Collapsed;
				stackPanelChecked.Width = 900;
			}

			else
			{
				imagePanel.Visibility = Visibility.Visible;
				ImageQuestion.Source = new BitmapImage(new Uri(imageURL, UriKind.Relative));
				stackPanelChecked.Width = 480;
			}
			List<IAnswer> answers = ServiceQuestion.Data[QuestionNumber].Answers;

            for (int i = 0; i < answers.Count; i++)
            {
                string tbName = String.Concat("answerText_", (i+1).ToString());
                string chbName = String.Concat("answerCheck_", (i+1).ToString());
				RadioButton radioButton = (RadioButton)this.FindName(chbName);
				TextBlock textBlock = (TextBlock)this.FindName(tbName);
                textBlock.Text = answers[i].AnswerText;
				textBlock.Visibility = Visibility.Visible;
                radioButton.Visibility = Visibility.Visible;
			}
			Button paginatinButton = (Button)PaginationGrid.Children[QuestionNumber];
			paginatinButton.Background = new SolidColorBrush(Color.FromRgb(84, 137, 217));

			if (QuestionNumber == AmountQuestion - 1)
				button_next.Content = "Закончить";
			else
				button_next.Content = "Далее";
			
		}

		private void CheckedAndButtonClear()
		{
			answerCheck_1.IsChecked = true;
			answerCheck_1.IsChecked = false;
			answerCheck_3.Visibility = Visibility.Collapsed;
			answerText_3.Visibility = Visibility.Collapsed;
			answerCheck_4.Visibility = Visibility.Collapsed;
			answerText_4.Visibility = Visibility.Collapsed;
			answerText_1.Foreground = Brushes.Black;
			answerText_2.Foreground = Brushes.Black;
			answerText_3.Foreground = Brushes.Black;
			answerText_4.Foreground = Brushes.Black;

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

		private void OffsetButtons_Click(object sender, RoutedEventArgs e)
		{
			Button? button = e.Source as Button;
			if (button.Name.Split("_")[1] == "right")
				PaginationScroll.ScrollToHorizontalOffset(PaginationScroll.HorizontalOffset + 195);
			else
				PaginationScroll.ScrollToHorizontalOffset(PaginationScroll.HorizontalOffset - 195);
		}
	}
}
