using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Laws.Entity
{
	public class IQuestionState
	{
		public bool IsAnswer;
		public bool IsAnswerCorrect;


		public IQuestionState()
		{
			IsAnswer = false;
			IsAnswerCorrect = false;
		}

	}
}
