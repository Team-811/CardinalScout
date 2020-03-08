using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace CardinalScout2020
{
    /*This class displays the data for a scouted match in a list*/

    [Activity(Label = "RecentData", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ViewMatchData: Activity
    {
        //get database instance
        private EventDatabase eData = new EventDatabase();

        private MatchData currentMatch;

        //declare objects for controls
        private TextView textRecent;

        private Button bDeleteMatch;
        private Button bEditMatch;
        private GridView gridRecent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Match_Data);
            //get controls from layout and assign event handlers
            bDeleteMatch = FindViewById<Button>(Resource.Id.bDeleteMatch);
            bDeleteMatch.Click += ButtonClicked;
            bEditMatch = FindViewById<Button>(Resource.Id.bEditMatch);
            bEditMatch.Click += ButtonClicked;
            gridRecent = FindViewById<GridView>(Resource.Id.gridRecent);
            textRecent = FindViewById<TextView>(Resource.Id.textRecent);
            //get the current match to display data for
            currentMatch = eData.GetCurrentMatch();
            SpannableString[] textDisp = new SpannableString[]
            {
                FormatString.setNormal("Viewing Data For - Match: "),
                FormatString.setBold(currentMatch.matchNumber.ToString()),
                FormatString.setNormal(" /// Team: "),
                FormatString.setBold(currentMatch.teamNumber.ToString())
            };
            //set title text
            textRecent.TextFormatted = new SpannableString(TextUtils.ConcatFormatted(textDisp));
            //make display lists
            List<SpannableString> data = new List<SpannableString>();
            List<SpannableString> properties = new List<SpannableString>();
            List<SpannableString> display = new List<SpannableString>();
            string[] propertiesPre = new string[]
            {
                "Match Number",
                "Team Number",
                "Result of Team's Alliance",
                "Position",
                "Table",
                "Crossed Initiation Line",
                "Auto",
                "Can Shoot",
                "Outer Port",
                "Inner Port",
                "Lower Port",
                "Shoots Well",
                "Barely Shoots",
                "Shoots from Trench",
                "Shoots from Initiation Line",
                "Shoots from Against Power Port",
                "Climb",
                "Adjusts on Climbing Bar",
                "Can Spin Color Wheel",
                "Rotation Control",
                "Position Control",
                "Fits Under Trench",
                "Good Driveteam",
                "Recommended",
                "Additional Comments",
            };
            //format the properties
            for (int i = 0; i < propertiesPre.Length; i++)
            {
                properties.Add(FormatString.setBold(propertiesPre[i]));
            }
            string[] dataPre = new string[]
            {
               currentMatch.matchNumber.ToString(),
               currentMatch.teamNumber.ToString(),
               currentMatch.getResult(),
               currentMatch.getPosition(),
               currentMatch.isTable.ToString().ToUpper(),
               currentMatch.initiationCrossed.ToString(),
               currentMatch.getAuto(),
               currentMatch.shoot.ToString(),
               currentMatch.shootOuter.ToString(),
               currentMatch.shootInner.ToString(),
               currentMatch.shootLower.ToString(),
               currentMatch.shootWell.ToString(),
               currentMatch.shootBarely.ToString(),
               currentMatch.shootTrench.ToString(),
               currentMatch.shootLine.ToString(),
               currentMatch.shootPort.ToString(),
               currentMatch.climb.ToString(),
               currentMatch.adjustClimb.ToString(),
               currentMatch.wheel.ToString(),
               currentMatch.rotationControl.ToString(),
               currentMatch.positionControl.ToString(),
               currentMatch.underTrench.ToString(),               
               currentMatch.goodDrivers.ToString(),
               currentMatch.getRecommendation(),
               currentMatch.additionalComments,
            };
            //format the data
            for (int i = 0; i < dataPre.Length; i++)
            {
                data.Add(FormatString.setNormal(dataPre[i]));
            }
            //combine properties and data
            for (int i = 0; i < properties.Count; i++)
            {
                display.Add(properties[i]);
                display.Add(data[i]);
            }
            //adapt the lists to be displayed in the grid
            ArrayAdapter gridAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridRecent.Adapter = gridAdapt;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            //decide which button was pressed
            if ((sender as Button) == bDeleteMatch)
            {
                Popup.Double("Alert", "Are you sure you want to delete match " + currentMatch.matchNumber + "?", "Yes", "CANCEL", this, Delete);
                void Delete()
                {
                    eData.DeleteMatchData(currentMatch.ID);
                    StartActivity(typeof(ScoutLandingPage));
                    Finish();
                }
            }
            else if ((sender as Button) == bEditMatch)
            {
                StartActivity(typeof(ScoutFormEdit));
            }
        }
    }
}