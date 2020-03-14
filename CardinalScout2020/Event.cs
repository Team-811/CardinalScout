using SQLite;

namespace CardinalScout2020
{
    /// <summary>
    /// This class stores an event and its properties.
    /// </summary>
    public class Event
    {        
        /// <summary>
        /// Events are identified by their Event ID in the SQL Database. 
        /// </summary>
        [PrimaryKey]
        public int EventID { get; set; }

        /// <summary>
        /// Default constructor for the database.
        /// </summary>
        public Event()
        {
        }

        /// <summary>
        /// Creates a new event with a start date, end date, name, and ID.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <param name="isCurrent"></param>
        public Event(string start, string end, string name, int ID, bool isCurrent)
        {
            StartDate = start;
            EndDate = end;
            EventName = name;
            EventID = ID;
            IsCurrentEvent = isCurrent;
        }

        /// <summary>
        /// Start date of a match.
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// End date of a match.
        /// </summary>
        public string EndDate { get; set; }       

        /// <summary>
        /// Name of the event.
        /// </summary>
        public string EventName { get; set; }
        
        /// <summary>
        /// Whether or not the event is currently selected or being edited by an activity in the app.
        /// </summary>
        public bool IsCurrentEvent { get; set; }
        
    }
}