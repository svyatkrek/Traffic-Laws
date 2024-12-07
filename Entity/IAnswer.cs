using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Traffic_Laws.Entity
{
	public class IAnswer
	{
		[JsonPropertyName("answer_text")]
		public string? AnswerText { get; set; }

		[JsonPropertyName("is_correct")]
		public bool IsCorrect {  get; set; }
	}
}
