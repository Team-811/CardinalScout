using Android.App;
using Android.OS;
using Android.Widget;
using System;


namespace CardinalScout2020
{
    /// <summary>
    /// This activity is started when "Create Event" is clicked on the home screen. It gathers the data from user input and creates a new instance of the "Event" class with the properties given.
    /// </summary>
    [Activity(Label = "CreateEvent", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class CreateEvent: Activity
    {
        //Declare objects that will refer to controls.
        private Button bCreate;
        private DatePicker startDate;
        private DatePicker endDate;
        private EditText txtEventName;
        private EditText txtEventID;

        //Set initial values
        private string getStartDate = null;
        private string getEndDate = null;
        private string eventName = null;          

        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set the correct screen.
            SetContentView(Resource.Layout.Create_Event);

            //Get controls from layout and assign neccessary event handlers.
            bCreate = FindViewById<Button>(Resource.Id.bCreate);
            bCreate.Click += ButtonClicked;
            startDate = FindViewById<DatePicker>(Resource.Id.startDate);
            endDate = FindViewById<DatePicker>(Resource.Id.endDate);
            txtEventName = FindViewById<EditText>(Resource.Id.txtEventName);
            txtEventID = FindViewById<EditText>(Resource.Id.txtEventID);
        }

        //Handle the button click.
        private void ButtonClicked(object sender, EventArgs e)
        {
            //Decide which button on the form was clicked.
            if ((sender as Button) == bCreate)
            {
                //Convert DateTime format to a readable string.
                getStartDate = startDate.DateTime.ToString("MM/dd/yyyy");
                getEndDate = endDate.DateTime.ToString("MM/dd/yyyy");
                eventName = txtEventName.Text;
                try
                {
                    //Make sure user inputted an event name and id.
                    if (eventName != null && eventName != "" && txtEventID.Text != null && txtEventID.Text != "")
                    {
                        //Create new event and add it to the database.                        
                        ScoutDatabase.AddEvent(new Event(getStartDate, getEndDate, eventName, int.Parse(txtEventID.Text), false));

                        //Go back to the home screen and finish this activity.
                        StartActivity(typeof(MainActivity));
                        Finish();
                    }
                    else
                    {
                        Popup.Single("Alert", "Please Enter Event Details", "OK", this);
                    }
                }
                //If the database has a duplicate event id, it will throw an exception not allowing the new one to be added.
                catch
                {
                    Popup.Single("Alert", "Duplicate Event ID Detected.", "OK", this);
                }
            }
        }
    }
}