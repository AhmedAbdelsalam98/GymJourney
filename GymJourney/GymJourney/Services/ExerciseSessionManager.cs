using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GymJourney.Persistence;
using SQLite;
using Xamarin.Forms;
using GymJourney.Models;

namespace GymJourney.Services
{
    //Class service for managing and retrieving journal session database
    //By Ahmed Abdelsalam
    public class ExerciseSessionManager
    {
        //Calls DependencyService to get connection based on platform implementation for ISQLite
        private SQLiteAsyncConnection _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        //bool check for tables existing
        private static bool _isInitialized = false;

        //checks if tables have been initialized atleast once this run
        private async Task initializeTablesCheck()
        {
            if (!_isInitialized)
            {
                await _connection.CreateTableAsync<ExerciseSession>();
                await _connection.CreateTableAsync<Checkbox>();
                _isInitialized = true;
            }
        }

        //creates new session journal, accepts date & time for title and optional workout notes
        //inserts into database
        public async Task AddSession(DateTime dateTime, string preWorkOutNotes = "", string postWorkOutNotes = "")
        {
            await initializeTablesCheck();
            var _es = new ExerciseSession
            {
                PreWorkOutNotes = preWorkOutNotes,
                PostWorkOutNotes = postWorkOutNotes,
                Title = $"{dateTime.DayOfWeek}, {dateTime.ToLongDateString()}"
            };
            await _connection.InsertAsync(_es);
        }

        //This method takes session_id and returns matching session journal from database
        public async Task<ExerciseSession> GetSession(int session_id)
        {
            await initializeTablesCheck();
            var _es = await _connection.FindAsync<ExerciseSession>((s) => s.Id == session_id);
            return _es;

        }

        //This method takes session journal object and deletes from database based on matching primary key
        //Also deletes dependent rows from Goals database based on session id foreign key
        public async Task DeleteSession(ExerciseSession es)
        {
            await initializeTablesCheck();
            var goals = await GetGoals(es.Id);
            foreach (var g in goals)
            {
                await DeleteGoal(g);
            }
            await _connection.DeleteAsync(es);
        }

        //This method takes session journal object and updates database according to matching PK
        public async Task UpdateSession(ExerciseSession es)
        {
            await initializeTablesCheck();
            await _connection.UpdateAsync(es);
        }

        //This method retrieves list of session journals
        public async Task<IEnumerable<ExerciseSession>> GetSessions()
        {
            await initializeTablesCheck();
            var _sessions = await _connection.Table<ExerciseSession>().ToListAsync();
            return _sessions;
        }

        //This method drops all tables
        public async Task DeleteSessions()
        {
            await initializeTablesCheck();
            await _connection.DropTableAsync<ExerciseSession>();
            await _connection.DropTableAsync<Checkbox>();
            Console.WriteLine("Sessions Deleted");
        }

        //This method adds goals to session journal to database by passing in Id and goal name
        public async Task AddGoal(int session_id, string name = "")
        {
            await initializeTablesCheck();
            var _goal = new Checkbox
            {
                Session_Id = session_id,
                Name = name,
                IsChecked = false
            };
            await _connection.InsertAsync(_goal);
        }

        //This method retrieves goals from database for session journal based on its Id
        public async Task<IEnumerable<Checkbox>> GetGoals(int session_id)
        {
            await initializeTablesCheck();
            var _es = await _connection.Table<Checkbox>().ToListAsync();
            return _es.FindAll((s) => s.Session_Id == session_id);
        }


        //This method iterates through a list of goals and updates them in database
        public async Task UpdateGoals(IEnumerable<Checkbox> goals)
        {
            await initializeTablesCheck();
            foreach (var g in goals)
            {
                await _connection.UpdateAsync(g);
            }
        }

        //this method takes checkbox goal object and deletes it from database
        public async Task DeleteGoal(Checkbox g)
        {
            await initializeTablesCheck();
            await _connection.DeleteAsync(g);
        }



    }
}
