using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;
using ZXing;
using ZXing.Mobile;
using static Android.Widget.AdapterView;

namespace CardinalScout2020
{
    /// <summary>
    /// This activity assigns the current device as "master" and collects data from the other 5 devices by scanning their generated QR codes. After gathering the data, it creates a new instance of the CompiledEventData class which contains the raw text from the QR codes and the data stored on the master device itself.
    /// </summary>
    [Activity(Label = "MasterView", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MasterView: Activity
    {
        //Declare objects for controls.
        private Spinner receiveDataSpinner;
        private Button b1p1;
        private Button b1p2;
        private Button b2p1;
        private Button b2p2;
        private Button b3p1;
        private Button b3p2;
        private Button b4p1;
        private Button b4p2;
        private Button b5p1;
        private Button b5p2;
        private Button bCompile;       

        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set the correct screen.
            SetContentView(Resource.Layout.Master_View);

            //Get controls and assign event handlers.
            b1p1 = FindViewById<Button>(Resource.Id.b1p1);
            b1p1.Click += ButtonClicked;
            b1p2 = FindViewById<Button>(Resource.Id.b1p2);
            b1p2.Click += ButtonClicked;
            b2p1 = FindViewById<Button>(Resource.Id.b2p1);
            b2p1.Click += ButtonClicked;
            b2p2 = FindViewById<Button>(Resource.Id.b2p2);
            b2p2.Click += ButtonClicked;
            b3p1 = FindViewById<Button>(Resource.Id.b3p1);
            b3p1.Click += ButtonClicked;
            b3p2 = FindViewById<Button>(Resource.Id.b3p2);
            b3p2.Click += ButtonClicked;
            b4p1 = FindViewById<Button>(Resource.Id.b4p1);
            b4p1.Click += ButtonClicked;
            b4p2 = FindViewById<Button>(Resource.Id.b4p2);
            b4p2.Click += ButtonClicked;
            b5p1 = FindViewById<Button>(Resource.Id.b5p1);
            b5p1.Click += ButtonClicked;
            b5p2 = FindViewById<Button>(Resource.Id.b5p2);
            b5p2.Click += ButtonClicked;
            bCompile = FindViewById<Button>(Resource.Id.bCompile2);
            bCompile.Click += CompileData;
            receiveDataSpinner = FindViewById<Spinner>(Resource.Id.receiveDataSpinner);
            receiveDataSpinner.ItemSelected += SpinnerClick;

            //Create an adapter for the drop down picker with event names to choose from.
            ArrayAdapter selectAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, ScoutDatabase.GetEventDisplayList());
            receiveDataSpinner.Adapter = selectAdapt;

            //Initialize QR scanner class
            MobileBarcodeScanner.Initialize(Application);
        }

        //Placeholder button.
        private Button clickedButton;
        //Handle button clicks.
        private async void ButtonClicked(object sender, EventArgs e)
        {
            //Remember which button was pressed.
            clickedButton = sender as Button;

            //Set up QR scanner.
            var scanner = new MobileBarcodeScanner();
            var opt = new MobileBarcodeScanningOptions
            {
                //Make sure the scanner only looks for QR codes.
                PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }
            };
            scanner.TopText = "Hold the camera up to the QR Code";
            scanner.BottomText = "Wait for the QR Code to automatically scan!";

            //This will start scanning.
            ZXing.Result result = await scanner.Scan();

            //Show the result returned.
            HandleResult(result);
        }

        //Create a string to store collected values.
        private string concatedQR = null;

        //Handle the scanned QR and add it to the collected data.
        private void HandleResult(ZXing.Result result)
        {
            //Default message
            var msg = "Failed, Please try again";

            //Make sure the scanner detected a valid QR code
            if (result != null && result.BarcodeFormat == BarcodeFormat.QR_CODE)
            {
                clickedButton.Text = "Success";
                clickedButton.SetBackgroundColor(Color.Rgb(121, 234, 144));
                
                //Add scanned result to the combined result string
                concatedQR += result.Text;
                msg = result.Text;
            }           
        }
    
