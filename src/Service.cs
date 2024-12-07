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
using System.Xml;
using Traffic_Laws.Entity;

namespace Traffic_Laws.src
{
    public class Service
    {
        public List<IQuestion>? Data;
        public List<IQuestionState> QuestionsState = new();
        private const string FilePathPattern = "Contents/questions/{0}/{1}";


        public void InitData(string type, string category, bool isRandom, string selectedName = "")
        {
            string folderPath = ConstructToPath(type, category);

            if (isRandom)
            {
                int randomNumber = GetRandomNumber(1, GetNumberOfFilesByPath(folderPath));
                folderPath = GetAllFilesToPath(folderPath)[randomNumber - 1];
            }
            else
            {
                folderPath = Path.Combine(folderPath, ConstructFileName(selectedName));
            }

            ProcessData(folderPath);

            for (int i = 0; i < Data.Count; i++)
            {
                IQuestionState tmp = new();
                QuestionsState.Add(tmp);
            }
        }
        public static List<string> GetFileNamesByParams(string type, string category)
        {
            return GetAllFilesToPath(string.Format(FilePathPattern, category, type)); ;
        }

        private void ProcessData(string folderPath)
        {
            string tmp = GetStringData(folderPath);

            Data = JsonSerializer.Deserialize<List<IQuestion>>(tmp);
        }

        private static string ConstructFileName(string name)
        {
            return name + ".json";
        }
        private static int GetNumberOfFilesByPath(string path)
        {
            return GetAllFilesToPath(path).Count;
        }
        private static string ConstructToPath(string type, string category)
        {
            return string.Format(FilePathPattern, category, type);
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
