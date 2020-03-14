﻿using Android.App;
using Android.OS;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace CardinalScout2020
{
    /// <summary>
    /// This activity allows a user to edit previously inputted match data by using the scouting form.
    /// </summary>
    [Activity(Label = "ScoutFormEdit", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    internal class ScoutFormEdit: Activity
    {
        /* Declare objects that will refer to the controls on the scouting form. */

        private TextView textTitle;
        private EditText textMatchNumber;
        private EditText textTeamNumber;
        private Spinner spinnerPosition;
        private CheckBox table;
        private CheckBox initiationLine;
        private RadioGroup radioAuto;
        private CheckBox shoot;
        private CheckBox outer;
        private CheckBox inner;
        private CheckBox lower;        
        private CheckBox shootTrench;
        private CheckBox shootInitiation;
        private CheckBox shootPort;
        private CheckBox climb;
        private CheckBox readjust;
        private CheckBox wheel;
        private CheckBox rotation;
        private CheckBox position;
        private CheckBox under;
        private RadioGroup radioDrivers;
        private RadioGroup radioRecommend;
        private RadioGroup radioResult;
        private MultiAutoCompleteTextView textComments;
        private Button bFinish;

        //Initialize radio button fields to false before changing them later.
        private int auto = 0;
        private bool goodDrivers = false;
        private int wouldRecommend = 1;
        private int result = 1;

        //Placeholder for the event that is being scouted.
        private Event currentEvent;

        //Placeholder for the MatchData which will be generated by the scouter.
        private MatchData matchData;

        //Placeholder for the existing MatchData.
        private MatchData currentMatch;        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Scout_Form);

            /* Assign the controls from the form and also assign any needed event handlers */

            textTitle = FindViewById<TextView>(Resource.Id.textEventTitle);
            textMatchNumber = FindViewById<EditText>(Resource.Id.vMatchNumber);
            textTeamNumber = FindViewById<EditText>(Resource.Id.vTeamNumber);
            spinnerPosition = FindViewById<Spinner>(Resource.Id.vPosition);
            spinnerPosition.ItemSelected += SpinnerClick;
            table = FindViewById<CheckBox>(Resource.Id.table);
            initiationLine = FindViewById<CheckBox>(Resource.Id.initiation);
            radioAuto = FindViewById<RadioGroup>(Resource.Id.radioAuto);
            radioAuto.CheckedChange += RadioClicked;
            shoot = FindViewById<CheckBox>(Resource.Id.shoot);
            shoot.CheckedChange += CheckboxClicked;
            outer = FindViewById<CheckBox>(Resource.Id.outer);
            outer.CheckedChange += CheckboxClicked;
            inner = FindViewById<CheckBox>(Resource.Id.inner);
            inner.CheckedChange += CheckboxClicked;
            lower = FindViewById<CheckBox>(Resource.Id.lower);
            lower.CheckedChange += CheckboxClicked;            
            shootTrench = FindViewById<CheckBox>(Resource.Id.shootTrench);
            shootTrench.CheckedChange += CheckboxClicked;
            shootInitiation = FindViewById<CheckBox>(Resource.Id.shootInitiation);
            shootInitiation.CheckedChange += CheckboxClicked;
            shootPort = FindViewById<CheckBox>(Resource.Id.shootPort);
            shootPort.CheckedChange += CheckboxClicked;
            climb = FindViewById<CheckBox>(Resource.Id.climb);
            climb.CheckedChange += CheckboxClicked;
            readjust = FindViewById<CheckBox>(Resource.Id.readjust);
            readjust.CheckedChange += CheckboxClicked;
            wheel = FindViewById<CheckBox>(Resource.Id.wheel);
            wheel.CheckedChange += CheckboxClicked;
            rotation = FindViewById<CheckBox>(Resource.Id.rotationC);
            rotation.CheckedChange += CheckboxClicked;
            position = FindViewById<CheckBox>(Resource.Id.positionC);
            position.CheckedChange += CheckboxClicked;
            under = FindViewById<CheckBox>(Resource.Id.under);
            under.CheckedChange += CheckboxClicked;
            radioDrivers = FindViewById<RadioGroup>(Resource.Id.radioDrivers);
            radioDrivers.CheckedChange += RadioClicked;
            radioRecommend = FindViewById<RadioGroup>(Resource.Id.radioRecommend);
            radioRecommend.CheckedChange += RadioClicked;
            radioResult = FindViewById<RadioGroup>(Resource.Id.radioResult);
            radioResult.CheckedChange += RadioClicked;
            textComments = FindViewById<MultiAutoCompleteTextView>(Resource.Id.comments);
            bFinish = FindViewById<Button>(Resource.Id.bFinish);
            bFinish.Click += ButtonClicked;

            //Put driverstation positions into the dropdown.
            string[] positions = new string[] { "Red 1", "Red 2", "Red 3", "Blue 1", "Blue 2", "Blue 3" };
            ArrayAdapter posAdapt = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, positions);
            spinnerPosition.Adapter = posAdapt;

            //Get current match and set title text.
            currentEvent = ScoutDatabase.GetCurrentEvent();
            currentMatch = ScoutDatabase.GetCurrentMatch();
            textTitle.Text += currentMatch.MatchNumber.ToString() + "'";

            //Re-create the scouting form to match the current MatchData's properties.
            textMatchNumber.Text = currentMatch.MatchNumber.ToString();
            textTeamNumber.Text = currentMatch.TeamNumber.ToString();
            spinnerPosition.SetSelection(currentMatch.DSPosition);
            table.Checked = currentMatch.IsTable;
            initiationLine.Checked = currentMatch.InitiationCrossed;
            ((RadioButton)radioAuto.GetChildAt(currentMatch.AutoResult)).Checked = true;
            shoot.Checked = currentMatch.Shoot;
            outer.Checked = currentMatch.ShootOuter;
            inner.Checked = currentMatch.ShootInner;
            lower.Checked = currentMatch.ShootLower;            
            shootTrench.Checked = currentMatch.ShootTrench;
            shootInitiation.Checked = currentMatch.ShootInitiation;
            shootPort.Checked = currentMatch.ShootPort;
            climb.Checked = currentMatch.Climb;
            readjust.Checked = currentMatch.AdjustClimb;
            wheel.Checked = currentMatch.Wheel;
            rotation.Checked = currentMatch.RotationControl;
            position.Checked = currentMatch.PositionControl;
            under.Checked = currentMatch.UnderTrench;
            ((RadioButton)radioDrivers.GetChildAt(Convert.ToInt16(!currentMatch.GoodDrivers))).Checked = true;
            ((RadioButton)radioRecommend.GetChildAt(currentMatch.WouldRecommend)).Checked = true;
            ((RadioButton)radioResult.GetChildAt(currentMatch.Result)).Checked = true;
            textComments.Text = currentMatch.AdditionalComments;
        }

        //Handle when a button on the form is clicked.
        private void ButtonClicked(object sender, EventArgs e)
        {
            //Decide which button was clicked.
            if ((sender as Button) == bFinish)
            {
                try
                {
                    //Create a new MatchData based off the results of the form.
                    matchData = new MatchData(
                    currentEvent.EventName,
                    currentEvent.StartDate,
                    currentEvent.EndDate,
                    int.Parse(textMatchNumber.Text),
                    int.Parse(textTeamNumber.Text),
                    driverIndex,
                    table.Checked,
                    initiationLine.Checked,
                    auto,
                    shoot.Checked,
                    outer.Checked,
                    inner.Checked,
                    lower.Checked,                    
                    shootTrench.Checked,
                    shootInitiation.Checked,
                    shootPort.Checked,
                    climb.Checked,
                    readjust.Checked,
                    wheel.Checked,
                    rotation.Checked,
                    position.Checked,
                    under.Checked,
                    goodDrivers,
                    wouldRecommend,
                    result,
                    textComments.Text,
                    false,
                    currentEvent.EventID);

                    //Delete the old data and add the new.
                    ScoutDatabase.DeleteMatchData(currentMatch.ID);
                    ScoutDatabase.AddMatchData(matchData);
                    //Return to the main scouting page.
                    StartActivity(typeof(ScoutLandingPage));
                    Finish();
                }

                //An exception is thrown if the match and team number are not filled out or if the database already contains a match of the same number for that event.
                catch
                {
                    Popup.Single("Alert", "Please Enter At Least the Match and Team Number//Possible Duplicate Match Number", "OK", this);
                }
            }
        }

        //Get driverstation position based on index of item choosed in the spinner.
        private int driverIndex;

        private void SpinnerClick(object sender, ItemSelectedEventArgs e)
        {
            driverIndex = e.Position;
        }

        //Handle radio buttons.
        private void RadioClicked(object sender, EventArgs e)
        {
            //Placeholder for group of radio buttons that was modified.
            RadioGroup selectedGroup = sender as RadioGroup;

            //Placeholder for the button itself.
            RadioButton clickedButton = FindViewById<RadioButton>(selectedGroup.CheckedRadioButtonId);

            //Check the clicked button
            clickedButton.Checked = true;

            //Determine the index of the button clicked in its group.
            int childIndex = selectedGroup.IndexOfChild(clickedButton);

            //Decide which group was modified and change fields based on that.
            if (selectedGroup == radioAuto)
            {
                auto = childIndex;
            }
            else if (selectedGroup == radioDrivers)
            {
                if (childIndex == 0)
                {
                    goodDrivers = true;
                }
                else
                {
                    goodDrivers = false;
                }
            }
            else if (selectedGroup == radioRecommend)
            {
                wouldRecommend = childIndex;
            }
            else if (selectedGroup == radioResult)
            {
                result = childIndex;
            }
        }

        /// <summary>
        /// Handle checkboxes. Make sure that if, for example, a general attribute like "shoot" is unchecked, the checkboxes underneath are also unchecked (if the robot can't shoot, it can't shoot in the outer goal, etc).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckboxClicked(object sender, EventArgs e)
        {
            //Placeholder for the modified checkbox.
            CheckBox clicked = sender as CheckBox;
            if ((clicked == rotation || clicked == position) && clicked.Checked)
            {
                wheel.Checked = true;
            }
            else if ((clicked == outer || clicked == inner || clicked == lower || clicked == shootTrench || clicked == shootInitiation || clicked == shootPort) && clicked.Checked)
            {
                shoot.Checked = true;
            }
            else if (clicked == wheel && !clicked.Checked)
            {
                rotation.Checked = false;
                position.Checked = false;
            }
            else if (clicked == shoot && !clicked.Checked)
            {
                outer.Checked = false;
                inner.Checked = false;
                lower.Checked = false;                
                shootTrench.Checked = false;
                shootInitiation.Checked = false;
                shootPort.Checked = false;
            }
        }
    }
}