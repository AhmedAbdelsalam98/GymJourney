using System;
using System.IO;
using SQLite;
using GymJourney.Persistence;

//=============================================
// Reference A3: externally sourced algorithm
// Purpose: Interface implmentation to set-up SQLiteConnection for Android
// Date: 8/11/2020
// Source: Xamarin Forms: Build Native Mobile Apps with C# Course
// Author: Mosh Hamedani
// url: https://codewithmosh.com/courses/enrolled/223139 
//=============================================

[assembly: Xamarin.Forms.Dependency(typeof(SQLiteDb))]

namespace GymJourney.Persistence
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentPath, "GymJourneySQLite.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
    //=============================================
    // End reference A3
    //=============================================
}