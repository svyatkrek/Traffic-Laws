using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using Traffic_Laws.Entity;

namespace Traffic_Laws.src
{
    class FactService
    {
		private const string FilePath = "/contents/facts.json";
		public IFacts? Data;

		public FactService()
		{
			DataInitialization();
		}

		public IFact GetFact()
		{
			return Data.Facts[GetRandomNumber(0, Data.Facts.Count)];
		}

		private static int GetRandomNumber(int first, int last)
		{
			var rand = new Random();
			return rand.Next(first, last + 1);
		}
		private void DataInitialization()
		{
			string path = FilePath;
			Uri uri = new(path, UriKind.Relative);
			var resourceStream = (Application.GetResourceStream(uri)?.Stream) ?? throw new FileNotFoundException("Resource not found", path);
			using StreamReader reader = new(resourceStream);
			string tmp = reader.ReadToEnd();
			Data = JsonSerializer.Deserialize<IFacts>(tmp);
		}
	}
}
