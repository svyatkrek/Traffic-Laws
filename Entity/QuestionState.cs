using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Laws.Entity
{
	public class QuestionState
	{
		public bool IsAnswer;
		public bool IsAnswerCorrect;


		public QuestionState()
		{
			IsAnswer = false;
			IsAnswerCorrect = false;
		}

	}
}
