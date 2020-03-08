using System;
using System.Collections.Generic;
using System.Linq;

namespace Team811Scout
{
    /*The CompiledEventData class extends the MatchData class. It contains information for all
     * scouted matches for an event from all devices. Compiled Event Data can have all the same properties as
     * a normal MatchData, but it also can perform calculations and figure out percentages*/


    public class CompiledEventData: MatchData
    {
        public CompiledEventData()
        {
        }

        public CompiledEventData(string officialname, string start, string end, string qrdata, bool isactive, int id)
        {
            //make the compiled event data mimic the event details
            officialName = officialname;
            startDate = start;
            endDate = end;
            //CompiledEventData specific data
            rawData = qrdata;
            cID = id;
            ID = cID.ToString();
            isActive = isactive;
            //note the date and time modified
            dateMod = DateTime.Now.ToString("MM/dd/yyyy");
            timeMod = DateTime.Now.ToShortTimeString();
        }

        public string officialName { get; set; }

        //string of data collected from the QR codes
        public string rawData { get; set; }

        public bool isActive { get; set; }
        public string dateMod { get; set; }
        public string timeMod { get; set; }

        //the official primary key for this class is a string, but cID is used in the database because the string
        //is match-specific and this class deals with the entire event
        //the cID matches the event id for the compiled event data
        public int cID { get; set; }

        //read QR data and return it in a multidimensional list of matches grouped by team number
        public List<List<CompiledEventData>> compileData()
        {
            //how many properties are we looking for:
            const int dataLength = 22;
            CompiledEventData placeholder = new CompiledEventData();
            //list before putting it in team order
            List<CompiledEventData> preOrder = new List<CompiledEventData>();
            //get a substring for the raw data
            string matchData = rawData;
            int substringStart = 0;
            while (matchData.Length > 1)
            {
                int startIndex = 0;
                //give default values to the placeholder
                placeholder = new CompiledEventData();
                placeholder.officialName = officialName;
                placeholder.cID = cID;
                placeholder.isActive = false;
                //set values which appear at the beginning of the QR string separated by commas
                List<int> matchCommas = AllIndexesOf(matchData, ",").ToList();
                placeholder.teamNumber = int.Parse(matchData.Substring(0, matchCommas[0]));
                placeholder.matchNumber = int.Parse(matchData.Substring(matchCommas[0] + 1, matchCommas[1] - matchCommas[0] - 1));
                //create substring of data after the commas
                matchData = matchData.Substring(matchCommas[1] + 1, dataLength);

                //interpret numerical values
                placeholder.result = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;
                placeholder.position = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;
                placeholder.isTable = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.initiationCrossed = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.auto = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;
                placeholder.shoot = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.shootOuter = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.shootInner = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.shootLower = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.shootWell = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.shootBarely = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.shootTrench = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.shootLine = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.shootPort = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.climb = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.adjustClimb = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.wheel = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.rotationControl = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.positionControl = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.underTrench = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.goodDrivers = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.wouldRecommend = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;
                //add the CompiledEventData
                preOrder.Add(placeholder);
                //move to the next team in the string
                substringStart += dataLength + matchCommas[1] + 1;
                matchData = rawData.Substring(substringStart);
            }
            //convert to an array to sort by team number
            CompiledEventData[] dataSorted = preOrder.ToArray();
            Array.Sort(dataSorted, delegate (CompiledEventData data1, CompiledEventData data2)
            {
                return data1.teamNumber.CompareTo(data2.teamNumber);
            });
            //create a list of the teams contained in the raw data
            List<int> uniqueTeams = new List<int>();
            foreach (CompiledEventData data in dataSorted)
            {
                if (!uniqueTeams.Contains(data.teamNumber))
                {
                    uniqueTeams.Add(data.teamNumber);
                }
            }
            //create a multidimensional list to group that data
            List<List<CompiledEventData>> groupedData = new List<List<CompiledEventData>>();
            groupedData.Add(new List<CompiledEventData>());
            //starting first index
            int c = 0;
            foreach (CompiledEventData data in dataSorted)
            {
                //add all matches by one team to one index of the list
                if (data.teamNumber == uniqueTeams[c])
                {
                    groupedData[c].Add(data);
                }
                //go to the next index after all matches by that team are added
                else
                {
                    c++;
                    groupedData.Add(new List<CompiledEventData>());
                    groupedData[c].Add(data);
                }
            }
            //return the grouped data list
            return groupedData;
        }

