using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using SQLite.Net;
using NuMo.iOS;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(SQLiteService))]
namespace NuMo.iOS
{
    class SQLiteService : ISQLite
    {
        //If it's the first time, make a local copy of the database and return a connection for that file path.
        //after the first time, making the copy may be skipped.
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "numoDatabase.db";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, sqliteFilename);

			try
			{
                //---copy only if file does not exist---
				if (!File.Exists(path))
				{
					File.Copy(sqliteFilename, path);

				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

	
            var plat = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
            var conn = new SQLite.Net.SQLiteConnection(plat, path);
		
            return conn;
        }
    }


}