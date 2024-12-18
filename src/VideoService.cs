using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Traffic_Laws.Entity;

namespace Traffic_Laws.src
{
    class VideoService
    {
		private const string FilePath = "/contents/video.json";
		public IVideos? Data;

		public VideoService()
		{
			DataInitialization();
		}

		private void DataInitialization()
		{
			string path = FilePath;
			Uri uri = new(path, UriKind.Relative);
			var resourceStream = (Application.GetResourceStream(uri)?.Stream) ?? throw new FileNotFoundException("Resource not found", path);
			using StreamReader reader = new(resourceStream);
			string tmp = reader.ReadToEnd();
			Data = JsonSerializer.Deserialize<IVideos>(tmp);
		}
	}
}
