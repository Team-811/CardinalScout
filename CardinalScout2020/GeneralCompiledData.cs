using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using static Android.Widget.AdapterView;

namespace CardinalScout2020
{
    /// <summary>
    /// This activity displays general compiled data for an event. It displays the teams involved in the event and their overall percentages based off of performance in matches.
    /// </summary>
    [Activity(Label = "GeneralCompiledData", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class GeneralCompiledData: Activity
    {
        //Declare objects for controls.
        private TextView textRecent;
        private string[] properties;
        private Button bDelete;
        private GridView gridTeams;
        private LinearLayout viewDataHeight;

        //Placeholder for current compiled event data to view.
        private CompiledEventData currentCompiled;        

        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set the correct screen.
            SetContentView(Resource.Layout.General_Compiled);
             
            //Get controls from layout and assign event handlers.
            bDelete = FindViewById<Button>(Resource.Id.bDeleteData);
            bDelete.Click += ButtonClicked;
            gridTeams = FindViewById<GridView>(Resource.Id.gridViewTeam);
            gridTeams.ItemClick += GridClicked;
            textRecent = FindViewById<TextView>(Resource.Id.textEvent);
            viewDataHeight = FindViewById<LinearLayout>(Resource.Id.viewDataHeight);
            
            //Get current compiled data.
            currentCompiled = ScoutDatabase.GetCurrentCompiled();

            //Get data for title text.
            SpannableString[] textDisp = new SpannableString[]
            {
                FormatString.SetNormal("Viewing Data for Event: \n"),
                FormatString.SetBold("'"+currentCompiled.OfficialName+"'"),
                FormatString.SetNormal(" as of "),
                FormatString.SetBold(currentCompiled.DateMod),
                FormatString.SetNormal(" at "),
                FormatString.SetBold(currentCompiled.TimeMod)
            };

            //Set title display text.
            textRecent.TextFormatted = TextUtils.ConcatFormatted(textDisp);
            
            //Column titles.
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

            //Get general formatted data for all teams.
            List<SpannableString> teamNumbers = currentCompiled.GetTeamListFormatted();
            List<SpannableString> recPerc = currentCompiled.GetRecPercentListFormatted();
            List<SpannableString> record = currentCompiled.GetRecordList();
            List<SpannableString> shootPerc = currentCompiled.GetShootListFormatted();
            List<SpannableString> prefPort = currentCompiled.GetPrimaryPortListFormatted();
            List<SpannableString> climbPerc = currentCompiled.GetClimbListFormatted();
            List<SpannableString> climbAdj = currentCompiled.GetAdjustListFormatted();
            List<SpannableString> cWheel = currentCompiled.GetWheelListFormatted();
            List<SpannableString> driversPerc = currentCompiled.GetDriversListFormatted();
            List<SpannableString> tablePerc = currentCompiled.GetTableListFormatted();            

            //Placeholder for all of the data to display.
            List<SpannableString> display = new List<SpannableString>();

            //Add the properties to the top of the display.
            for (int i = 0; i < properties.Length; i++)
            {
                display.Add(FormatString.SetBold(properties[i]));
            }

            //Slash that will be used between certain properties.
            SpannableString slash = FormatString.SetNormal(" / ");

            //Add the rest of the data in the correct order.           
            for (int i = 0; i < teamNumbers.Count; i++)
            {                
                display.Add(teamNumbers[i]);
                display.Add(recPerc[i]);
                display.Add(record[i]);

                //Format shooter data.
                SpannableStringBuilder shootData = new SpannableStringBuilder(shootPerc[i]);
                shootData.Append(slash);
                shootData.Append(prefPort[i]);
                display.Add(new SpannableString(shootData));

                //Format climber data.
                SpannableStringBuilder climbData = new SpannableStringBuilder(climbPerc[i]);
                climbData.Append(slash);
                climbData.Append(climbAdj[i]);
                display.Add(new SpannableString(climbData));

                display.Add(cWheel[i]);
                display.Add(driversPerc[i]);
                display.Add(tablePerc[i]);              
                
            }

            //Display the data in the grid.
            ArrayAdapter gridAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridTeams.Adapter = gridAdapt;

            //Determine the width of the grid.
            float scale = Resources.DisplayMetrics.Density;
            FrameLayout.LayoutParams _params = new FrameLayout.LayoutParams((int)(1200 * scale), Android.Views.ViewGroup.LayoutParams.WrapContent);

            viewDataHeight.LayoutParameters = _params;
        }

        //Handle button clicks.
        private void ButtonClicked(object sender, EventArgs e)
        {
            //Decide which button was clicked.
            if ((sender as Button) == bDelete)
            {
                Popup.Double("Alert", "Are you sure you want to delete the compiled data for '" + currentCompiled.OfficialName + "'", "Yes", "CANCEL", this, ifYes);
                void ifYes()
                {
                    ScoutDatabase.DeleteCompiledEventData(currentCompiled.ID);
                    Finish();
                    StartActivity(typeof(SelectEventCompiled));
                }
            }
        }

        //Handle if a user selects a team from the grid.
        private void GridClicked(object sender, ItemClickEventArgs e)
        {
            //Make sure a team number on the left was selected.
            if (e.Position % 8 == 0 && e.Position != 0)
            {
                int index = e.Position / 8 - 1;
                int team = currentCompiled.GetTeamList().ToList()[index];
                
                //Set the current team for the CompiledEventData.
                ScoutDatabase.SetCurrentCompiledTeam(team);
                StartActivity(typeof(DetailedTeamData));
            }
            else
            {
                Popup.Single("Alert", "Please click a team number to view detailed data", "OK", this);
            }
        }
    }
}