        //Create a new CompiledEventData based on the scanned data.
        private void CompileData(object sender, EventArgs e)
        {
            try
            {
                bool isDuplicate = false;
                //Get a list of already existing CompiledEventData items and make sure there aren't any duplicates.        
                foreach (CompiledEventData q in ScoutDatabase.GetCompiledEventDataList())
                {
                    if (q.ID == selectedEvent.EventID)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                //Add data from the master device.
                List<MatchData> scoutList = ScoutDatabase.GetMatchDataForEvent(selectedEvent.EventID);
                for (int i = 0; i < scoutList.Count; i++)
                {
                    concatedQR += scoutList[i].TeamNumber.ToString() + "," +
                        scoutList[i].MatchNumber.ToString() + "," +
                        scoutList[i].Result.ToString() +
                        scoutList[i].DSPosition.ToString() +
                        Convert.ToByte(scoutList[i].IsTable).ToString() +
                        Convert.ToByte(scoutList[i].InitiationCrossed).ToString() +
                        scoutList[i].AutoResult.ToString() +
                        Convert.ToByte(scoutList[i].Shoot).ToString() +
                        Convert.ToByte(scoutList[i].ShootOuter).ToString() +
                        Convert.ToByte(scoutList[i].ShootInner).ToString() +
                        Convert.ToByte(scoutList[i].ShootLower).ToString() +                       
                        Convert.ToByte(scoutList[i].ShootTrench).ToString() +
                        Convert.ToByte(scoutList[i].ShootInitiation).ToString() +
                        Convert.ToByte(scoutList[i].ShootPort).ToString() +
                        Convert.ToByte(scoutList[i].Climb).ToString() +
                        Convert.ToByte(scoutList[i].AdjustClimb).ToString() +
                        Convert.ToByte(scoutList[i].Wheel).ToString() +
                        Convert.ToByte(scoutList[i].RotationControl).ToString() +
                        Convert.ToByte(scoutList[i].PositionControl).ToString() +
                        Convert.ToByte(scoutList[i].UnderTrench).ToString() +
                        Convert.ToByte(scoutList[i].GoodDrivers).ToString() +
                        scoutList[i].WouldRecommend.ToString();
                }

                //Make sure there is some data.
                if (concatedQR != null)
                {
                    //Make sure it isn't a duplicate.
                    if (!isDuplicate)
                    {
                        //Create a new compiled event data and add it to the database.                        
                        ScoutDatabase.AddCompiledEventData(new CompiledEventData(selectedEvent.EventName, selectedEvent.StartDate, selectedEvent.EndDate, concatedQR, selectedEvent.EventID));
                        Popup.Single("Alert", "Successfully generated data for event '" + selectedEvent.EventName + "'.", "OK", this);                        
                    }
                    //If it is a duplicate.
                    else
                    {
                        Popup.Single("Alert", "Data for this event has already been generated on this device. " +
                            "Please delete it in 'View Data' from the home screen first if you want to generate new data", "OK", this);
                        //Reset the data.
                        concatedQR = null;
                    }
                }
                //If the QR data is completely blank.
                else
                {
                    Popup.Single("Alert", "No data collected, please start over", "OK", this);                    
                    concatedQR = null;
                }
            }
            //If no event has been selected.
            catch
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog missingDetails = dialog.Create();
                missingDetails.SetTitle("Alert");
                missingDetails.SetMessage("No event selected");
                ;
                missingDetails.SetButton("OK", (c, ev) =>
                {
                });
                missingDetails.Show();
            }
        }

        //Determine which event was selected from and put it in the current event placeholder to be able to collect data from it.
        private int spinnerIndex;
        private Event selectedEvent;

        private void SpinnerClick(object sender, ItemSelectedEventArgs e)
        {
            spinnerIndex = e.Position;
            selectedEvent = ScoutDatabase.GetEventFromIndex(spinnerIndex);
        }
    }
}