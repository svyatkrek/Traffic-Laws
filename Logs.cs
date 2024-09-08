using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Laws
{
	public class Logs
	{
		private readonly string PathToLogs = Path.GetTempPath();
		private const string FileName = "pddLogs.txt";
		public string AmountAttempts;
		public string AmountCorrect;
		public string AmountUncorrect;
		public string BestTime;

		public void Save(int type, string answerCorrect, string answerUncorrect, string time)
		{

			string pathToFile = GetPathByType(type);


			if (!File.Exists(pathToFile) )
				File.Create(String.Concat(PathToLogs, FileName));

			string text = String.Concat(answerCorrect,":" ,answerUncorrect, ":", time, ";");

			File.AppendAllText(pathToFile, text);
		}

		public void Load(int type)
		{
			string pathToFile = GetPathByType(type);
			string[] data = File.ReadAllText(pathToFile).Split(';');
			AmountAttempts = data.Length.ToString();
			int amountCorrect = 0;
			int amountUncorrect = 0;
			int time = Int32.MaxValue;
			for (int i = 0; i < data.Length; ++i)
			{
				string[] tmp = data[i].Split(":");
				amountCorrect += Convert.ToInt32(tmp[0]);
				amountUncorrect += Convert.ToInt32(tmp[1]);

				if (Convert.ToInt32(tmp[2]) < time)
					time = Convert.ToInt32(tmp[2]);
			}
			AmountCorrect = amountCorrect.ToString();
			AmountUncorrect = amountUncorrect.ToString();
			BestTime = String.Concat((time / 60).ToString(), " минут ", (time - (time / 60 * 60)).ToString(), " секунд");
		}

		public string GetPathByType(int type)
		{
			if (type == 1)
				return Path.Combine(PathToLogs, "exam-", FileName);
			else
				return Path.Combine(PathToLogs, "test-", FileName);
		}
	}
}
