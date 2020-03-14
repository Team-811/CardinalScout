using Android.App;
using Android.OS;
using Android.Widget;
using System;

namespace CardinalScout2020
{
    /// <summary>
    /// This activity contains instructions for using the app and an option to factory reset it, which deletes the entire database. It also has a button to edit existing events to delete individual events or change event ids.
    /// </summary>
    [Activity(Label = "InstructSettings", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Settings: Activity
    {
        //Declare objects for controls.
        private Button bDelete;
        private Button bEditEvent;

        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set the correct screen.
            SetContentView(Resource.Layout.Instructions_Settings);

            //Get controls from layout and assign event handlers.
            bDelete = FindViewById<Button>(Resource.Id.bDelete);
            bDelete.Click += ButtonClicked;
            bEditEvent = FindViewById<Button>(Resource.Id.bEditEvent);
            bEditEvent.Click += ButtonClicked;
        }

        //Handle button clicks.
        private void ButtonClicked(object sender, EventArgs e)
        {
            //Decide which button was pressed.
            if ((sender as Button) == bDelete)
            {
                //Double check to make sure the user wants to delete.
                Popup.Double("WARNING", "Are you SURE you want to delete the ENTIRE database (all events, matches, files, etc?", "Yes", "CANCEL", this, yes1);
                void yes1()
                {
                    Popup.Double("WARNING", "Are you ABSOLUTELY sure?", "Yes, I'm sure", "CANCEL", this, yes2);
                    void yes2()
                    {
                        //Delete database and go back to the main page.
                        DeleteDatabase(SQLite_android.GetDatabasePath());
                        StartActivity(typeof(MainActivity));
                        Finish();
                    }
                }
            }
            //Go to event editing page.
            else if ((sender as Button) == bEditEvent)
            {
                StartActivity(typeof(EditEvents));
            }
        }
    }
}