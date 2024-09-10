using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Traffic_Laws.Entity;

namespace Traffic_Laws
{
    /// <summary>
    /// Логика взаимодействия для ExWindow.xaml
    /// </summary>
    public partial class ExWindow : Window
    {
		private readonly Service service = new();
		private int type;
        private int QuestionNumber;
        private DispatcherTimer timer;
		private int time = 0;
		private int AmountQuestion;

		public ExWindow(int _typeWindow, int _category)
        {
			InitializeComponent();
			type = _typeWindow;
			service.InitData(_typeWindow, _category);
			QuestionNumber = 0;
			DrawPagination();
			DrawQuestion();
			TimerStart();

		}

        private void TimerStart()
        {
			timer = new DispatcherTimer();
			timer.Tick += new EventHandler(TimerTick);
			timer.Interval = new TimeSpan(0, 0, 1);
			timer.Start();
		}

		private void TimerTick(object sender, EventArgs e)
		{
			time++;
			TimeSpan span = new(0, 0, time);
			labelTime.Content = span.ToString(@"mm\:ss");

		}


		private void DrawPagination()
		{
			AmountQuestion = service.data.Count;
			int wight = 650 / AmountQuestion;
			for (int i = 0; i < AmountQuestion; i++)
			{
				Button button = new()
				{
					Width = wight,
					Height = 40,
					HorizontalAlignment = HorizontalAlignment.Left,
					VerticalAlignment = VerticalAlignment.Top,
					Content = i + 1,
					Name = String.Concat("pagination_", i.ToString()),
					Background = new SolidColorBrush(Color.FromRgb(93, 86, 86)),
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
			paginationButtonOld.Background = new SolidColorBrush(Color.FromRgb(93, 86, 86));
			QuestionNumber = Convert.ToInt32(paginationButton.Name.Split('_')[1]);
			DrawQuestion();
		}

		private void ButtonAnswer_Click(object sender, RoutedEventArgs e)
		{
			if (!service.QuestionsState[QuestionNumber].IsAnswer)
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

					service.QuestionsState[QuestionNumber].IsAnswer = true;

					int answerNumber = Convert.ToInt32(selectedButton.Name.Split("_")[1]) - 1;
					string answerTextBlockName = selectedButton.Name.Replace("Check", "Text");
					TextBlock answerTextBlock = (TextBlock)this.FindName(answerTextBlockName);
					Button paginatinButton = (Button)PaginationGrid.Children[QuestionNumber];
					if (service.data[QuestionNumber].Answers[answerNumber].IsCorrect)
					{
						service.QuestionsState[QuestionNumber].IsAnswerCorrect = true;
						paginatinButton.Foreground = Brushes.Green;
						answerTextBlock.Foreground = Brushes.Green;
					}
					else
					{
						paginatinButton.Foreground = Brushes.Red;
						answerTextBlock.Foreground = Brushes.Red;

						for (int i = 0; i < service.data[QuestionNumber].Answers.Count; ++i)
						{
							if (service.data[QuestionNumber].Answers[i].IsCorrect)
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
				paginationButtonOld.Background = new SolidColorBrush(Color.FromRgb(93, 86, 86));
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
				if (service.QuestionsState[i].IsAnswer)
				{
					if (service.QuestionsState[i].IsAnswerCorrect)
						++amountCorrect;
					else
						++amountUncorrect;
				}
			}
			FinalWindow window = new(type, AmountQuestion, amountUncorrect, amountCorrect, time);
			window.Show();
			this.Hide();
		}
		private void DrawQuestion()
        {
			CheckedAndButtonClear();
			questionLabel.Text = service.data[QuestionNumber].QuestionText;
			string imageURL = service.data[QuestionNumber].ImageURL.Replace("./", "");
			if (imageURL.Contains("no_image"))
				ImageQuestion.Visibility = Visibility.Collapsed;
			else
			{
				ImageQuestion.Visibility = Visibility.Visible;
				ImageQuestion.Source = new BitmapImage(new Uri(imageURL, UriKind.Relative));
			}
			List<Answer> answers = service.data[QuestionNumber].Answers;

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
			paginatinButton.Background = new SolidColorBrush(Color.FromRgb(47, 45, 45));

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
	}
}
