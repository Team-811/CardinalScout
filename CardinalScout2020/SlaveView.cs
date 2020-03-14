using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;
using ZXing;
using ZXing.Common;
using ZXing.Mobile;
using ZXing.QrCode;
using static Android.Widget.AdapterView;

namespace CardinalScout2020
{
    /// <summary>
    /// This activity assigns the current device as a slave and generates the QR codes needed to send data to the master device. It splits the QR code into 2 equal parts to make reading it easier for low-quality cameras.
    /// </summary>
    [Activity(Label = "SlaveView", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SlaveView: Activity
    {
        //Declare objects for controls.
        private ImageView QR1;
        private ImageView QR2;
        private Button bGenerate;
        private Spinner selectEvent;        

        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set correct screen.
            SetContentView(Resource.Layout.Slave_View);

            //Get controls from layout and assign event handlers.
            QR1 = FindViewById<ImageView>(Resource.Id.imgQR1);
            QR2 = FindViewById<ImageView>(Resource.Id.imgQR2);
            bGenerate = FindViewById<Button>(Resource.Id.bGenerateQR);
            bGenerate.Click += ButtonClicked;
            selectEvent = FindViewById<Spinner>(Resource.Id.sendDataChooser);
            selectEvent.ItemSelected += SpinnerClick;

            //Put events in the dropdown.
            ArrayAdapter selectAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, ScoutDatabase.GetEventDisplayList());
            selectEvent.Adapter = selectAdapt;
        }

        //Handle button clicks.
        private void ButtonClicked(object sender, EventArgs e)
        {
            if ((sender as Button) == bGenerate)
            {
                //Try to generate the QR codes.
                try
                {
                    string QRdata1 = null;
                    string QRdata2 = null;

                    //Get the MatchData for the selected event.
                    List<MatchData> scoutList = ScoutDatabase.GetMatchDataForEvent(selectedEvent.EventID);

                    //Add each match to the codes.
                    for (int i = 0; i < scoutList.Count; i++)
                    {
                        //Split data into two.
                        if (i % 2 == 0)
                        {
                            QRdata1 += scoutList[i].TeamNumber.ToString() + "," +
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
                        else
                        {
                            QRdata2 += scoutList[i].TeamNumber.ToString() + "," +
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
                    }

                    //Get QR code writer.
                    Writer writer = new QRCodeWriter();
                    int width = Resources.DisplayMetrics.WidthPixels;
                    
                    //Create the QR codes.
                    try
                    {
                        BitMatrix bm1 = writer.encode(QRdata1, BarcodeFormat.QR_CODE, width, width);
                        BitmapRenderer bit1 = new BitmapRenderer();
                        QR1.SetImageBitmap(bit1.Render(bm1, BarcodeFormat.QR_CODE, QRdata1));
                        Popup.Single("Alert", "Data for Event: '" + selectedEvent.EventName + "' Event ID: " +
                            selectedEvent.EventID.ToString() + " Generated. Please make sure that the receiving " +
                            "device has the correct event selected and that the event ids match", "OK", this);
                    }
                    //Exception thrown if QR data strings are empty.
                    catch
                    {
                        Popup.Single("Alert", "No data for this match", "OK", this);
                    }
                    //Try to generate a second QR code if there is enough data.
                    try
                    {
                        BitMatrix bm2 = writer.encode(QRdata2, BarcodeFormat.QR_CODE, width, width);
                        BitmapRenderer bit2 = new BitmapRenderer();
                        QR2.SetImageBitmap(bit2.Render(bm2, BarcodeFormat.QR_CODE, QRdata2));
                    }
                    //Do nothing if there is not enough data for a second QR code.
                    catch
                    {
                    }
                }
                //If no event is selected, it throws an exception.
                catch
                {
                    Popup.Single("Alert", "Please select an event to generate data for", "OK", this);
                }
            }
        }

        //Placeholders for dropdown index and selected event.
        private int spinnerIndex;
        private Event selectedEvent;

        //Handle user selecting an event from dropdown.
        private void SpinnerClick(object sender, ItemSelectedEventArgs e)
        {
            spinnerIndex = e.Position;
            selectedEvent = ScoutDatabase.GetEventFromIndex(spinnerIndex);
            
            //Clear QR codes from previous events.
            QR1.SetImageBitmap(null);
            QR2.SetImageBitmap(null);
            Popup.Single("Alert", "Selected event Changed. Please click generate again to update the data", "OK", this);
        }
    }
}