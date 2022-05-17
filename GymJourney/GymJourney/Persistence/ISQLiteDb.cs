using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
//=============================================
// Reference A3: externally sourced algorithm
// Purpose: Interface to set-up SQLiteConnection
// Date: 8/11/2020
// Source: Xamarin Forms: Build Native Mobile Apps with C# Course
// Author: Mosh Hamedani
// url: https://codewithmosh.com/courses/enrolled/223139 
//=============================================
namespace GymJourney.Persistence
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
//=============================================
// End reference A3
//=============================================
