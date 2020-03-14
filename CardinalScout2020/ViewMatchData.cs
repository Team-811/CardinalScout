using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace CardinalScout2020
{
    /// <summary>
    /// This activity displays the data for a scouted match.
    /// </summary>
    [Activity(Label = "RecentData", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ViewMatchData: Activity
    {
        //Declare objects for controls.
        private TextView textRecent;
        private Button bDeleteMatch;
        private Button bEditMatch;
        private GridView gridRecent;

        //Placeholder for current match.
        private MatchData currentMatch;

        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set the correct screen.
            SetContentView(Resource.Layout.Match_Data);

            //Get controls from layout and assign event handlers.
            bDeleteMatch = FindViewById<Button>(Resource.Id.bDeleteMatch);
            bDeleteMatch.Click += ButtonClicked;
            bEditMatch = FindViewById<Button>(Resource.Id.bEditMatch);
            bEditMatch.Click += ButtonClicked;
            gridRecent = FindViewById<GridView>(Resource.Id.gridRecent);
            textRecent = FindViewById<TextView>(Resource.Id.textRecent);

            //Get the current match to display data for.
            currentMatch = ScoutDatabase.GetCurrentMatch();

            //Format title text.
            SpannableString[] textDisp = new SpannableString[]
            {
                FormatString.SetNormal("Viewing Data For - Match: "),
                FormatString.SetBold(currentMatch.MatchNumber.ToString()),
                FormatString.SetNormal(" /// Team: "),
                FormatString.SetBold(currentMatch.TeamNumber.ToString())
            };

            //Set title text.
            textRecent.TextFormatted = new SpannableString(TextUtils.ConcatFormatted(textDisp));
            
            //Make display lists.
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
            
            //Format the properties.
            for (int i = 0; i < propertiesPre.Length; i++)
            {
                properties.Add(FormatString.SetBold(propertiesPre[i]));
            }

            //Get the raw data from the match.
            string[] dataPre = new string[]
            {
               currentMatch.MatchNumber.ToString(),
               currentMatch.TeamNumber.ToString(),
               currentMatch.GetResult(),
               currentMatch.GetPosition(),
               currentMatch.IsTable.ToString().ToUpper(),
               currentMatch.InitiationCrossed.ToString(),
               currentMatch.GetAuto(),
               currentMatch.Shoot.ToString(),
               currentMatch.ShootOuter.ToString(),
               currentMatch.ShootInner.ToString(),
               currentMatch.ShootLower.ToString(),               
               currentMatch.ShootTrench.ToString(),
               currentMatch.ShootInitiation.ToString(),
               currentMatch.ShootPort.ToString(),
               currentMatch.Climb.ToString(),
               currentMatch.AdjustClimb.ToString(),
               currentMatch.Wheel.ToString(),
               currentMatch.RotationControl.ToString(),
               currentMatch.PositionControl.ToString(),
               currentMatch.UnderTrench.ToString(),               
               currentMatch.GoodDrivers.ToString(),
               currentMatch.GetRecommendation(),
               currentMatch.AdditionalComments,
            };

            //Format the data.
            for (int i = 0; i < dataPre.Length; i++)
            {
                data.Add(FormatString.SetNormal(dataPre[i]));
            }

            //Combine properties and data.
            for (int i = 0; i < properties.Count; i++)
            {
                display.Add(properties[i]);
                display.Add(data[i]);
            }

            //Adapt the combined list to be displayed in the grid.
            ArrayAdapter gridAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridRecent.Adapter = gridAdapt;
        }

        //Handle button clicks.
        private void ButtonClicked(object sender, EventArgs e)
        {
            //Decide which button was pressed.
            if ((sender as Button) == bDeleteMatch)
            {
                Popup.Double("Alert", "Are you sure you want to delete match " + currentMatch.MatchNumber + "?", "Yes", "CANCEL", this, Delete);
                void Delete()
                {
                    ScoutDatabase.DeleteMatchData(currentMatch.ID);
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