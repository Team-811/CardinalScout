using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;

namespace CardinalScout2020
{
    /// <summary>
    /// This is the home page for the app. It shows up when the app is launched and contains buttons to navigate to other screens.
    /// </summary>
    [Activity(Label = "Cardinal Scout", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity: AppCompatActivity
    {
        //Declare objects to refer to controls.
        private Button bViewPrev;
        private Button bSync;
        private Button bNewEvent;
        private Button bContinue;
        private Button bSettings;

        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            ScoutDatabase.Initialize();

            //UNCOMMENT THIS TO RESET APP AND DELETE DATABASE ON LAUNCH
            //DeleteDatabase(SQLite_android.GetDatabasePath());

            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Activity_Main);

            //Get controls and assign event handlers.
            bViewPrev = FindViewById<Button>(Resource.Id.bViewPrev);
            bViewPrev.Click += ButtonClicked;
            bSync = FindViewById<Button>(Resource.Id.bSync);
            bSync.Click += ButtonClicked;
            bNewEvent = FindViewById<Button>(Resource.Id.bNewEvent);
            bNewEvent.Click += ButtonClicked;
            bContinue = FindViewById<Button>(Resource.Id.bContinue);
            bContinue.Click += ButtonClicked;
            bSettings = FindViewById<Button>(Resource.Id.bSettings);
            bSettings.Click += ButtonClicked;
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
        }

        //Handle button clicks.
        private void ButtonClicked(object sender, EventArgs e)
        {
            //Decide which button was pressed and start the appropriate activity.
            if ((sender as Button) == bViewPrev)
            {
                StartActivity(typeof(SelectEventCompiled));
            }
            else if ((sender as Button) == bSync)
            {
                StartActivity(typeof(MasterSlaveSelect));
            }
            else if ((sender as Button) == bNewEvent)
            {
                StartActivity(typeof(CreateEvent));
            }
            else if ((sender as Button) == bSettings)
            {
                StartActivity(typeof(Settings));
            }
            else if ((sender as Button) == bContinue)
            {
                StartActivity(typeof(SelectEventScout));
            }
        }
    }
}