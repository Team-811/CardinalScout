using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace CardinalScout2020
{
    /// <summary>
    /// This activity is for selecting which event to view compiled data for after clicking "View Data" on the home page.
    /// </summary>
    [Activity(Label = "SelectEventCompiled", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SelectEventCompiled: Activity
    {
        //Declare objects for controls.
        private ListView eventList;
        private Button bSelect;

        //Placeholder for selected event.
        private CompiledEventData selectedCompiled;
        
        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            //Set correct screen.
            SetContentView(Resource.Layout.Select_Compiled);

            //Get controls from layout and assign event handlers.
            eventList = FindViewById<ListView>(Resource.Id.choosetoView);
            eventList.ItemClick += ListViewClick;
            bSelect = FindViewById<Button>(Resource.Id.bSelectView);
            bSelect.Click += ButtonClicked;

            //Display available events with compiled data.
            var eventAdapter = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, ScoutDatabase.GetCompiledDisplayList());
            eventList.Adapter = eventAdapter;
        }

        //Handle button clicks.
        private void ButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if ((sender as Button) == bSelect)
                {
                    //Set current compiled event data to the selected one to be used in next activity.
                    ScoutDatabase.SetCurrentCompiled(selectedCompiled.ID);
                    Finish();
                    
                    //Go to general compiled data display page for event.
                    StartActivity(typeof(GeneralCompiledData));
                }
            }
            //If no event is selected, it throws an exception.
            catch
            {
                Popup.Single("Alert", "Please select an event to view data for", "OK", this);
            }
        }

        //Decide which event was clicked based off of the compiled id list.
        private int selectedIndex;

        private void ListViewClick(object sender, ItemClickEventArgs e)
        {
            selectedIndex = e.Position;
            selectedCompiled = ScoutDatabase.GetCompiledEventDataFromIndex(selectedIndex);
        }
    }
}