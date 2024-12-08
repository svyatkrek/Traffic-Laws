using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Traffic_Laws.Entity
{

	public class IFact
	{
		[JsonPropertyName("content")]
		public string? Text { get; set; }

		[JsonPropertyName("url")]
		public string? ImageURL { get; set; }
	}

	public class IFacts
    {
		[JsonPropertyName("facts")]
		public List<IFact>? Facts { get; set; }
	}
}
