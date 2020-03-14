using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace CardinalScout2020
{
    /// <summary>
    /// This activity allows the user to choose which event they would like to scout for.
    /// </summary>
    [Activity(Label = "SelectEventScout", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SelectEventScout: Activity
    {
        //Declare objects for controls.
        private ListView eventList;
        private Button bSelect;

        //Placeholder for selected event.
        private Event selectedEvent;

        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set correct screen.
            SetContentView(Resource.Layout.Select_Event);

            //Get controls from layout and assign event handlers.
            eventList = FindViewById<ListView>(Resource.Id.chooseList);
            eventList.ItemClick += ListViewClick;
            bSelect = FindViewById<Button>(Resource.Id.bSelect);
            bSelect.Click += ButtonClicked;

            //Get a display list of events and adapt them to a ListView.
            var eventAdapter = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, ScoutDatabase.GetEventDisplayList());
            eventList.Adapter = eventAdapter;
        }

        //Handle button clicks.
        private void ButtonClicked(object sender, EventArgs e)
        {
            try
            {
                //Decide which button was pressed
                if ((sender as Button) == bSelect)
                {
                    //Set the current event and go to the scouting page.
                    ScoutDatabase.SetCurrentEvent(selectedEvent.EventID);
                    StartActivity(typeof(ScoutLandingPage));
                }
            }
            //If no event is selected, it will throw an exception.
            catch
            {
                Popup.Single("Alert", "Please select an event to scout", "OK", this);
            }
        }        

        //Determine which event the user has selected.
        private int selectedIndex;
        private void ListViewClick(object sender, ItemClickEventArgs e)
        {
            selectedIndex = e.Position;
            //get the selected event based on the position in the event id list
            selectedEvent = ScoutDatabase.GetEventFromIndex(selectedIndex);
        }
    }
}