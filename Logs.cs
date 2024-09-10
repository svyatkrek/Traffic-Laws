using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading;
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


			if (!File.Exists(pathToFile))
			{
				var file = File.Create(pathToFile);
				file.Close();
			}

			string text = String.Concat(answerCorrect,":" ,answerUncorrect, ":", time, ";");
			Thread.Sleep(250);	
			File.AppendAllText(pathToFile, text);
		}

		public bool Load(int type)
		{
			string[] data;
			string pathToFile = GetPathByType(type);
			try
			{
				data = File.ReadAllText(pathToFile).Split(';');
			}
			catch
			{
				return false;
			}
			AmountAttempts = data.Length.ToString();
			int amountCorrect = 0;
			int amountUncorrect = 0;
			int time = Int32.MaxValue;
			for (int i = 0; i < data.Length - 1; ++i)
			{
				string[] tmp = data[i].Split(':');
				amountCorrect += Convert.ToInt32(tmp[0]);
				amountUncorrect += Convert.ToInt32(tmp[1]);

				if (Convert.ToInt32(tmp[2]) < time)
					time = Convert.ToInt32(tmp[2]);
			}
			AmountCorrect = amountCorrect.ToString();
			AmountUncorrect = amountUncorrect.ToString();
			if (time != Int32.MaxValue)
			{
				BestTime = String.Concat((time / 60).ToString(), " минут ", (time - (time / 60 * 60)).ToString(), " секунд");
			}
            else
            {
				BestTime = "нет записи";
            }
            return true;
		}

		public string GetPathByType(int type)
		{
			if (type == 1)
				return Path.Combine(PathToLogs, String.Concat("exam-", FileName));
			else
				return Path.Combine(PathToLogs, String.Concat("test-", FileName));
		}
	}
}
