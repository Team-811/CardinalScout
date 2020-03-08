﻿using Android.App;
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
    /*This activity assigns the current device as a slave and generates the QR codes needed
     * to send data to the master device. It splits the QR code into 2 equal parts to make reading
     * it easier for low-quality cameras*/

    [Activity(Label = "SlaveView", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SlaveView: Activity
    {
        //declare objects for controls
        private ImageView QR1;

        private ImageView QR2;
        private Button bGenerate;
        private Spinner selectEvent;

        //get databse instance
        private EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Slave_View);
            //get controls from layout and assign event handlers
            QR1 = FindViewById<ImageView>(Resource.Id.imgQR1);
            QR2 = FindViewById<ImageView>(Resource.Id.imgQR2);
            bGenerate = FindViewById<Button>(Resource.Id.bGenerateQR);
            bGenerate.Click += ButtonClicked;
            selectEvent = FindViewById<Spinner>(Resource.Id.sendDataChooser);
            selectEvent.ItemSelected += SpinnerClick;
            //put events in the dropdown
            ArrayAdapter selectAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, eData.GetEventDisplayList());
            selectEvent.Adapter = selectAdapt;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            if ((sender as Button) == bGenerate)
            {
                try
                {
                    string QRdata1 = null;
                    string QRdata2 = null;
                    //get the MatchData for the selected event
                    List<MatchData> scoutList = eData.GetMatchDataForEvent(selectedEvent.eventID);
                    for (int i = 0; i < scoutList.Count; i++)
                    {
                        //split data into two
                        if (i < Math.Round((double)scoutList.Count / 2))
                        {
                            QRdata1 += scoutList[i].teamNumber.ToString() + "," +
                              scoutList[i].matchNumber.ToString() + "," +
                              scoutList[i].result.ToString() +
                              scoutList[i].position.ToString() +
                              Convert.ToByte(scoutList[i].isTable).ToString() +
                              Convert.ToByte(scoutList[i].initiationCrossed).ToString() +
                              scoutList[i].auto.ToString() +
                              Convert.ToByte(scoutList[i].shoot).ToString() +
                              Convert.ToByte(scoutList[i].shootOuter).ToString() +
                              Convert.ToByte(scoutList[i].shootInner).ToString() +
                              Convert.ToByte(scoutList[i].shootLower).ToString() +
                              Convert.ToByte(scoutList[i].shootWell).ToString() +
                              Convert.ToByte(scoutList[i].shootBarely).ToString() +
                              Convert.ToByte(scoutList[i].shootTrench).ToString() +
                              Convert.ToByte(scoutList[i].shootLine).ToString() +
                              Convert.ToByte(scoutList[i].shootPort).ToString() +
                              Convert.ToByte(scoutList[i].climb).ToString() +
                              Convert.ToByte(scoutList[i].adjustClimb).ToString() +
                              Convert.ToByte(scoutList[i].wheel).ToString() +
                              Convert.ToByte(scoutList[i].rotationControl).ToString() +
                              Convert.ToByte(scoutList[i].positionControl).ToString() +
                              Convert.ToByte(scoutList[i].underTrench).ToString() +
                              Convert.ToByte(scoutList[i].goodDrivers).ToString() +
                              scoutList[i].wouldRecommend.ToString();
                        }
                        else
                        {
                            QRdata2 += scoutList[i].teamNumber.ToString() + "," +
                              scoutList[i].matchNumber.ToString() + "," +
                              scoutList[i].result.ToString() +
                              scoutList[i].position.ToString() +
                              Convert.ToByte(scoutList[i].isTable).ToString() +
                              Convert.ToByte(scoutList[i].initiationCrossed).ToString() +
                              scoutList[i].auto.ToString() +
                              Convert.ToByte(scoutList[i].shoot).ToString() +
                              Convert.ToByte(scoutList[i].shootOuter).ToString() +
                              Convert.ToByte(scoutList[i].shootInner).ToString() +
                              Convert.ToByte(scoutList[i].shootLower).ToString() +
                              Convert.ToByte(scoutList[i].shootWell).ToString() +
                              Convert.ToByte(scoutList[i].shootBarely).ToString() +
                              Convert.ToByte(scoutList[i].shootTrench).ToString() +
                              Convert.ToByte(scoutList[i].shootLine).ToString() +
                              Convert.ToByte(scoutList[i].shootPort).ToString() +
                              Convert.ToByte(scoutList[i].climb).ToString() +
                              Convert.ToByte(scoutList[i].adjustClimb).ToString() +
                              Convert.ToByte(scoutList[i].wheel).ToString() +
                              Convert.ToByte(scoutList[i].rotationControl).ToString() +
                              Convert.ToByte(scoutList[i].positionControl).ToString() +
                              Convert.ToByte(scoutList[i].underTrench).ToString() +
                              Convert.ToByte(scoutList[i].goodDrivers).ToString() +
                              scoutList[i].wouldRecommend.ToString();
                        }
                    }
                    //get QR code writer
                    Writer writer = new QRCodeWriter();
                    int width = this.Resources.DisplayMetrics.WidthPixels;
                    //create the QR codes
                    try
                    {
                        BitMatrix bm1 = writer.encode(QRdata1, BarcodeFormat.QR_CODE, width, width);
                        BitmapRenderer bit1 = new BitmapRenderer();
                        QR1.SetImageBitmap(bit1.Render(bm1, BarcodeFormat.QR_CODE, QRdata1));
                        Popup.Single("Alert", "Data for Event: '" + selectedEvent.eventName + "' Event ID: " +
                            selectedEvent.eventID.ToString() + " Generated. Please make sure that the receiving " +
                            "device has the correct event selected and that the event ids match", "OK", this);
                    }
                    //exception thrown if QR data strings are empty
                    catch
                    {
                        Popup.Single("Alert", "No data for this match", "OK", this);
                    }
                    //try to generate a second QR code
                    try
                    {
                        BitMatrix bm2 = writer.encode(QRdata2, BarcodeFormat.QR_CODE, width, width);
                        BitmapRenderer bit2 = new BitmapRenderer();
                        QR2.SetImageBitmap(bit2.Render(bm2, BarcodeFormat.QR_CODE, QRdata2));
                    }
                    //do nothing if there is not enough data for a second QR code
                    catch
                    {
                    }
                }
                //if no event is selected, it throws an exception
                catch
                {
                    Popup.Single("Alert", "Please select an event to generate data for", "OK", this);
                }
            }
        }

        //placeholders for dropdown index and selected event
        private int spinnerIndex;

        private Event selectedEvent;

        private void SpinnerClick(object sender, ItemSelectedEventArgs e)
        {
            spinnerIndex = e.Position;
            selectedEvent = eData.GetEvent(eData.EventIDList()[spinnerIndex]);
            //clear QR codes from previous events
            QR1.SetImageBitmap(null);
            QR2.SetImageBitmap(null);
            Popup.Single("Alert", "Selected event Changed. Please click generate again to update the data", "OK", this);
        }
    }
}