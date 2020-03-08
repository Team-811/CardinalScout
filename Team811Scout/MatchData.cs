using SQLite;

namespace Team811Scout
{
    /*This class stores data for individual scouted matches. The data comes from the user input
     on the scouting form*/

    public class MatchData
    {
        //the PrimaryKey for this class is a string (eventID,matchNumber)
        [PrimaryKey]
        public string ID { get; set; }

        public MatchData()
        {
        }

        public MatchData(
            string name,
            string start,
            string end,
            int matchNum,
            int teamNum,
            int pos,
            bool table,
            bool initiation,
            int autoResult,

            bool s,
            bool sOuter,
            bool sInner,
            bool sLower,
            bool sWell,
            bool sBarely,
            bool sTrench,
            bool sLine,
            bool sPort,

            bool climb,
            bool adjust,

            bool cWheel,
            bool rControl,
            bool pControl,

            bool under,

            bool Drivers,
            int Recommend,
            int res,
            string comments,
            bool isCurrent,
            int eventid)
        {
            //(eventID,matchNumber
            ID = eventid.ToString() + "," + matchNum;
            eventName = name;
            startDate = start;
            endDate = end;
            matchNumber = matchNum;
            teamNumber = teamNum;
            position = pos;
            isTable = table;
            initiationCrossed = initiation;
            auto = autoResult;

            shoot = s;
            shootOuter = sOuter;
            shootInner = sInner;
            shootLower = sLower;
            shootWell = sWell;
            shootBarely = sBarely;
            shootTrench = sTrench;
            shootLine = sLine;
            shootPort = sPort;

            wheel = cWheel;
            rotationControl = rControl;
            positionControl = pControl;

            underTrench = under;            

            goodDrivers = Drivers;
            wouldRecommend = Recommend;
            result = res;
            additionalComments = comments;
            isCurrentMatch = isCurrent;
            eventID = eventid;
        }

        //make sure we can easily identify which event the match belongs to
        public int eventID { get; set; }

        public int matchNumber { get; set; }
        public int teamNumber { get; set; }
        public int position { get; set; }

        //0-2: red 1-3; 3-5: blue 1-3
        //get string representation of position
        public string getPosition()
        {
            if (position == 0)
            {
                return "Red 1";
            }
            else if (position == 1)
            {
                return "Red 2";
            }
            else if (position == 2)
            {
                return "Red 3";
            }
            else if (position == 3)
            {
                return "Blue 1";
            }
            else if (position == 4)
            {
                return "Blue 2";
            }
            else
            {
                return "Blue 3";
            }
        }

        public bool isTable { get; set; }
        public bool initiationCrossed { get; set; }
        public int auto { get; set; }

        //0 - 0 balls, 1 - 1-3 balls, 2 - 4+ balls
        //string representation of sandstorm mode
        public string getAuto()
        {
            if (auto == 0)
            {
                return "0 Balls";
            }
            else if (auto == 1)
            {
                return "1-3 Balls";
            }
            else
            {
                return "4+ Balls";
            }
        }       

        public bool shoot { get; set; }
        public bool shootOuter { get; set; }
        public bool shootInner { get; set; }        
        public bool shootLine { get; set; }
        public bool shootWell { get; set; }
        public bool shootBarely { get; set; }
        public bool shootLower { get; set; }
        public bool shootTrench { get; set; }
        public bool shootPort { get; set; }

        public bool climb { get; set; }
        public bool adjustClimb { get; set; }

        public bool wheel { get; set; }
        public bool rotationControl { get; set; }
        public bool positionControl { get; set; }

        public bool underTrench { get; set; }  

        public bool goodDrivers { get; set; }
        public int wouldRecommend { get; set; }

        //0 - yes; 1 - no; 2 - maybe
        //string for recommendation
        public string getRecommendation()
        {
            if (wouldRecommend == 0)
            {
                return "Yes";
            }
            else if (wouldRecommend == 1)
            {
                return "No";
            }
            else
            {
                return "Maybe";
            }
        }

        public int result { get; set; }

        //0 - win; 1 - lose; 2 - tie
        //string for result
        public string getResult()
        {
            if (result == 0)
            {
                return "Win";
            }
            else if (result == 1)
            {
                return "Loss";
            }
            else
            {
                return "Tie";
            }
        }

        public string additionalComments { get; set; }
        public string eventName { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool isCurrentMatch { get; set; }
    }
}