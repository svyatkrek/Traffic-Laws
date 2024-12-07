using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Traffic_Laws.src
{
    public class Logs
    {
        private readonly string PathToLogs = Path.GetTempPath();
        private const string FileName = "pddLogs.txt";
        public string? AmountAttempts;
        public string? AmountCorrect;
        public string? AmountUncorrect;
        public string? BestTime;

        public void Save(string type, string answerCorrect, string answerUncorrect, string time)
        {

            string pathToFile = GetPath(type);


            if (!File.Exists(pathToFile))
            {
                var file = File.Create(pathToFile);
                file.Close();
            }

            string text = string.Concat(answerCorrect, ":", answerUncorrect, ":", time, ";");
            Thread.Sleep(250);
            File.AppendAllText(pathToFile, text);
        }

        public bool Load(string type)
        {
            string[] data;
            string pathToFile = GetPath(type);
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
            int time = int.MaxValue;
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
            if (time != int.MaxValue)
            {
                BestTime = string.Concat((time / 60).ToString(), " МИНУТ ", (time - time / 60 * 60).ToString(), " СЕКУНД");
            }
            else
            {
                BestTime = "НЕТ ЗАПИСИ";
            }
            return true;
        }

        public string GetPath(string type)
        {
            return Path.Combine(PathToLogs, string.Concat(type, "-", FileName));
        }
    }
}
