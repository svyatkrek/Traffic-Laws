﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
		private readonly string type;
        private int QuestionNumber;
        private DispatcherTimer? timer;
		private int time = 0;
		private int AmountQuestion;

		public ExWindow(string _typeWindow, string _category, string selectedFile)
        {
			InitializeComponent();
			type = _typeWindow;
			if (selectedFile == "")
				service.InitData(type, _category, true);
			else
				service.InitData(type, _category, false, selectedFile);
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
					Width = 45,
					Height = 70,
					FontSize = 34,
					FontWeight = FontWeights.Bold,
					HorizontalAlignment = HorizontalAlignment.Left,
					VerticalAlignment = VerticalAlignment.Top,
					Content = i + 1,
					Margin =  new Thickness(20,0,0,0),
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
	}
}
