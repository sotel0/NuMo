using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(NuMo.Droid.SqliteService))]
namespace NuMo.Droid
{

    class SqliteService : ISQLite
    {
        public SqliteService() { }

        //If it's the first time, make a local copy of the database and return a connection for that file path.
        //after the first time, making the copy may be skipped.
        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "NumoData";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            if (!File.Exists(path))
            {
                var asset = Android.App.Application.Context.Assets.Open("numoDatabase.db");
                var dest = File.Create(path);
                asset.CopyTo(dest);
            }
            var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var conn = new SQLite.Net.SQLiteConnection(plat, path);
            return conn;
        }
    }
}