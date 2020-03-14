using SQLite;

namespace CardinalScout2020
{
    /// <summary>
    /// This class stores data for individual scouted matches. The data comes from the user input on the scouting form.
    /// </summary>
    public class MatchData
    {
        /// <summary>
        /// The PrimaryKey for a MatchData is this string ("eventID,matchNumber").
        /// </summary>
        [PrimaryKey]
        public string ID { get; set; }

        /// <summary>
        /// Default MatchData constructor used by the SQLite database.
        /// </summary>
        public MatchData()
        {
        }

        /// <summary>
        /// Paramaterized constructor for a MatchData. This MatchData will store all of the data for one team in one match.
        /// </summary>
        /// <param name="eventName"></param> Name of the event which the match is part of.
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="matchNumber"></param>
        /// <param name="teamNumber"></param>
        /// <param name="dsPosition"></param>
        /// <param name="isTable"></param>
        /// <param name="initiationCrossed"></param>
        /// <param name="autoResult"></param>
        /// <param name="shoot"></param>
        /// <param name="shootOuter"></param>
        /// <param name="shootInner"></param>
        /// <param name="shootLower"></param>
        /// <param name="shootTrench"></param>
        /// <param name="shootInitiation"></param>
        /// <param name="shootPort"></param>
        /// <param name="climb"></param>
        /// <param name="adjustClimb"></param>
        /// <param name="wheel"></param>
        /// <param name="rotationControl"></param>
        /// <param name="positionControl"></param>
        /// <param name="underTrench"></param>
        /// <param name="goodDrivers"></param>
        /// <param name="wouldRecommend"></param>
        /// <param name="result"></param>
        /// <param name="additionalComments"></param>
        /// <param name="isCurrentMatch"></param>
        /// <param name="eventID"></param>
        public MatchData(
            string eventName,
            string startDate,
            string endDate,
            int matchNumber,
            int teamNumber,
            int dsPosition,
            bool isTable,
            bool initiationCrossed,
            int autoResult,
            bool shoot,
            bool shootOuter,
            bool shootInner,
            bool shootLower,            
            bool shootTrench,
            bool shootInitiation,
            bool shootPort,
            bool climb,
            bool adjustClimb,
            bool wheel,
            bool rotationControl,
            bool positionControl,
            bool underTrench,
            bool goodDrivers,
            int wouldRecommend,
            int result,
            string additionalComments,
            bool isCurrentMatch,
            int eventID)
        {
            //Generate the MatchData's ID from the given EventID and match number.
            ID = eventID.ToString() + "," + matchNumber;
            EventName = eventName;
            StartDate = startDate;
            EndDate = endDate;
            MatchNumber = matchNumber;
            TeamNumber = teamNumber;
            DSPosition = dsPosition;
            IsTable = isTable;
            InitiationCrossed = initiationCrossed;
            AutoResult = autoResult;
            Shoot = shoot;
            ShootOuter = shootOuter;
            ShootInner = shootInner;
            ShootLower = shootLower;            
            ShootTrench = shootTrench;
            ShootInitiation = shootInitiation;
            ShootPort = shootPort;
            Climb = climb;
            AdjustClimb = adjustClimb;
            Wheel = wheel;
            RotationControl = rotationControl;
            PositionControl = positionControl;
            UnderTrench = underTrench;
            GoodDrivers = goodDrivers;
            WouldRecommend = wouldRecommend;
            Result = result;
            AdditionalComments = additionalComments;
            IsCurrentMatch = isCurrentMatch;
            EventID = eventID;
        }

