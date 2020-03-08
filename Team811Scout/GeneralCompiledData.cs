using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    /*This activity displays general compiled data for an event. It displays the teams involved in the event
     * and their overall percentages based off of performance in matches*/

    [Activity(Label = "GeneralCompiledData", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class GeneralCompiledData: Activity
    {
        //declare objects for controls
        private TextView textRecent;

        private string[] properties;
        private Button bDeleteData;
        private GridView gridTeams;
        private LinearLayout viewDataHeight;

        //placeholder for current compiled event data to view
        private CompiledEventData currentCompiled;

        //get database instance
        private EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.General_Compiled);
            //get controls from layout
            bDeleteData = FindViewById<Button>(Resource.Id.bDeleteData);
            bDeleteData.Click += ButtonClicked;
            gridTeams = FindViewById<GridView>(Resource.Id.gridViewTeam);
            gridTeams.ItemClick += gridClicked;
            textRecent = FindViewById<TextView>(Resource.Id.textEvent);
            viewDataHeight = FindViewById<LinearLayout>(Resource.Id.viewDataHeight);
            //get current compiled data
            currentCompiled = eData.GetCurrentCompiled();
            SpannableString[] textDisp = new SpannableString[]
            {
                FormatString.setNormal("Viewing Data for Event: \n"),
                FormatString.setBold("'"+currentCompiled.officialName+"'"),
                FormatString.setNormal(" as of "),
                FormatString.setBold(currentCompiled.dateMod),
                FormatString.setNormal(" at "),
                FormatString.setBold(currentCompiled.timeMod)
            };
            //set title display text
            textRecent.TextFormatted = TextUtils.ConcatFormatted(textDisp);
            //column titles
            properties = new string[]
            {
                "Team",               
                "Recommend %",
                "Record",
                "Shoot% / Port",
                "Climb% / Adjust",
                "Color Wheel",
                "Good Drivers (%)",
                "'Table' %"
            };
            //get general data for all teams
            List<int> teamNumbers = currentCompiled.getTeamNumbersArray();
            List<int> recPerc = currentCompiled.getRecPercentArray();
            List<string> record = currentCompiled.getWinRecordArray();
            List<int> shootPerc = currentCompiled.getShootPercentArray();
            List<string> prefPort = currentCompiled.getPrimaryPortArray();
            List<int> climbPerc = currentCompiled.getClimbPercentArray();
            List<bool> climbAdj = currentCompiled.getClimbAdjustArray();
            List<bool> cWheel = currentCompiled.getWheelArray();
            List<int> driversPerc = currentCompiled.getDriversPercentArray();
            List<int> tablePerc = currentCompiled.getTablePercentArray();
            List<int> winPerc = currentCompiled.getWinPercentArray();
            List<SpannableString> display = new List<SpannableString>();
            //format data in the right order for the grid based on values
            for (int i = 0; i < properties.Length; i++)
            {
                display.Add(FormatString.setBold(properties[i]));
            }
            for (int i = 0; i < currentCompiled.compileData().Count; i++)
            {
                //add the team number
                display.Add(FormatString.setBold(teamNumbers[i].ToString()));
                //recommendation percent
                if (recPerc[i] >= Constants.recommendThreshHigh)
                {
                    display.Add(FormatString.setColorBold(recPerc[i].ToString() + "%", Constants.appGreen));
                }
                else if (recPerc[i] <= Constants.recommendThreshLow)
                {
                    display.Add(FormatString.setColorBold(recPerc[i].ToString() + "%", Constants.appRed));
                }
                else
                {
                    display.Add(FormatString.setNormal(recPerc[i].ToString() + "%"));
                }
                //win-loss-tie record
                if (winPerc[i] >= Constants.winThreshHigh)
                {
                    display.Add(FormatString.setColorBold(record[i], Constants.appGreen));
                }
                else if (winPerc[i] <= Constants.winThreshLow)
                {
                    display.Add(FormatString.setColorBold(record[i], Constants.appRed));
                }
                else
                {
                    display.Add(FormatString.setNormal(record[i]));
                }
                //Shooter data
                if (shootPerc[i] >= Constants.shootThreshHigh)
                {
                    display.Add(FormatString.setColorBold(shootPerc[i].ToString() + "% / " + prefPort[i], Constants.appGreen));
                }
                else if (shootPerc[i] <= Constants.shootThreshLow)
                {
                    display.Add(FormatString.setColorBold(shootPerc[i].ToString() + "% / " + prefPort[i], Constants.appRed));
                }
                else
                {
                    display.Add(FormatString.setNormal(shootPerc[i].ToString() + "% / " + prefPort[i]));
                }
                //Climbing Data
                if (climbPerc[i] >= Constants.climbThreshHigh)
                {
                    display.Add(FormatString.setColorBold(climbPerc[i].ToString() + "% / " + climbAdj[i].ToString(), Constants.appGreen));
                }
                else if (shootPerc[i] <= Constants.climbThreshLow)
                {
                    display.Add(FormatString.setColorBold(climbPerc[i].ToString() + "% / " + climbAdj[i].ToString(), Constants.appRed));
                }
                else
                {
                    display.Add(FormatString.setNormal(climbPerc[i].ToString() + "% / " + climbAdj[i].ToString()));
                }
                //Wheel Data
                if (cWheel[i])
                {
                    display.Add(FormatString.setColorBold(cWheel[i].ToString(), Constants.appGreen));
                }
                else
                {
                    display.Add(FormatString.setColorBold(cWheel[i].ToString(), Constants.appRed));
                }               
                //Driveteam Data
                if (driversPerc[i] >= Constants.driversThreshHigh)
                {
                    display.Add(FormatString.setColorBold(driversPerc[i].ToString() + "%", Constants.appGreen));
                }
                else if (driversPerc[i] <= Constants.driversThreshLow)
                {
                    display.Add(FormatString.setColorBold(driversPerc[i].ToString() + "%", Constants.appRed));
                }
                else
                {
                    display.Add(FormatString.setNormal(driversPerc[i].ToString() + "%"));
                }
                //Table Data
                if (tablePerc[i] >= Constants.tableThresh)
                {
                    display.Add(FormatString.setColorBold(tablePerc[i].ToString() + "%", Constants.appRed));
                }
                else
                {
                    display.Add(FormatString.setColorBold(tablePerc[i].ToString() + "%", Constants.appGreen));
                }
            }
            //display data in the grid
            ArrayAdapter gridAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridTeams.Adapter = gridAdapt;
            float scale = this.Resources.DisplayMetrics.Density;
            FrameLayout.LayoutParams _params = new FrameLayout.LayoutParams((int)(1200 * scale), LinearLayout.LayoutParams.WrapContent);
            viewDataHeight.LayoutParameters = _params;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            //decide which button was clicked
            if ((sender as Button) == bDeleteData)
            {
                Popup.Double("Alert", "Are you sure you want to delete the compiled data for '" + currentCompiled.officialName + "'", "Yes", "CANCEL", this, ifYes);
                void ifYes()
                {
                    eData.DeleteCompiledEventData(currentCompiled.cID);
                    Finish();
                    StartActivity(typeof(SelectEventCompiled));
                }
            }
        }

        private void gridClicked(object sender, ItemClickEventArgs e)
        {
            //make sure a team number on the left was selected
            if (e.Position % 8 == 0 && e.Position != 0)
            {
                CompiledTeamIndex currentIndex = new CompiledTeamIndex((e.Position / 8) - 1);
                //set where the team is in the compiled event data multidimensional lsit
                eData.setTeamIndex(currentIndex);
                StartActivity(typeof(DetailedTeamData));
            }
            else
            {
                Popup.Single("Alert", "Please click a team number to view detailed data", "OK", this);
            }
        }
    }
}