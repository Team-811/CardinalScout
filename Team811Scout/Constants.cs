using Android.Graphics;

namespace Team811Scout
{

    /*contains constants for calculations of the compiled data. Thresholds are the percents which determine between yes/no or true/false
     * Differences are the point at which two values will be considered "the same." For example, if a robot can do cargo 70% of the time and
     * hatches 85% of the time, the difference will be ignored and the robot will display as being able to do both. These values can only be
     * changed here*/

    public static class Constants
    {
        
        public static readonly int recommendThreshHigh = 75;
        public static readonly int recommendThreshLow = 50;
        public static readonly int winThreshHigh = 75;
        public static readonly int winThreshLow = 25;
        public static readonly int shootThreshHigh = 66;
        public static readonly int shootThreshLow = 33;
        public static readonly int climbThreshHigh = 66;
        public static readonly int climbThreshLow = 33;
        public static readonly int driversThreshHigh = 66;
        public static readonly int driversThreshLow = 33;
        public static readonly int tableThresh = 15;
        public static readonly int initiationThreshHigh = 60;
        public static readonly int initiationThreshLow = 30;
        public static readonly int autoNoneThresh = 25;
        public static readonly int autoBallThresh = 70;
        public static readonly double well_barelyWeight = 0.50;

        //colors
        public static readonly Color appGreen = Color.Rgb(0, 137, 9);
        public static readonly Color appRed = Color.Rgb(255, 0, 0);
        public static readonly Color appYellow = Color.Rgb(237, 162, 14);
        public static readonly Color appBlue = Color.Rgb(0, 0, 255);
        public static readonly Color appOrange = Color.Rgb(244, 127, 17);
    }
}