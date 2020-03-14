using Android.App;
using Android.OS;
using Android.Widget;
using System;

namespace CardinalScout2020
{
    /// <summary>
    /// This activity decides if a device will be sending or receiving during the data transfer process.
    /// </summary>
    [Activity(Label = "MasterSlaveSelect", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MasterSlaveSelect: Activity
    {
        //Declare objects for controls.
        private Button bMaster;
        private Button bSlave;

        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set the correct screen.
            SetContentView(Resource.Layout.Master_Slave_Select);

            //Get controls from layout and assign event handlers.
            bMaster = FindViewById<Button>(Resource.Id.bSetMaster);
            bMaster.Click += ButtonClicked;
            bSlave = FindViewById<Button>(Resource.Id.bSetSlave);
            bSlave.Click += ButtonClicked;
        }

        //Handle button clicks.
        private void ButtonClicked(object sender, EventArgs e)
        {
            //Decide which button was clicked and start appropriate activity.
            if ((sender as Button) == bMaster)
            {
                StartActivity(typeof(MasterView));
            }
            else if ((sender as Button) == bSlave)
            {
                StartActivity(typeof(SlaveView));
            }
        }
    }
}