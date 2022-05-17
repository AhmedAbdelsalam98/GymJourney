using SQLite;

namespace GymJourney.Models
{
    //Class models for storing SQL table for checkbox goals
    //By Ahmed Abdelsalam
    public class Checkbox
    {
        [PrimaryKey, AutoIncrement]
        public int Id { set; get; }
        public int Session_Id { set; get; }
        public string Name { set; get; }
        public bool IsChecked { set; get; }
    }
}