        //used to find where commas are in the rawData in order to split it up into teams/matches
        private IEnumerable<int> AllIndexesOf(string str, string searchstring)
        {
            int minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
            }
        }

        //Calculations with MatchData
        //Group calculations (arrays of percents/values for all teams in the data)
        //used for display purposes

        //list team numbers that have data
        public List<int> getTeamNumbersArray()
        {
            List<List<CompiledEventData>> c = compileData();
            List<int> result = new List<int>();
            for (int i = 0; i < c.Count; i++)
            {
                result.Add(c[i][0].teamNumber);
            }
            return result;
        }

        //Percent of scouters who recommended the team
        public List<int> getRecPercentArray()
        {
            List<List<CompiledEventData>> c = compileData();
            List<int> recsPerTeam = new List<int>();
            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();
                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].wouldRecommend);
                }
                double countYes = recs.Where(x => x.Equals(0)).Count();
                double countNo = recs.Where(x => x.Equals(1)).Count();
                double countMaybe = recs.Where(x => x.Equals(2)).Count();
                double percent = (countYes + (0.5 * countMaybe)) / (countYes + countMaybe + countNo) * 100;
                recsPerTeam.Add((int)Math.Round(percent));
            }
            return recsPerTeam;
        }

        //W-L-T record
        public List<string> getWinRecordArray()
        {
            List<string> recsPerTeam = new List<string>();
            List<List<CompiledEventData>> c = compileData();
            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();
                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].result);
                }
                int countWin = recs.Where(x => x.Equals(0)).Count();
                int countLoss = recs.Where(x => x.Equals(1)).Count();
                int countTie = recs.Where(x => x.Equals(2)).Count();
                recsPerTeam.Add(countWin.ToString() + "-" + countLoss.ToString() + "-" + countTie.ToString());
            }
            return recsPerTeam;
        }

        //Percent of time the team won
        public List<int> getWinPercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<List<CompiledEventData>> c = compileData();
            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();
                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].result);
                }
                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].result);
                }
                double countWin = recs.Where(x => x.Equals(0)).Count();
                double countLoss = recs.Where(x => x.Equals(1)).Count();
                double countTie = recs.Where(x => x.Equals(2)).Count();
                double percent = countWin / (countLoss + countTie) * 100;
                recsPerTeam.Add((int)Math.Round(percent));
            }
            return recsPerTeam;
        }

        //Percent of the time the team was able to shoot
        public List<int> getShootPercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<int> recsWellPerTeam = new List<int>();
            List<int> recsBarelyPerTeam = new List<int>();
            List<List<CompiledEventData>> c = compileData();
            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();
                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].shoot));
                    recsWellPerTeam.Add(Convert.ToByte(c[i][j].shootWell));
                    recsBarelyPerTeam.Add(Convert.ToByte(c[i][j].shootBarely));
                }
                double countYes = recs.Where(x => x.Equals(1)).Count();
                double countNo = recs.Where(x => x.Equals(0)).Count();
                double countWell = recsWellPerTeam.Where(x => x.Equals(1)).Count();
                double countBarely = recsBarelyPerTeam.Where(x => x.Equals(1)).Count();
                double percent = (countYes + countWell * Constants.well_barelyWeight) / (countYes + countNo + countBarely * Constants.well_barelyWeight) * 100;
                if (percent > 100)
                {
                    percent = 100;
                }
                recsPerTeam.Add((int)Math.Round(percent));
            }
            return recsPerTeam;
        }

        //Get teams' primary shooting port
        public List<string> getPrimaryPortArray()
        {
            List<int> recsOuter = new List<int>();
            List<int> recsInner = new List<int>();
            List<int> recsLower = new List<int>();

            List<string> modes = new List<string>();

            List<List<CompiledEventData>> c = compileData();
            for (int i = 0; i < c.Count; i++)
            {
                for (int j = 0; j < c[i].Count; j++)
                {
                    recsOuter.Add(Convert.ToByte(c[i][j].shootOuter));
                    recsInner.Add(Convert.ToByte(c[i][j].shootInner));
                    recsLower.Add(Convert.ToByte(c[i][j].shootLower));
                }
                double outer = recsOuter.Where(x => x.Equals(1)).Count();
                double inner = recsInner.Where(x => x.Equals(1)).Count();
                double lower = recsLower.Where(x => x.Equals(1)).Count();

                if (outer == 0 && inner == 0 && lower == 0)
                    modes.Add("None");
                else if (inner > outer && inner > lower)
                    modes.Add("Inner");
                else if (lower > inner && lower > outer)
                    modes.Add("Lower");
                else
                    modes.Add("Outer");
            }
            return modes;
        }

        //Percent of the time the team was able to climb
        public List<int> getClimbPercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<List<CompiledEventData>> c = compileData();
            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();
                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].climb));
                }
                double countYes = recs.Where(x => x.Equals(1)).Count();
                double countNo = recs.Where(x => x.Equals(0)).Count();
                double percent = ((countYes) / (countYes + countNo)) * 100;
                recsPerTeam.Add((int)Math.Round(percent));
            }
            return recsPerTeam;
        }

        //Did the team adjust their climb at all
        public List<bool> getClimbAdjustArray()
        {
            List<bool> recsPerTeam = new List<bool>();
            List<List<CompiledEventData>> c = compileData();
            for (int i = 0; i < c.Count; i++)
            {
                for (int j = 0; j < c[i].Count; j++)
                {
                    recsPerTeam.Add(c[i][j].adjustClimb);
                }
            }
            return recsPerTeam;
        }

        //Did the team spin the wheel at all
        public List<bool> getWheelArray()
        {
            List<bool> recsPerTeam = new List<bool>();
            List<List<CompiledEventData>> c = compileData();
            for (int i = 0; i < c.Count; i++)
            {
                for (int j = 0; j < c[i].Count; j++)
                {
                    recsPerTeam.Add(c[i][j].wheel);
                }
            }
            return recsPerTeam;
        }

        //Percent of the time drivers were good
        public List<int> getDriversPercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<List<CompiledEventData>> c = compileData();
            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();
                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].goodDrivers));
                }
                double countYes = recs.Where(x => x.Equals(1)).Count();
                double countNo = recs.Where(x => x.Equals(0)).Count();
                double percent = ((countYes) / (countYes + countNo)) * 100;
                recsPerTeam.Add((int)Math.Round(percent));
            }
            return recsPerTeam;
        }

        //Percent of the time a team was not functioning
        public List<int> getTablePercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<List<CompiledEventData>> c = compileData();
            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();
                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].isTable));
                }
                double countYes = recs.Where(x => x.Equals(1)).Count();
                double countNo = recs.Where(x => x.Equals(0)).Count();
                double percent = (countYes) / (countYes + countNo) * 100;
                recsPerTeam.Add((int)Math.Round(percent));
            }
            return recsPerTeam;
        }

        //calculations for an individual team (for detailed display)
        //get a list of all matchdatas for one team
        private List<CompiledEventData> dataForTeam(int team)
        {
            int index = -1;
            for (int i = 0; i < compileData().Count; i++)
            {
                if (compileData()[i][0].teamNumber == team)
                {
                    index = i;
                    break;
                }
            }
            List<CompiledEventData> c = compileData()[index];
            return c;
        }

        //Percent of scouters who recommended the team
        public int getRecPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(c[i].wouldRecommend);
            }
            double countYes = recs.Where(x => x.Equals(0)).Count();
            double countNo = recs.Where(x => x.Equals(1)).Count();
            double countMaybe = recs.Where(x => x.Equals(2)).Count();
            double percent = (countYes + (0.5 * countMaybe)) / (countYes + countMaybe + countNo) * 100;
            return (int)percent;
        }

        //Percent of matches a team could shoot
        public int getShootPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].shoot));
            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();
            double percent = (countYes) / (countYes + countNo) * 100;
            return (int)percent;
        }

        //Which port does the team primarily score in
        public string getPrimaryPort(int team)
        {
            List<CompiledEventData> c = dataForTeam(team);
            int outer = 0;
            int inner = 0;
            int lower = 0;

            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].shootOuter)
                    outer++;
                if (c[i].shootInner)
                    inner++;
                if (c[i].shootLower)
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

        //From where on the field does the team typically shoot
        public string getPrimaryShoot(int team)
        {
            List<CompiledEventData> c = dataForTeam(team);
            int trench = 0;
            int line = 0;
            int port = 0;

            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].shootTrench)
                    trench++;
                if (c[i].shootLine)
                    line++;
                if (c[i].shootPort)
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

        //Percent of the time a team climbed
        public int getClimbPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].climb));
            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();
            double percent = (countYes) / (countYes + countNo) * 100;
            return (int)percent;
        }

        //Did they adjust once climbed
        public bool getAdjustForTeam(int team)
        {
            int count = 0;
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].adjustClimb)
                    count++;
            }

            if (count > 0)
                return true;
            return false;
        }

        //Can the team spin the wheel
        public bool getWheelForTeam(int team)
        {
            int count = 0;
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].wheel)
                    count++;
            }

            if (count > 0)
                return true;
            return false;
        }

        //Can the team do rotation control
        public bool getWheelRotationForTeam(int team)
        {
            int count = 0;
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].rotationControl)
                    count++;
            }

            if (count > 0)
                return true;
            return false;
        }

        //Can the team do position control
        public bool getWheelPositionForTeam(int team)
        {
            int count = 0;
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].positionControl)
                    count++;
            }

            if (count > 0)
                return true;
            return false;
        }

        //Percent of the time the drivers were good
        public int getDriversPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].goodDrivers));
            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();
            double percent = ((countYes) / (countYes + countNo)) * 100;
            return (int)percent;
        }

        //Percent of the time the team did nothing
        public int getTablePercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].isTable));
            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();
            double percent = (countYes) / (countYes + countNo) * 100;
            return (int)percent;
        }

        //Autonomous

        //Percent of the time a team moved off the initiation line
        public int getInitiationLineForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].initiationCrossed));
            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();
            double percent = (countYes) / (countYes + countNo) * 100;
            return (int)percent;
        }

        //Percent of the time a team scored between 1 and 3 balls during auto
        public int getLowAutoForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].auto));
            }
            double countNone = recs.Where(x => x.Equals(0)).Count();
            double countLow = recs.Where(x => x.Equals(1)).Count();
            double countHigh = recs.Where(x => x.Equals(2)).Count();
            double percent = (countLow) / (countHigh + countNone + countLow) * 100;
            return (int)percent;
        }

        //Percent of the time a team scored 4+ balls during auto
        public int getHighAutoForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].auto));
            }
            double countNone = recs.Where(x => x.Equals(0)).Count();
            double countLow = recs.Where(x => x.Equals(1)).Count();
            double countHigh = recs.Where(x => x.Equals(2)).Count();
            double percent = (countHigh) / (countLow + countNone + countHigh) * 100;
            return (int)percent;
        }

        //Percent of the time a team scored 0 balls during auto
        public int getNoneAutoForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].auto));
            }
            double countNone = recs.Where(x => x.Equals(0)).Count();
            double countLow = recs.Where(x => x.Equals(1)).Count();
            double countHigh = recs.Where(x => x.Equals(2)).Count();
            double percent = (countNone) / (countHigh + countLow + countNone) * 100;
            return (int)percent;
        }
    }
}