        /// <summary>
        /// Name of the event which the match is part of.
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// Start date of the event which the match is part of.
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// End date of the event which the match is part of.
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// Match number for the MatchData.
        /// </summary>
        public int MatchNumber { get; set; }
        /// <summary>
        /// Team number for the MatchData.
        /// </summary>
        public int TeamNumber { get; set; }
        /// <summary>
        /// Driverstation position of the team during the match. 0-3 is Red 1-3 and 3-5 is Blue 1-3.
        /// </summary>
        public int DSPosition { get; set; }
        /// <summary>
        /// Whether or not a robot was nonfunctional, or a "table bot," during the match.
        /// </summary>
        public bool IsTable { get; set; }
        /// <summary>
        /// Whether or not the robot moved off the initation line during auto.
        /// </summary>
        public bool InitiationCrossed { get; set; }
        /// <summary>
        /// What the robot did during auto. 0:shot 0 balls; 1: shot 1-3 balls; 2: shot 4+ balls.
        /// </summary>
        public int AutoResult { get; set; }
        /// <summary>
        /// Can the robot can shoot balls.
        /// </summary>
        public bool Shoot { get; set; }
        /// <summary>
        /// Can the robot shoot in the outer port.
        /// </summary>
        public bool ShootOuter { get; set; }
        /// <summary>
        /// Can the robot shoot in the inner port.
        /// </summary>
        public bool ShootInner { get; set; }
        /// <summary>
        /// Can the robot shoot in the lower port.
        /// </summary>
        public bool ShootLower { get; set; }        
        /// <summary>
        /// Does the robot shoot from the trench run.
        /// </summary>
        public bool ShootTrench { get; set; }
        /// <summary>
        /// Does the robot shoot from the initiation line or nearby it.
        /// </summary>
        public bool ShootInitiation { get; set; }
        /// <summary>
        /// Does the robot shoot from against the Power Port wall.
        /// </summary>
        public bool ShootPort { get; set; }
        /// <summary>
        /// Can the robot climb.
        /// </summary>
        public bool Climb { get; set; }
        /// <summary>
        /// Can the robot adjust its position after it has climbed to balance.
        /// </summary>
        public bool AdjustClimb { get; set; }
        /// <summary>
        /// Can the robot spin the color wheel.
        /// </summary>
        public bool Wheel { get; set; }
        /// <summary>
        /// Did the robot perform rotation control.
        /// </summary>
        public bool RotationControl { get; set; }
        /// <summary>
        /// Did the robot perform position control.
        /// </summary>
        public bool PositionControl { get; set; }
        /// <summary>
        /// Can the robot fit under the trench run.
        /// </summary>
        public bool UnderTrench { get; set; }
        /// <summary>
        /// Did the robot's driveteam seem good and confident.
        /// </summary>
        public bool GoodDrivers { get; set; }
        /// <summary>
        /// Did the scouter recommend this team.
        /// </summary>
        public int WouldRecommend { get; set; }
        /// <summary>
        /// Any additional comments provided by a scouter about a team.
        /// </summary>
        public string AdditionalComments { get; set; }
        /// <summary>
        /// Result of the match. 0: Win; 1: Loss; 2: Tie.
        /// </summary>
        public int Result { get; set; }
        /// <summary>
        /// Whether or not the current MatchData is selected. Used when changing between activites to determine which MatchData should be loaded after one is selected by the user.
        /// </summary>
        public bool IsCurrentMatch { get; set; }
        /// <summary>
        /// Numerical ID of the event which the MatchData belongs to.
        /// </summary>
        public int EventID { get; set; }

        /// <summary>
        /// Determines the string representation of the team during the match ("Red 1," "Blue 2," etc.).
        /// </summary>
        /// <returns>A string representation of the driver station position of the team during the match.</returns>
        public string GetPosition()
        {
            if (DSPosition == 0)
            {
                return "Red 1";
            }
            else if (DSPosition == 1)
            {
                return "Red 2";
            }
            else if (DSPosition == 2)
            {
                return "Red 3";
            }
            else if (DSPosition == 3)
            {
                return "Blue 1";
            }
            else if (DSPosition == 4)
            {
                return "Blue 2";
            }
            else
            {
                return "Blue 3";
            }
        }

        /// <summary>
        /// Determines the string representation of what the robot did during auto (ex: "4+ Balls")
        /// </summary>
        /// <returns>A string representation of what the team did during auto.</returns>
        public string GetAuto()
        {
            if (AutoResult == 0)
            {
                return "0 Balls";
            }
            else if (AutoResult == 1)
            {
                return "1-3 Balls";
            }
            else
            {
                return "4+ Balls";
            }
        }

        /// <summary>
        /// Determines the string representation whether or not a team was recommended. ("Yes," "No," "Maybe").
        /// </summary>
        /// <returns>A string representation of the team's recommendation.</returns>
        public string GetRecommendation()
        {
            if (WouldRecommend == 0)
            {
                return "Yes";
            }
            else if (WouldRecommend == 1)
            {
                return "No";
            }
            else
            {
                return "Maybe";
            }
        }

        /// <summary>
        /// Determines the string representation of the result of the match. 0: win; 1: loss; 2: tie.
        /// </summary>
        /// <returns>A string representation of the result of the match.</returns>
        public string GetResult()
        {
            if (Result == 0)
            {
                return "Win";
            }
            else if (Result == 1)
            {
                return "Loss";
            }
            else
            {
                return "Tie";
            }
        }
    }
}