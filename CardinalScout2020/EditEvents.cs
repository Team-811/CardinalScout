using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace CardinalScout2020
{
    /// <summary>
    /// This activity is started when "Edit Events" is clicked in instructions/settings. It lists events and allows the user to edit an incorrect event id or delete an event and its associated matches.
    /// </summary>
    [Activity(Label = "editevents", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class EditEvents: Activity
    {
        //Declare objects that will refer to controls.
        private ListView eventList;
        private Button bDeleteEvent;
        private Button bEditID;
        private Button bRefresh;

        //Selected Index of the ListView.
        private int selectedIndex;

        //Placeholder for selected event.
        private Event selectedEvent;

        /// <summary>
        /// Initialize the activity
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set to the correct screen.
            SetContentView(Resource.Layout.Edit_Event);

            //Get controls/assign event handlers.
            eventList = FindViewById<ListView>(Resource.Id.eventList);
            eventList.ItemClick += ListViewClick;
            bDeleteEvent = FindViewById<Button>(Resource.Id.bDeleteEvent);
            bDeleteEvent.Click += ButtonClicked;
            bEditID = FindViewById<Button>(Resource.Id.bEditID);
            bEditID.Click += ButtonClicked;
            bRefresh = FindViewById<Button>(Resource.Id.bRefreshEvents);
            bRefresh.Click += ButtonClicked;

            //Use an ArrayAdapter to convert the list of events to ListView format.
            var eventAdapter = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, ScoutDatabase.GetEventDisplayList());
            eventList.Adapter = eventAdapter;
        }

        //Handle button clicks.
        private void ButtonClicked(object sender, EventArgs e)
        {
            try
            {
                //Decide which button was pressed.
                if ((sender as Button) == bDeleteEvent)
                {
                    Popup.Double("Alert", "Are you sure you want to delete the event '" +
                        selectedEvent.EventName + "' AND all associated matches?", "Yes", "CANCEL", this, Delete);
                    //If user presses delete.
                    void Delete()
                    {
                        ScoutDatabase.DeleteEvent(selectedEvent.EventID);                        
                        Popup.Single("Alert", "Event Deleted", "OK", this);                        
                    }
                }
                else if ((sender as Button) == bEditID)
                {
                    //Set current event to the one selected in the list so it can be accessed by the next activity.
                    ScoutDatabase.SetCurrentEvent(selectedEvent.EventID);
                    Finish();
                    //Go to the event id editing screen.
                    StartActivity(typeof(EventID));
                }
                else if ((sender as Button) == bRefresh)
                {
                    //Refresh
                    Recreate();
                }
            }
            //If no event is selected, selected event will be null and throw an exception.
            catch
            {
                Popup.Single("Alert", "Please select an event to edit", "OK", this);
            }
        }

        //Handle when an event in the list is selected.
        private void ListViewClick(object sender, ItemClickEventArgs e)
        {
            selectedIndex = e.Position;
            //Determine the event selected.
            selectedEvent = ScoutDatabase.GetEventFromIndex(selectedIndex);
        }
    }
}