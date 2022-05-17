using SQLite;

namespace GymJourney.Models
{
    //Class model for storing SQL table for journal sessions
    //By Ahmed Abdelsalam
    public class ExerciseSession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string PreWorkOutNotes { set; get; }
        public string PostWorkOutNotes { set; get; }
        public string FilePath { set; get; }
        public string Title { set; get; }
    }
}
