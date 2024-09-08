using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Traffic_Laws.Entity
{
	public class Question
	{
		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("image")]
		public string ImageURL { get; set; }

		[JsonPropertyName("question")]
		public string QuestionText {  get; set; }

		[JsonPropertyName("answer_tip")]
		public string Comments { get; set; }

		[JsonPropertyName("answers")]
		public List<Answer> Answers { get; set; }

	}
}
