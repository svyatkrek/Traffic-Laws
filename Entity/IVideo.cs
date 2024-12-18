using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Traffic_Laws.Entity
{
    public class IVideo
    {
		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("imageUrl")]
		public string? ImageURL { get; set; }

		[JsonPropertyName("videoUrl")]
		public string? VideoURL { get; set; }

	}

	public class IVideos
	{
		[JsonPropertyName("items")]
		public List<IVideo>? Videos { get; set; }
	}
}
