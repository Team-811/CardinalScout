using Android.App;
using Android.OS;
using Android.Widget;
using System;

namespace CardinalScout2020
{
    /// <summary>
    /// This activity is used to reassign event IDs to events if for whatever reason the IDs don't match across the 6 devices used for scouting.
    /// </summary>
    [Activity(Label = "EventID", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class EventID: Activity
    {
        //Declare objects for controls.
        private Button bConfirm;
        private EditText newID;
        private TextView title;

        //Placeholder for the event to be edited.
        private Event currentEvent;       

        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Go to the event id editing screen.
            SetContentView(Resource.Layout.Event_ID);

            //Get controls from layout and assign event handlers.
            bConfirm = FindViewById<Button>(Resource.Id.bConfirmID);
            bConfirm.Click += ButtonClicked;
            newID = FindViewById<EditText>(Resource.Id.textID);
            title = FindViewById<TextView>(Resource.Id.titleID);

            //Get the event that was selected to be edited.
            currentEvent = ScoutDatabase.GetCurrentEvent();

            //Display current event name at the top.
            title.Text += "'" + currentEvent.EventName + "'";
        }

        //Handle button click.
        private void ButtonClicked(object sender, EventArgs e)
        {
            //Decide which button was clicked.
            if ((sender as Button) == bConfirm)
            {
                try
                {
                    //Change the id based on entered value and exit the activity.
                    ScoutDatabase.ChangeEventID(currentEvent.EventID, int.Parse(newID.Text));
                    Finish();
                }
                //If the database has a duplicate id, it will throw an exception.
                catch
                {
                    Popup.Single("Alert", "Please enter a new ID not used by an existing event", "OK", this);
                }
            }
        }
    }
}