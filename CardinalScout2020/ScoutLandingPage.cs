using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace CardinalScout2020
{
    /// <summary>
    /// This activity displays a list of recently scouted matches. A match can be selected to view more details. It also allows a user to scout a new match.
    /// </summary>
    [Activity(Label = "scoutLandingPage", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ScoutLandingPage: Activity
    {
        //Declare objects for controls.
        private Button bAddTeam;
        private ListView recentMatches;
        private TextView textTitle;
        private Button bRefresh;
        private Button bViewEvent;

        //Placeholder for current event being scouted for.
        private Event currentEvent;        

        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set the correct screen.
            SetContentView(Resource.Layout.Scout_Landing_Page);

            //Get controls from layout and assign event handlers.
            bAddTeam = FindViewById<Button>(Resource.Id.bAddTeam);
            bAddTeam.Click += ButtonClicked;
            recentMatches = FindViewById<ListView>(Resource.Id.recentMatches);
            recentMatches.ItemClick += ListViewClick;
            textTitle = FindViewById<TextView>(Resource.Id.textTitle);
            bRefresh = FindViewById<Button>(Resource.Id.bRefreshMatches);
            bRefresh.Click += ButtonClicked;
            bViewEvent = FindViewById<Button>(Resource.Id.bViewData);
            bViewEvent.Click += ButtonClicked;
            
            //Get the current event from the database.
            currentEvent = ScoutDatabase.GetCurrentEvent();

            //Display recent matches in ListView.
            var matchAdapter = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, ScoutDatabase.GetMatchDisplayList(currentEvent.EventID));
            recentMatches.Adapter = matchAdapter;

            //Set title text based on current event.
            textTitle.Text += currentEvent.EventName + "'";
        }

        //Handle button clicks.
        private void ButtonClicked(object sender, EventArgs e)
        {
            //Decide which button was clicked
            if ((sender as Button) == bRefresh)
            {
                Recreate();
            }
            else if ((sender as Button) == bAddTeam)
            {
                //Each event can have a max of 80 matches; more makes reading QR codes difficult.
                if (ScoutDatabase.GetMatchDataForEvent(currentEvent.EventID).Count > 79)
                {
                    Popup.Single("Alert", "Max 80 matches per event reached", "OK", this);
                }
                //If ok, go to scout form.
                else
                {
                    StartActivity(typeof(ScoutForm));
                    Finish();
                }
            }
            else if ((sender as Button) == bViewEvent)
            {
                try
                {
                    //Set the current match to view to the selected match in the list.
                    ScoutDatabase.SetCurrentMatch(selectedMatch.ID);
                    StartActivity(typeof(ViewMatchData));
                }
                //If no event is selected, it throws an exception.
                catch
                {
                    Popup.Single("Alert", "Please select a match to view", "OK", this);
                }
            }
        }

        //Handle selecting matches in the ListView.
        private int selectedIndex;
        private MatchData selectedMatch;
        private void ListViewClick(object sender, ItemClickEventArgs e)
        {
            selectedIndex = e.Position;
            selectedMatch = ScoutDatabase.GetMatchDataFromIndex(currentEvent, selectedIndex);

            //Change button text to reflect selected event.
            bViewEvent.Text = "Details for Match " + selectedMatch.MatchNumber;
        }
    }
}