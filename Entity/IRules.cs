using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Traffic_Laws.Entity
{

	public class IRule
	{
		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("title")]
		public string? Title { get; set; }

		[JsonPropertyName("content")]
		public string? Text { get; set; }
	}

	public class IAppendixContent
	{
		[JsonPropertyName("content")]
		public string? Text { get; set; }

		[JsonPropertyName("url")]
		public string? ImageURL { get; set; }
	}
	public class IAppendix
	{
		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("title")]
		public string? Title { get; set; }

		[JsonPropertyName("contents")]
		public List<IAppendixContent>? Contents { get; set; }
	}
	public class IAppendixes
	{
		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("items")]
		public List<IAppendix>? Items { get; set; }

	}

    public class IRules
	{
		[JsonPropertyName("rules")]
		public List<IRule>? Rules { get; set; }

		[JsonPropertyName("appendixes")]
		public List<IAppendixes>? Appendixes { get; set; }
		
	}
}
