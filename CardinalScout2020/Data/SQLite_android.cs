﻿using SQLite;
using System.IO;
using Xamarin.Forms;

//this creates a new connection to the database
//the file for the database is stored on the device at the path name given
//uses a dependency service for android

[assembly: Dependency(typeof(CardinalScout2020.Data.SQLite_android))]
namespace CardinalScout2020.Data
{
    public class SQLite_android:ISQLite
    {
        public SQLite_android() { }
        
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "scoutdb.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            var conn = new SQLiteConnection(path);

            return conn;
        }

        //get the path to the database
        public static string getDatabasePath()
        {
            var sqliteFilename = "scoutdb.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);

            return path;
        }
       
    }
}