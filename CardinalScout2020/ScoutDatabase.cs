using Android.Text;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CardinalScout2020
{
    /// <summary>
    /// This class contains the SQLite database used by the app to store data. Each type of data has its own "table" in the databse. In each table, each instance of a type has its properties stored in "columns." Each type of data has a "PrimaryKey" which is how it can be put in/taken out of the database.
    /// </summary>
    public static class ScoutDatabase
    {
        private static SQLiteConnection _connection;

        /// <summary>
        /// Initialize the database and get a connection when the app starts.
        /// </summary>
        public static void Initialize()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();

            //Create tables for types of data that will be in the database.
            _connection.CreateTable<MatchData>();
            _connection.CreateTable<Event>();
            _connection.CreateTable<CompiledEventData>();        
        }      

        /*Table for Events*/

        /// <summary>
        /// Get an Event based on its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Event with the given ID.</returns>
        public static Event GetEvent(int id)
        {
            return _connection.Table<Event>().FirstOrDefault(t => t.EventID == id);
        }

        /// <summary>
        /// Get an Event based on its index in the database.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Event at the given index.</returns>
        public static Event GetEventFromIndex(int index)
        {
            return GetEvent(GetEventIDList()[index]);
        }

        /// <summary>
        /// Delete an Event based on its ID.
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteEvent(int id)
        {
            foreach (MatchData s in _connection.Table<MatchData>())
            {
                if (id == s.EventID)
                {
                    _connection.Delete<MatchData>(s.ID);
                }
            }
            _connection.Delete<Event>(id);
        }

        /// <summary>
        /// Add an Event to the database.
        /// </summary>
        /// <param name="putEvent"></param>
        public static void AddEvent(Event putEvent)
        {
            _connection.Insert(putEvent);
        }

        /// <summary>
        /// A formatted list of Events. Used in lists/dropdowns.
        /// </summary>
        /// <returns>A formatted list of Events and their properties.</returns>
        public static List<SpannableString> GetEventDisplayList()
        {
            SpannableString[] result;
            List<SpannableString> final = new List<SpannableString>();
            foreach (Event e in _connection.Table<Event>())
            {
                result = new SpannableString[]
                {
                    FormatString.SetBold("'"+e.EventName+"'"),
                    FormatString.SetBold(" | "),
                    FormatString.SetNormal("("+e.StartDate+" - "+e.EndDate+")"),
                    FormatString.SetBold(" | "),
                    FormatString.SetBold("ID: " + e.EventID),
                };

                final.Add(new SpannableString(TextUtils.ConcatFormatted(result)));
            }

            //Reverse so that newest Event is shown first.
            final.Reverse();
            return final;
        }

        /// <summary>
        /// Get list of EventIDs in the database.
        /// </summary>
        /// <returns>List of EventIDs.</returns>
        public static List<int> GetEventIDList()
        {
            List<int> ids = new List<int>();
            foreach (Event e in _connection.Table<Event>())
            {
                ids.Add(e.EventID);
            }

            //Reverse to that this list matches any display lists.
            ids.Reverse();
            return ids;
        }

        /// <summary>
        /// Gets the Event currently being used by an activity.
        /// </summary>
        /// <returns>Currently active Event.</returns>
        public static Event GetCurrentEvent()
        {
            foreach (Event e in _connection.Table<Event>())
            {
                if (e.IsCurrentEvent)
                {
                    return e;
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the current Event being used by the activity.
        /// </summary>
        /// <param name="id"></param>
        public static void SetCurrentEvent(int id)
        {
            Event placeholder;
            foreach (Event e in _connection.Table<Event>())
            {
                placeholder = e;
                //Set all other events to not current.
                if(e.IsCurrentEvent)
                {
                    placeholder.IsCurrentEvent = false;
                    _connection.Delete<Event>(e.EventID);
                    _connection.Insert(placeholder);
                }         

                //Set the Event with the given ID to the current one.
                if (e.EventID == id)
                {
                    placeholder.IsCurrentEvent = true;
                    _connection.Delete<Event>(id);
                    _connection.Insert(placeholder);
                }
            }
        }
        
        /// <summary>
        /// Changes an Event's ID. Also changes the ID's of matches associated with the Event.
        /// </summary>
        /// <param name="oldID"></param>
        /// <param name="newID"></param>
        public static void ChangeEventID(int oldID, int newID)
        {           
            foreach (Event e in _connection.Table<Event>())
            {
                Event placeholder = e;
                if (e.EventID == oldID)
                {
                    placeholder.EventID = newID;
                    _connection.Insert(placeholder);
                    _connection.Delete<Event>(oldID);
                }
            }

            //Change ID for matches associated with the event.
            foreach (MatchData s in _connection.Table<MatchData>())
            {
                MatchData placeholder = s;
                if (s.EventID == oldID)
                {
                    //MatchData IDs are strings (eventID,matchNumber).
                    placeholder.ID = newID.ToString() + "," + s.ID.Substring(s.ID.IndexOf(",") + 1);
                    s.EventID = newID;
                    _connection.Insert(placeholder);
                    _connection.Delete<MatchData>(s.ID);
                }
            }
        }

        /*Database for MatchData*/

        /// <summary>
        /// Get a MatchData based on its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MatchData with given ID.</returns>
        public static MatchData GetMatchData(string id)
        {
            return _connection.Table<MatchData>().FirstOrDefault(t => t.ID == id);
        }

        /// <summary>
        /// Get a MatchData based on its position within an Event in the database.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="index"></param>
        /// <returns>MatchData based on its index in the array.</returns>
        public static MatchData GetMatchDataFromIndex(Event e, int index)
        {
            string key = GetMatchIDList()[index];
            return GetMatchData(key);          
        }

        /// <summary>
        /// Get list of MatchData IDs in the database.
        /// </summary>
        /// <returns>List of MatchData in the database.</returns>
        public static List<string> GetMatchIDList()
        {
            List<string> ids = new List<string>();
            foreach (MatchData m in _connection.Table<MatchData>())
            {
                ids.Add(m.ID);
            }

            //Reverse to that this list matches any display lists.
            ids.Reverse();
            return ids;
        }

        /// <summary>
        /// Delete a MatchData based on its ID.
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteMatchData(string id)
        {
            _connection.Delete<MatchData>(id);
        }

        /// <summary>
        /// Add a MatchData to the database.
        /// </summary>
        /// <param name="putMatchData"></param>
        public static void AddMatchData(MatchData putMatchData)
        {
            _connection.Insert(putMatchData);
        }

        /// <summary>
        /// Get the current MatchData being used by an activity.
        /// </summary>
        /// <returns>Currently active MatchData.</returns>
        public static MatchData GetCurrentMatch()
        {
            foreach (MatchData s in _connection.Table<MatchData>())
            {
                if (s.IsCurrentMatch)
                {
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// Set the MatchData currently being used.
        /// </summary>
        /// <param name="id"></param>
        public static void SetCurrentMatch(string id)
        {
            foreach (MatchData s in _connection.Table<MatchData>())
            {
                MatchData placeholder = s;                
                if(s.IsCurrentMatch)
                {
                    placeholder.IsCurrentMatch = false;
                    _connection.Delete<MatchData>(s.ID);
                    _connection.Insert(placeholder);
                }                
                if (s.ID == id)
                {
                    placeholder.IsCurrentMatch = true;
                    _connection.Delete<MatchData>(id);
                    _connection.Insert(placeholder);
                }
            }
        }        

        /// <summary>
        /// Get the matches associated with an Event.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of MatchData for an Event.</returns>
        public static List<MatchData> GetMatchDataForEvent(int id)
        {
            List<MatchData> result = new List<MatchData>();
            foreach (MatchData s in _connection.Table<MatchData>())
            {
                if (s.EventID == id)
                {
                    result.Add(s);
                }
            }

            //Reverse so that newest matches are first. 
            result.Reverse();
            return result;
        }

        /// <summary>
        /// Get a formatted list of matches.
        /// </summary>
        /// <param name="eid"></param>
        /// <returns>Formatted list of matches from newest to oldest.</returns>
        public static List<SpannableString> GetMatchDisplayList(int eid)
        {
            List<SpannableString> result = new List<SpannableString>();
            SpannableString[] disp;
            foreach (MatchData s in _connection.Table<MatchData>())
            {
                if (s.EventID == eid)
                {
                    disp = new SpannableString[]
                    {
                        FormatString.SetNormal("Match "),
                        FormatString.SetBold(s.MatchNumber.ToString()),
                        FormatString.SetNormal(" (Team: "),
                        FormatString.SetBold(s.TeamNumber.ToString()),
                        FormatString.SetNormal(")"),
                    };
                    result.Add(new SpannableString(TextUtils.ConcatFormatted(disp)));
                }
            }

            //Put the newest match first.
            result.Reverse();
            return result;
        }

        /*Database for CompiledEventData*/

        /// <summary>
        /// Get CompiledEventData based on ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CompiledEventData with the given ID.</returns>
        public static CompiledEventData GetCompiledEventData(int id)
        {
            return _connection.Table<CompiledEventData>().FirstOrDefault(t => t.ID == id);
        }

        /// <summary>
        /// Get a list of all the CompiledEventData currently in the database.
        /// </summary>     
        /// <returns></returns>
        public static List<CompiledEventData> GetCompiledEventDataList()
        {
            List<CompiledEventData> result = new List<CompiledEventData>();

            foreach (CompiledEventData c in _connection.Table<CompiledEventData>())
            {
                result.Add(c);
            }

            return result;
        }

        /// <summary>
        /// Get CompiledEventData based on its position in the database.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>CompiledEventData at the given index.</returns>
        public static CompiledEventData GetCompiledEventDataFromIndex(int index)
        {
            return GetCompiledEventData(CompiledIDList()[index]);
        }
        /// <summary>
        /// Delete a CompiledEventData based on its ID.
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteCompiledEventData(int id)
        {
            _connection.Delete<CompiledEventData>(id);
        }

        /// <summary>
        /// Add a CompiledEventData to the database.
        /// </summary>
        /// <param name="putEventData"></param>
        public static void AddCompiledEventData(CompiledEventData putEventData)
        {
            _connection.Insert(putEventData);
        }
        
        /// <summary>
        /// Get a formatted list of CompiledEventData.
        /// </summary>
        /// <returns></returns>
        public static List<SpannableString> GetCompiledDisplayList()
        {
            SpannableString[] result;
            List<SpannableString> final = new List<SpannableString>();
            foreach (CompiledEventData e in _connection.Table<CompiledEventData>())
            {
                result = new SpannableString[]
                {
                    FormatString.SetBold("'"+e.OfficialName+"'"),
                    FormatString.SetBold(" | "),
                    FormatString.SetNormal("Date Modified: " + e.DateMod + " at "),
                    FormatString.SetBold(e.TimeMod),
                    FormatString.SetBold(" | "),
                    FormatString.SetBold("ID: " + e.ID),
                };
                final.Add(new SpannableString(TextUtils.ConcatFormatted(result)));
            }

            //Newest first.
            final.Reverse();
            return final;
        }        

        /// <summary>
        /// Get a list of all CompiledEventData IDs.
        /// </summary>
        /// <returns></returns>
        public static List<int> CompiledIDList()
        {
            List<int> ids = new List<int>();
            foreach (CompiledEventData e in _connection.Table<CompiledEventData>())
            {
                ids.Add(e.ID);
            }

            //Newst first.
            ids.Reverse();
            return ids;
        }

        /// <summary>
        /// Get the CompiledEventData currently being used by an Activity.
        /// </summary>
        /// <returns>Currently active CompiledEventData.</returns>
        public static CompiledEventData GetCurrentCompiled()
        {
            foreach (CompiledEventData e in _connection.Table<CompiledEventData>())
            {
                if (e.IsCurrentCompiled)
                {
                    return e;
                }
            }

            return null;
        }        

        /// <summary>
        /// Set the CompiledEventData currently being used by an activity.
        /// </summary>
        /// <param name="id"></param>
        public static void SetCurrentCompiled(int id)
        {
            foreach (CompiledEventData e in _connection.Table<CompiledEventData>())
            {
                CompiledEventData placeholder = e;

                if (e.IsCurrentCompiled)
                {
                    placeholder.IsCurrentCompiled = false;
                    _connection.Delete<CompiledEventData>(e.ID);
                    _connection.Insert(placeholder);
                }
                if (e.ID == id)
                {
                    placeholder.IsCurrentCompiled = true;
                    _connection.Delete<CompiledEventData>(id);
                    _connection.Insert(placeholder);
                }
            }
        }

        /// <summary>
        /// Set the team which is currently active within a CompiledEventData.
        /// </summary>       
        /// <param name="team"></param>
        public static void SetCurrentCompiledTeam(int team)
        {
            CompiledEventData placeholder = GetCurrentCompiled();
            placeholder.CurrentTeam = team;

            _connection.Delete<CompiledEventData>(placeholder.ID);
            _connection.Insert(placeholder);
        }

        /// <summary>
        /// Get the team active within the active CompiledEventData.
        /// </summary>
        /// <returns>Team number in active CompiledEventData.</returns>
        public static int GetCurrentCompiledTeam()
        {
            return GetCurrentCompiled().CurrentTeam;
        }
    }
}