using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic_Laws.Entity;
using SQLitePCL;

namespace Traffic_Laws.src
{
    public class Connector
    {

		private static string dbPath = Path.Combine(System.IO.Path.GetFullPath(@"..\..\..\"), "Storage.db");
		private static string ConnectionString = $"Data Source={dbPath};";
		public static void CreateFactsTable()
		{
			Batteries.Init();
			using var connection = new SqliteConnection(ConnectionString);
			
			connection.Open();
			string query = @"
                CREATE TABLE IF NOT EXISTS facts (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    content TEXT NOT NULL,
                    url TEXT NOT NULL
                );";
			using var command = new SqliteCommand(query, connection);
			command.ExecuteNonQuery();

			FactService tmp = new();
			IFacts facts = tmp.Data;

			foreach (var fact in facts.Facts)
			{
				string insertQuery = "INSERT INTO facts (content, url) VALUES (@content, @url);";
				using var cmd = new SqliteCommand(insertQuery, connection);
				cmd.Parameters.AddWithValue("@content", fact.Text ?? "");
				cmd.Parameters.AddWithValue("@url", fact.ImageURL ?? "");
				cmd.ExecuteNonQuery();
			}
			connection.Close();
		}

        public static void CreateVideoTable()
        {
            Batteries.Init();
            using var connection = new SqliteConnection(ConnectionString);

            connection.Open();
            string query = @"
                CREATE TABLE IF NOT EXISTS video (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    video_url TEXT NOT NULL,
                    image_url TEXT NOT NULL,
                    name TEXT NOT NULL
                );";
            using var command = new SqliteCommand(query, connection);
            command.ExecuteNonQuery();

            VideoService tmp = new();
            IVideos videos = tmp.Data;

            foreach (var video in videos.Videos)
            {
                string insertQuery = "INSERT INTO video (video_url, image_url, name) VALUES (@video_url, @image_url, @name);";
                using var cmd = new SqliteCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@video_url", video.VideoURL ?? "");
                cmd.Parameters.AddWithValue("@image_url", video.ImageURL ?? "");
                cmd.Parameters.AddWithValue("@name", video.Name ?? "");
                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}
