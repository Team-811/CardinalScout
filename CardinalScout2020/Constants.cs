using Android.Graphics;

namespace CardinalScout2020
{
    /// <summary>
    /// Contains constants for calculations of the compiled data. These thresholds determine whether or not a robot's mechanism is good enough to be given a "good" rating rather than a "bad" or "neutral" rating. If the mechanism is successful more than the percentage of the high threshold, it will be "good." If it is lower than the low threshold, it will be "bad." In between, it is given a "neutral" rating. These values are given in percent of the time the robot was able to complete the task. This class also contains the colors used throughout the app.
    /// </summary>
    public static class Constants
    {
        /* Thresholds */

        /// <summary>
        /// Threshold for a robot to be recommended.
        /// </summary>
        public static readonly double recommendThreshHigh = 75;
        /// <summary>
        /// Threshold for a robot not to be recommended.
        /// </summary>
        public static readonly double recommendThreshLow = 50;
        /// <summary>
        /// Threshold to determine if a team's win/loss record is good.
        /// </summary>
        public static readonly double winThreshHigh = 75;
        /// <summary>
        /// Threshold to determine if a team's win/loss record is bad.
        /// </summary>
        public static readonly double winThreshLow = 25;
        /// <summary>
        /// Threshold to determine if a team's shooter is good.
        /// </summary>
        public static readonly double shootThreshHigh = 66;
        /// <summary>
        /// Threshold to determine if a team's shooter is bad.
        /// </summary>
        public static readonly double shootThreshLow = 33;
        /// <summary>
        /// Threshold to determine if a team's climber is good.
        /// </summary>
        public static readonly double climbThreshHigh = 66;
        /// <summary>
        /// Threshold to determine if a team's climber is bad.
        /// </summary>
        public static readonly double climbThreshLow = 33;
        /// <summary>
        /// Threshold to determine if a team has good drivers.
        /// </summary>
        public static readonly double driversThreshHigh = 66;
        /// <summary>
        /// Threshold to determine if the team has bad drivers.
        /// </summary>
        public static readonly double driversThreshLow = 33;
        /// <summary>
        /// Threshold under which a robot is considered non-functional.
        /// </summary>
        public static readonly double tableThresh = 15;
        /// <summary>
        /// Threshold to determine if the robot can consistently cross the initiation line.
        /// </summary>
        public static readonly double initiationThreshHigh = 60;
        /// <summary>
        /// Threshold to determine if the robot cannot consistently cross the initiation line.
        /// </summary>
        public static readonly double initiationThreshLow = 30;
        /// <summary>
        /// Threshold to determine if a robot doesn't perform well enough during auto.
        /// </summary>
        public static readonly double autoNoneThresh = 25;
        /// <summary>
        /// Threshold to determine if a robot can consistently shoot during auto.
        /// </summary>
        public static readonly double autoBallThresh = 70;        

        /* Colors */

        /// <summary>
        /// Standard green.
        /// </summary>
        public static readonly Color appGreen = Color.Rgb(0, 137, 9);
        /// <summary>
        /// Standard red.
        /// </summary>
        public static readonly Color appRed = Color.Rgb(255, 0, 0);
        /// <summary>
        /// Standard yellow.
        /// </summary>
        public static readonly Color appYellow = Color.Rgb(237, 162, 14);
        /// <summary>
        /// Standard blue.
        /// </summary>
        public static readonly Color appBlue = Color.Rgb(0, 0, 255);
        /// <summary>
        /// Standard orange.
        /// </summary>
        public static readonly Color appOrange = Color.Rgb(244, 127, 17);
    }
}