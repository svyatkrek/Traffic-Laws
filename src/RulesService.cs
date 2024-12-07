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
    class RulesService
    {
        private const string FilePath = "/contents/rules.json";
		public IRules? Data;
        public RulesService() 
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
			Data = JsonSerializer.Deserialize<IRules>(tmp);
		}
	}
}
