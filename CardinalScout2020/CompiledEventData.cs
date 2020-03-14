using Android.Text;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardinalScout2020
{
    /// <summary>
    /// The CompiledEventData class extends the MatchData class. It contains information for all scouted matches for an event from all devices. Compiled Event Data can have all the same properties as a normal MatchData, but it also can perform calculations and figure out percentages.
    /// </summary>
    public class CompiledEventData : MatchData
    {
        /// <summary>
        /// Primary key for the CompiledEventData. The same as the event ID that it is for.
        /// </summary>
        [PrimaryKey]
        public new int ID { get; set; }

        /// <summary>
        /// String data from the scanned QR codes.
        /// </summary>
        public string QRdata { get; set; }

        /// <summary>
        /// Default constructor for the database.
        /// </summary>
        public CompiledEventData()
        {
        }   

        /// <summary>
        /// Constructor used to create a new CompiledEventData.
        /// </summary>
        /// <param name="officialName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="qrdata"></param>        
        /// <param name="id"></param>
        public CompiledEventData(string officialName, string startDate, string endDate, string qrdata, int id)
        {
            //Make the compiled event data mimic the event details.
            OfficialName = officialName;
            StartDate = startDate;
            EndDate = endDate;

            //CompiledEventData specific data
            QRdata = qrdata;
            ID = id;
            IsCurrentCompiled = false;
            CurrentTeam = 0;

            //Note the date and time modified.
            DateMod = DateTime.Now.ToString("MM/dd/yyyy");
            TimeMod = DateTime.Now.ToShortTimeString();
        }

        /// <summary>
        /// Official name for the event and its compiled data.
        /// </summary>
        public string OfficialName { get; set; }        
        
        /// <summary>
        /// Whether or not a CompiledEventData is currently being used by an activity.
        /// </summary>
        public bool IsCurrentCompiled { get; set; }

        /// <summary>
        /// The team number of the current team within the CompiledEventData being used by an activity.
        /// </summary>
        public int CurrentTeam { get; set; }
        
        /// <summary>
        /// Date the CompiledEventData was generated.
        /// </summary>
        public string DateMod { get; set; }

        /// <summary>
        /// Time the CompiledEventData was generated.
        /// </summary>
        public string TimeMod { get; set; }

        /// <summary>
        /// Read QR data and return it in a multidimensional list of matches grouped by team number.
        /// </summary>
        /// <returns>CompiledEventData represented as a 2D list.</returns>
        public List<List<CompiledEventData>> CompileData()
        {
            //How many properties are we looking for:
            const int dataLength = 20;

            //Create a placeholder.
            CompiledEventData placeholder = new CompiledEventData();
            
            //List of CompiledEventData before grouping it.
            List<CompiledEventData> cList = new List<CompiledEventData>();
            
            //Set up the substring of raw data.
            string matchData = QRdata;
            int substringStart = 0;            

            //Process all of the QR data.
            while (matchData.Length > 1)
            {
                int startIndex = 0;

                //Give default values to the placeholder.
                placeholder = new CompiledEventData
                {
                    OfficialName = OfficialName,
                    ID = ID,
                    IsCurrentCompiled = false
                };

                //Get positions of all commas in the data.
                List<int> matchCommas = AllIndexesOf(matchData, ",");

                //Set values which appear at the beginning of the QR string separated by commas.
                placeholder.TeamNumber = int.Parse(matchData.Substring(0, matchCommas[0]));
                placeholder.MatchNumber = int.Parse(matchData.Substring(matchCommas[0] + 1, matchCommas[1] - matchCommas[0] - 1));

                //Create substring of data after the commas.
                matchData = matchData.Substring(matchCommas[1] + 1, dataLength);                

                //Interpret numerical values.
                placeholder.Result = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;
                placeholder.DSPosition = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;
                placeholder.IsTable = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.InitiationCrossed = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.AutoResult = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;
                placeholder.Shoot = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.ShootOuter = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.ShootInner = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.ShootLower = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;                
                placeholder.ShootTrench = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.ShootInitiation = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.ShootPort = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.Climb = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.AdjustClimb = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.Wheel = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.RotationControl = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.PositionControl = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.UnderTrench = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.GoodDrivers = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.WouldRecommend = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;

                //Add the CompiledEventData to the list.
                cList.Add(placeholder);

                //Move to the next team in the string.
                substringStart += dataLength + matchCommas[1] + 1;
                matchData = QRdata.Substring(substringStart);
            }

            //Sort the list.
            cList.Sort((x, y) => x.TeamNumber.CompareTo(y.TeamNumber));
            
            //Create a list of the teams contained in the compiled data.
            List<int> uniqueTeams = new List<int>();
            foreach (CompiledEventData data in cList)
            {
                if (!uniqueTeams.Contains(data.TeamNumber))
                {
                    uniqueTeams.Add(data.TeamNumber);
                }
            }

            //Create a multidimensional list to group that data.
            List<List<CompiledEventData>> groupedData = new List<List<CompiledEventData>>
            {
                new List<CompiledEventData>()
            };

            //Add all matches by the same team to the same first index in the list.
            int c = 0;
            foreach (CompiledEventData data in cList)
            {                
                if (data.TeamNumber == uniqueTeams[c])
                {
                    groupedData[c].Add(data);
                }
                //Go to the next index after all matches by that team are added.
                else
                {
                    c++;
                    groupedData.Add(new List<CompiledEventData>());
                    groupedData[c].Add(data);
                }
            }

            //Return this grouped data list.
            return groupedData;
        }

        /// <summary>
        /// Used to find where commas are in the QRdata.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="searchstring"></param>
        /// <returns>List of indexes where a string is found in another string.</returns>
        private List<int> AllIndexesOf(string str, string searchstring)
        {
            List<int> result = new List<int>();
            int minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                result.Add(minIndex);
                minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
            }

            return result;
        }

        /*--------------------------*/
        /*These methods return lists of data for all teams. There are list of the raw numbers and also formatted lists.
        /*--------------------------*/

        /// <summary>
        /// List of teams at the event.
        /// </summary>
        /// <returns>List of teams in the data.</returns>        
        public IEnumerable<int> GetTeamList()
        {
            List<List<CompiledEventData>> c = CompileData();            
            for (int i = 0; i < c.Count; i++)
            {
                yield return c[i][0].TeamNumber;
            }           
        }

        /// <summary>
        /// Bolded list of teams at the event.
        /// </summary>
        /// <returns>Bolded list in SpannableString form.</returns>        
        public List<SpannableString> GetTeamListFormatted()
        {
            List<SpannableString> result = new List<SpannableString>();

            foreach(int i in GetTeamList())
            {
                result.Add(FormatString.SetBold(i.ToString()));
            }

            return result;
        }

        /// <summary>
        /// List of the percentage of scouters who recommended the teams.
        /// </summary>
        /// <returns>List of percentages in integer form.</returns>
        public List<int> GetRecPercentList()
        {
            List<int> recsPerTeam = new List<int>();           

            foreach(int i in GetTeamList())
            {
                recsPerTeam.Add(GetRecPercentForTeam(i));                           
            }

            return recsPerTeam;
        }

        /// <summary>
        /// List of the percentage of scouters who recommended the teams. Formatted red/green/normal depending on percentage.
        /// </summary>
        /// <returns>Formatted list of percentages in SpannableString form.</returns>
        public List<SpannableString> GetRecPercentListFormatted()
        {
            List<SpannableString> recsPerTeam = new List<SpannableString>();            

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetRecPercentForTeamFormatted(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// A list of win/loss/tie records. Formatted depending on thresholds.
        /// </summary>
        /// <returns>Formatted string in form "W-L-T"</returns>
        public List<SpannableString> GetRecordList()
        {
            List<SpannableString> recsPerTeam = new List<SpannableString>();           

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetRecordForTeam(i));
            }

            return recsPerTeam;
        }       

        /// <summary>
        /// Returns a list of the percentage of the time teams were able to shoot.
        /// </summary>
        /// <returns>Integer list of percentages.</returns>
        public List<int> GetShootList()
        {
            List<int> recsPerTeam = new List<int>();            

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetShootPercentForTeam(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// Returns a list of the percentage of the time teams were able to shoot. Formatted for thresholds.
        /// </summary>
        /// <returns>Formatted list of percentages in SpannableString form.</returns>
        public List<SpannableString> GetShootListFormatted()
        {
            List<SpannableString> recsPerTeam = new List<SpannableString>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetShootPercentForTeamFormatted(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// Returns a list of the port which teams shoot in most often.
        /// </summary>
        /// <returns>String list.</returns>
        public List<string> GetPrimaryPortList()
        {
            List<string> recsPerTeam = new List<string>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetPrimaryPortForTeam(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// Returns a list of the port which teams shoot in most often. Formatted for thresholds.
        /// </summary>
        /// <returns>Formatted list in SpannableString form.</returns>
        public List<SpannableString> GetPrimaryPortListFormatted()
        {
            List<SpannableString> recsPerTeam = new List<SpannableString>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetPrimaryPortForTeamFormatted(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// Returns a list of the percent of the time teams could climb.
        /// </summary>
        /// <returns>Interger list of percentages.</returns>
        public List<int> GetClimbList()
        {
            List<int> recsPerTeam = new List<int>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetClimbForTeam(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// Returns a list of the percent of the time teams could climb. Formatted for thresholds.
        /// </summary>
        /// <returns>Formatted list of percentages in SpannableString form.</returns>
        public List<SpannableString> GetClimbListFormatted()
        {
            List<SpannableString> recsPerTeam = new List<SpannableString>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetClimbForTeamFormatted(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// Returns a list of whether teams could adjust their climbs.
        /// </summary>
        /// <returns>Boolean list.</returns>
        public List<bool> GetAdjustList()
        {
            List<bool> recsPerTeam = new List<bool>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetAdjustForTeam(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// Returns a list of whether teams could adjust their climbs. Formatted for thresholds.
        /// </summary>
        /// <returns>Formatted boolean list in SpannableString format.</returns>
        public List<SpannableString> GetAdjustListFormatted()
        {
            List<SpannableString> recsPerTeam = new List<SpannableString>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetAdjustForTeamFormatted(i));
            }

            return recsPerTeam;
        }
        
        /// <summary>
        /// Returns a list of whether or not a team can spin the color wheel.
        /// </summary>
        /// <returns>Boolean list.</returns>
        public List<bool> GetWheelList()
        {
            List<bool> recsPerTeam = new List<bool>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetWheelForTeam(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// Returns a list of whether or not a team can spin the color wheel. Formatted for tresholds.
        /// </summary>
        /// <returns>Formatted boolean list in SpannableString format.</returns>
        public List<SpannableString> GetWheelListFormatted()
        {
            List<SpannableString> recsPerTeam = new List<SpannableString>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetWheelForTeamFormatted(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// Returns a list of the percent of the time teams' drivers were good.
        /// </summary>
        /// <returns>Integer list of percentages.</returns>
        public List<int> GetDriversList()
        {
            List<int> recsPerTeam = new List<int>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetDriversForTeam(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// Returns a list of the percent of the time teams' drivers were good. Formatted for thresholds.
        /// </summary>
        /// <returns>Formatted list of percentages in SpannableString format.</returns>
        public List<SpannableString> GetDriversListFormatted()
        {
            List<SpannableString> recsPerTeam = new List<SpannableString>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetDriversForTeamFormatted(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// Returns a list of the percent of the time teams weren't functioning. Formatted for thresholds.
        /// </summary>
        /// <returns>Integer list of percentages.</returns>
        public List<int> GetTableList()
        {
            List<int> recsPerTeam = new List<int>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetTableForTeam(i));
            }

            return recsPerTeam;
        }

        /// <summary>
        /// Returns a list of the percent of the time teams weren't functioning. Formatted for thresholds.
        /// </summary>
        /// <returns>Formatted list of percentages in SpannableString format.</returns>
        public List<SpannableString> GetTableListFormatted()
        {
            List<SpannableString> recsPerTeam = new List<SpannableString>();

            foreach (int i in GetTeamList())
            {
                recsPerTeam.Add(GetTableForTeamFormatted(i));
            }

            return recsPerTeam;
        }

        /*--------------------------*/
        /*These methods return lists of data for individual teams. There are list of the raw numbers and also formatted lists.
        /*--------------------------*/

        /// <summary>
        /// Gets the CompiledEventData for just one team.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>List of CompiledEventData for one team.</returns>
        public List<CompiledEventData> GetCompiledForTeam(int team)
        {
            int index = -1;
            List<List<CompiledEventData>> c = CompileData();

            for (int i = 0; i < c.Count; i++)
            {
                if (c[i][0].TeamNumber == team)
                {
                    index = i;
                    break;
                }
            }   
            
            return c[index];
        }

        /// <summary>
        /// Returns the percent of the time a team was recommended.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Integer percentage.</returns>
        public int GetRecPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = GetCompiledForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(c[i].WouldRecommend);
            }

            double countYes = recs.Where(x => x.Equals(0)).Count();
            double countNo = recs.Where(x => x.Equals(1)).Count();
            double countMaybe = recs.Where(x => x.Equals(2)).Count();

            return (int)((countYes + (0.5 * countMaybe)) / (countYes + countMaybe + countNo) * 100);            
        }

        /// <summary>
        /// Returns a formatted percent of the time a team was recommended.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted percentage in SpannableString format.</returns>
        public SpannableString GetRecPercentForTeamFormatted(int team)
        {
            int percent = GetRecPercentForTeam(team);

            if (percent >= Constants.recommendThreshHigh)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appGreen);
            else if (percent <= Constants.recommendThreshLow)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appRed);
            else
                return FormatString.SetNormal(percent.ToString() + "%");
        }

        /// <summary>
        /// Returns a team's win/loss/tie record. Formatted depending on thresholds.
        /// </summary>
        /// /// <param name="team"></param>
        /// <returns>SpannableString in format "W-L-T"</returns>
        public SpannableString GetRecordForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = GetCompiledForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(c[i].Result);
            }

            double countWin = recs.Where(x => x.Equals(0)).Count();
            double countLoss = recs.Where(x => x.Equals(1)).Count();
            double countTie = recs.Where(x => x.Equals(2)).Count();

            int winPerc = (int)Math.Round(countWin / (countLoss + countTie + countWin));

            string record = countWin.ToString() + "-" + countLoss.ToString() + "-" + countTie.ToString();

            if (winPerc >= Constants.winThreshHigh)
               return FormatString.SetColorBold(record, Constants.appGreen);
            else if (winPerc <= Constants.winThreshLow)
                return FormatString.SetColorBold(record, Constants.appRed);
            else
                return FormatString.SetNormal(record.ToString());                       
        }

        /// <summary>
        /// Returns the percent of the time a team could shoot.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Integer percentage.</returns>
        public int GetShootPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = GetCompiledForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].Shoot));
            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();
            return (int)(countYes / (countYes + countNo) * 100);            
        }

        /// <summary>
        /// Returns a formatted percent of the time a team could shoot.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted percentage in SpannableString format.</returns>
        public SpannableString GetShootPercentForTeamFormatted(int team)
        {
            int percent = GetShootPercentForTeam(team);

            if (percent >= Constants.shootThreshHigh)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appGreen);
            else if (percent <= Constants.shootThreshLow)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appRed);
            else
                return FormatString.SetNormal(percent.ToString() + "%");
        }

        /// <summary>
        /// Returns which port a team typically shoots in.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>String of preferred port.</returns>
        public string GetPrimaryPortForTeam(int team)
        {
            List<CompiledEventData> c = GetCompiledForTeam(team);

            int outer = 0;
            int inner = 0;
            int lower = 0;

            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].ShootOuter)
                    outer++;
                if (c[i].ShootInner)
                    inner++;
                if (c[i].ShootLower)
                    lower++;
            }

            if (outer == 0 && inner == 0 && lower == 0)
                return "None";
            else if (inner > outer && inner > lower)
                return "Inner";
            else if (lower > inner && lower > outer)
                return "Lower";
            else
                return "Outer";
        }

        /// <summary>
        /// Returns which port a team typically shoots in. Formatted.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted string of the port.</returns>
        public SpannableString GetPrimaryPortForTeamFormatted(int team)
        {
            string port = GetPrimaryPortForTeam(team);

            if (!port.Equals("None"))
                return FormatString.SetColorBold(port, Constants.appGreen);
            else
                return FormatString.SetColorBold(port, Constants.appRed);
        }

        /// <summary>
        /// Returns from where a team typically shoots on the field.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>String of the preferred location.</returns>
        public string GetLocationForTeam(int team)
        {
            List<CompiledEventData> c = GetCompiledForTeam(team);

            int trench = 0;
            int line = 0;
            int port = 0;

            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].ShootTrench)
                    trench++;
                if (c[i].ShootInitiation)
                    line++;
                if (c[i].ShootPort)
                    port++;
            }

            if (port == 0 && line == 0 && trench == 0)
                return "None";
            else if (trench > line && trench > port)
                return "Trench";
            else if (port > line && port > trench)
                return "Port";
            else
                return "Initiation Line";
        }

        /// <summary>
        /// Returns from where a team typically shoots on the field. Formatted.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted string of team's preferred location.</returns>
        public SpannableString GetLocationForTeamFormatted(int team)
        {
            string location = GetLocationForTeam(team);

            if (!location.Equals("None"))
                return FormatString.SetColorBold(location, Constants.appGreen);
            else
                return FormatString.SetColorBold(location, Constants.appRed);
        }

       /// <summary>
       /// Returns the percent of the time a team climbed.
       /// </summary>
       /// <param name="team"></param>
       /// <returns>Integer percentage.</returns>
        public int GetClimbForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = GetCompiledForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].Climb));
            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();
            return (int)(countYes / (countYes + countNo) * 100);            
        }

        /// <summary>
        /// Returns the formatted percent of the time a team climbed.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted integer percentage in SpannableString format.</returns>
        public SpannableString GetClimbForTeamFormatted(int team)
        {
            int percent = GetClimbForTeam(team);

            if (percent >= Constants.climbThreshHigh)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appGreen);
            else if (percent <= Constants.climbThreshLow)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appRed);
            else
                return FormatString.SetNormal(percent.ToString() + "%");
        }

        /// <summary>
        /// Returns whether or not a team could adjust its climb at all.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>True if a team could adjust.</returns>
        public bool GetAdjustForTeam(int team)
        {
            int count = 0;
            List<CompiledEventData> c = GetCompiledForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].AdjustClimb)
                    count++;
            }

            if (count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Returns whether or not a team could adjust its climb at all. Formatted.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted boolean of whether a team could adjust.</returns>
        public SpannableString GetAdjustForTeamFormatted(int team)
        {
            bool adjust = GetAdjustForTeam(team);

            if (adjust)
                return FormatString.SetColorBold(adjust.ToString(), Constants.appGreen);
            else
                return FormatString.SetNormal(adjust.ToString());
            
        }

        /// <summary>
        /// Returns whether or not could spin the wheel at all.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>True if a team could spin the wheel</returns>
        public bool GetWheelForTeam(int team)
        {
            int count = 0;
            List<CompiledEventData> c = GetCompiledForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].Wheel)
                    count++;
            }

            if (count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Returns whether or not a team could spin the wheel at all. Formatted.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted boolean of whether or not a team could spin the wheel.</returns>
        public SpannableString GetWheelForTeamFormatted(int team)
        {
            bool wheel = GetWheelForTeam(team);

            if (wheel)
                return FormatString.SetColorBold(wheel.ToString(), Constants.appGreen);
            else
                return FormatString.SetColorBold(wheel.ToString(), Constants.appRed);

        }

        /// <summary>
        /// Returns whether or not the team did rotation control at all.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>True if team could do rotation control.</returns>
        public bool GetWheelRotationForTeam(int team)
        {
            int count = 0;
            List<CompiledEventData> c = GetCompiledForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].RotationControl)
                    count++;
            }

            if (count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Returns whether or not a team did rotation control at all. Formatted.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted boolean if team could do rotation control.</returns>
        public SpannableString GetWheelRotationForTeamFormatted(int team)
        {
            bool rotation = GetWheelRotationForTeam(team);

            if (rotation)
                return FormatString.SetColorBold(rotation.ToString(), Constants.appGreen);
            else
                return FormatString.SetColorBold(rotation.ToString(), Constants.appRed);
        }

        /// <summary>
        /// Returns whether or not the team did position control at all.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>True if a team could do position control.</returns>
        public bool GetWheelPositionForTeam(int team)
        {
            int count = 0;
            List<CompiledEventData> c = GetCompiledForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].PositionControl)
                    count++;
            }

            if (count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Returns whether or not a team did position control at all. Formatted.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted boolean of whether or not a team could do position control.</returns>
        public SpannableString GetWheelPositionForTeamFormatted(int team)
        {
            bool position = GetWheelPositionForTeam(team);

            if (position)
                return FormatString.SetColorBold(position.ToString(), Constants.appGreen);
            else
                return FormatString.SetColorBold(position.ToString(), Constants.appRed);
        }

        /// <summary>
        /// Returns percent of the time a team's drivers were good.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Integer percentage.</returns>
        public int GetDriversForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = GetCompiledForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].GoodDrivers));
            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();
            return (int)(countYes / (countYes + countNo) * 100);            
        }

        /// <summary>
        /// Returns formatted percent of the time a team's drivers were good.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted percentage in SpannableString format.</returns>
        public SpannableString GetDriversForTeamFormatted(int team)
        {
            int percent = GetDriversForTeam(team);

            if (percent >= Constants.climbThreshHigh)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appGreen);
            else if (percent <= Constants.climbThreshLow)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appRed);
            else
                return FormatString.SetNormal(percent.ToString() + "%");
        }

        /// <summary>
        /// Returns percent of the time a team was not functional.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Integer percentage.</returns>
        public int GetTableForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = GetCompiledForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].IsTable));
            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();
            return (int)(countYes / (countYes + countNo) * 100);            
        }

        /// <summary>
        /// Returns formatted percent of the time a team was not functional.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted percentage in SpannableString format.</returns>
        public SpannableString GetTableForTeamFormatted(int team)
        {
            int percent = GetTableForTeam(team);

            if (percent >= Constants.tableThresh)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appRed);            
            else
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appGreen);
        }

        /*Individual team autonomous calculations*/

        /// <summary>
        /// Returns the percent of the time a team was able to move off the initiation line.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Integer percentage.</returns>
        public int GetInitiationLineForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = GetCompiledForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].InitiationCrossed));
            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();
            return (int)(countYes / (countYes + countNo) * 100);            
        }

        /// <summary>
        /// Returns the formatted percent of the time a team was able to move off the initiation line.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted percentage in SpannableString format.</returns>
        public SpannableString GetInitiationLineForTeamFormatted(int team)
        {
            int percent = GetInitiationLineForTeam(team);

            if (percent >= Constants.initiationThreshHigh)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appGreen);
            else if (percent <= Constants.initiationThreshLow)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appRed);
            else
                return FormatString.SetNormal(percent.ToString() + "%");
        }

        /// <summary>
        /// Returns the percent of the time a team scored between 1 and 3 balls during auto.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Integer percentage.</returns>
        public int GetLowAutoForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = GetCompiledForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].AutoResult));
            }
            double countNone = recs.Where(x => x.Equals(0)).Count();
            double countLow = recs.Where(x => x.Equals(1)).Count();
            double countHigh = recs.Where(x => x.Equals(2)).Count();
            return (int)(countLow / (countHigh + countNone + countLow) * 100);            
        }

        /// <summary>
        /// Returns the formatted percent of the time a team scored between 1 and 3 balls during auto.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted percentage in SpannableString format.</returns>
        public SpannableString GetLowAutoForTeamFormatted(int team)
        {
            int percent = GetLowAutoForTeam(team);

            if (percent >= Constants.autoBallThresh)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appGreen);
            else if (percent <= Constants.autoBallThresh)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appRed);
            else
                return FormatString.SetNormal(percent.ToString() + "%");
        }

        /// <summary>
        /// Returns the percent of the time a team scored 4+ balls during auto.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Integer percentage.</returns>
        public int GetHighAutoForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = GetCompiledForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].AutoResult));
            }
            double countNone = recs.Where(x => x.Equals(0)).Count();
            double countLow = recs.Where(x => x.Equals(1)).Count();
            double countHigh = recs.Where(x => x.Equals(2)).Count();
            return (int)(countHigh / (countLow + countNone + countHigh) * 100);            
        }

        /// <summary>
        /// Returns the formatted percent of the time a team scored 4+ balls during auto.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted percentage in SpannableString format.</returns>
        public SpannableString GetHighAutoForTeamFormatted(int team)
        {
            int percent = GetHighAutoForTeam(team);

            if (percent >= Constants.autoBallThresh)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appGreen);
            else if (percent <= Constants.autoBallThresh)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appRed);
            else
                return FormatString.SetNormal(percent.ToString() + "%");
        }

        /// <summary>
        /// Returns the percent of the time a team scored no balls during auto.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Integer percentage.</returns>
        public int GetNoneAutoForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = GetCompiledForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].AutoResult));
            }
            double countNone = recs.Where(x => x.Equals(0)).Count();
            double countLow = recs.Where(x => x.Equals(1)).Count();
            double countHigh = recs.Where(x => x.Equals(2)).Count();
            return (int)(countNone / (countHigh + countLow + countNone) * 100);           
        }

        /// <summary>
        /// Returns the formatted percent of the time a team scored no balls during auto.
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Formatted percentage in SpannableString format.</returns>
        public SpannableString GetNoneAutoForTeamFormatted(int team)
        {
            int percent = GetNoneAutoForTeam(team);

            if (percent >= Constants.autoNoneThresh)
                return FormatString.SetColorBold(percent.ToString() + "%", Constants.appRed);            
            else
                return FormatString.SetBold(percent.ToString() + "%");
        }
    }
}