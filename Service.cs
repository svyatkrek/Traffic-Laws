using Accessibility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.TextFormatting;
using Traffic_Laws.Entity;

namespace Traffic_Laws
{
	public class Service
	{
		public List<Question> data;
		public List<QuestionState> QuestionsState = new();
		private const string categoryFolderNameA = "A_B";
		private const string categoryFolderNameC = "C_D";
		private const string typeFolderNameExam = "tickets";
		private const string typeFolderNameTest = "topics";

		

		public void InitData(int type, int category)
		{
			if (type == 1)
			{
				if (category == 1)
					InitDataExamCategory(type, categoryFolderNameA);
				else
					InitDataExamCategory(type, categoryFolderNameC);
			}
			else
			{
				if (category == 1)
					InitDataTestCategory(type, categoryFolderNameA);
				else
					InitDataTestCategory(type, categoryFolderNameC);
			}

			for (int i = 0; i < data.Count; i++)
			{
				QuestionState tmp = new();
				QuestionsState.Add(tmp);
			}

		}

		private void InitDataExamCategory(int type, string categoryFolderName)
		{
			int ticketNumber = GetRandomNumber(1, 40);

			string tmp = GetStringData(ConstructToPath(type, categoryFolderName, ticketNumber));

			data = JsonSerializer.Deserialize<List<Question>>(tmp);

		}

		private void InitDataTestCategory(int type, string categoryFolderName)
		{
			int ticketNumber = GetRandomNumber(1, 26);
			string tmp = GetStringData(ConstructToPath(type, categoryFolderName, ticketNumber));

			data = JsonSerializer.Deserialize<List<Question>>(tmp);

			int count = data.Count;
			if (count > 10)
				for (int i = 0; i < count - 10; ++i)
				{
					int randomIndex = GetRandomNumber(0, count - 1 - i);
					data.RemoveAt(randomIndex);
				}
			Console.WriteLine(data.Count);
		}


		private static int GetRandomNumber(int first, int last)
		{
			var rand = new Random();

			return rand.Next(first, last + 1);
		}

		private static string GetStringData(string path)
		{
			Uri uri = new(path, UriKind.Relative);
			var resourceStream = (Application.GetResourceStream(uri)?.Stream) ?? throw new FileNotFoundException("Resource not found", path);
			using StreamReader reader = new(resourceStream);
			string tmp = reader.ReadToEnd();
			return tmp;

		}

		private static string ConstructToPath( int type, string categoryFolderName, int number)
		{
			string path = Path.Combine("Contents", "questions", categoryFolderName);
			if (type == 1)
			{
				string fileName = String.Concat("Билет ", number.ToString(), ".json");
				return Path.Combine(path, typeFolderNameExam, fileName);
			}
			else
			{
				string pathToFolder = Path.Combine(path, typeFolderNameTest);

				string fileName = GetAllFilesToPath(pathToFolder)[number - 1];
				return fileName;
			}
			
		}

		private static List<string> GetAllFilesToPath(string folderPath)
		{

			folderPath = folderPath.Replace("\\", "/");

			List<string> filesList = new();

			Assembly assembly = Assembly.GetExecutingAssembly();

			string resourceName = assembly.GetName().Name + ".g.resources";

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			{
				if (stream != null)
				{
					using ResourceReader reader = new(stream);
					foreach (DictionaryEntry entry in reader)
					{
						string resourceKey = (string)entry.Key;

						// Если ресурс находится в нужной папке
						if (resourceKey.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase))
						{
							filesList.Add(resourceKey);
						}
					}
				}
			}

			return filesList;

		}

	}
}
