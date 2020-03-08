using Android.App;
using Android.OS;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    /*this activity creates a new MatchData based off of information inputted by users while scouting a match*/

    [Activity(Label = "ScoutFormEdit", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    internal class ScoutFormEdit: Activity
    {
        //declare objects that will refer to controls
        private TextView textTitle;

        private EditText vMatchNumber;
        private EditText vTeamNumber;
        private Spinner vPosition;
        private CheckBox table;

        private CheckBox initiationLine;
        private RadioGroup radioAuto;        
        
        private CheckBox shoot;
        private CheckBox outer;
        private CheckBox inner;
        private CheckBox lower;
        private CheckBox shootWell;
        private CheckBox shootBarely;
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
        private MultiAutoCompleteTextView comments;
        private Button bFinish;

        private int auto = 0;    
        private bool goodDrivers = false;
        private int wouldRecommend = 1;
        private int result = 1;

        //placeholder for new MatchData and current event
        private Event currentEvent;

        private MatchData currentMatch;
        private MatchData matchData;

        //get database instance
        private EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Scout_Form_Edit);
            //get controls from layout and assign event handlers
            textTitle = FindViewById<TextView>(Resource.Id.textEventTitle);
            vMatchNumber = FindViewById<EditText>(Resource.Id.vMatchNumber);
            vTeamNumber = FindViewById<EditText>(Resource.Id.vTeamNumber);
            vPosition = FindViewById<Spinner>(Resource.Id.vPosition);
            vPosition.ItemSelected += SpinnerClick;
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
            shootWell = FindViewById<CheckBox>(Resource.Id.shootWell);
            shootWell.CheckedChange += CheckboxClicked;
            shootBarely = FindViewById<CheckBox>(Resource.Id.shootBarely);
            shootBarely.CheckedChange += CheckboxClicked;
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
            comments = FindViewById<MultiAutoCompleteTextView>(Resource.Id.comments);
            bFinish = FindViewById<Button>(Resource.Id.bFinish);
            bFinish.Click += ButtonClicked;
            //put positions into dropdown
            string[] positions = new string[] { "Red 1", "Red 2", "Red 3", "Blue 1", "Blue 2", "Blue 3" };
            ArrayAdapter posAdapt = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, positions);
            vPosition.Adapter = posAdapt;
            //get current event and set title text
            currentEvent = eData.GetCurrentEvent();
            currentMatch = eData.GetCurrentMatch();
            textTitle.Text += currentMatch.matchNumber.ToString() + "'";

            //set options to match properties
            vMatchNumber.Text = currentMatch.matchNumber.ToString();
            vTeamNumber.Text = currentMatch.teamNumber.ToString();
            vPosition.SetSelection(currentMatch.position);
            table.Checked = currentMatch.isTable;
            initiationLine.Checked = currentMatch.initiationCrossed;
            ((RadioButton)radioAuto.GetChildAt(currentMatch.auto)).Checked = true;

            shoot.Checked = currentMatch.shoot;
            outer.Checked = currentMatch.shootOuter;
            inner.Checked = currentMatch.shootInner;
            lower.Checked = currentMatch.shootLower;
            shootWell.Checked = currentMatch.shootWell;
            shootBarely.Checked = currentMatch.shootBarely;
            shootTrench.Checked = currentMatch.shootTrench;
            shootInitiation.Checked = currentMatch.shootLine;

            climb.Checked = currentMatch.climb;
            readjust.Checked = currentMatch.adjustClimb;

            wheel.Checked = currentMatch.wheel;
            rotation.Checked = currentMatch.rotationControl;
            position.Checked = currentMatch.positionControl;

            under.Checked = currentMatch.underTrench;
            
            ((RadioButton)radioDrivers.GetChildAt(Convert.ToInt16(!currentMatch.goodDrivers))).Checked = true;
            ((RadioButton)radioRecommend.GetChildAt(currentMatch.wouldRecommend)).Checked = true;
            ((RadioButton)radioResult.GetChildAt(currentMatch.result)).Checked = true;
            comments.Text = currentMatch.additionalComments;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            //decide which button was clicked
            if ((sender as Button) == bFinish)
            {
                try
                {
                    matchData = new MatchData(
                    currentEvent.eventName,
                    currentEvent.startDate,
                    currentEvent.endDate,
                    int.Parse(vMatchNumber.Text),
                    int.Parse(vTeamNumber.Text),
                    spinnerIndex,
                    table.Checked,
                    initiationLine.Checked,
                    auto,
                    shoot.Checked,
                    outer.Checked,
                    inner.Checked,
                    lower.Checked,
                    shootWell.Checked,
                    shootBarely.Checked,
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
                    comments.Text,
                    false,
                    currentEvent.eventID);
                    //add the new match
                    eData.AddMatchData(matchData);
                    //go back to the main scouting page
                    StartActivity(typeof(ScoutLandingPage));
                    Finish();
                }
                //not putting a match or team number will throw an exception; so will a duplicate match number since the MatchData id
                //is based off of match number
                catch
                {
                    Popup.Single("Alert", "Please Enter At Least the Match and Team Number//Possible Duplicate Match Number", "OK", this);
                }
            }
        }

        //make changes to match
        //get driverstation position
        private int spinnerIndex;

        private void SpinnerClick(object sender, ItemSelectedEventArgs e)
        {
            spinnerIndex = e.Position;
        }

        //handle radio buttons
        private void RadioClicked(object sender, EventArgs e)
        {
            //which group of radio buttons was modified
            RadioGroup selectedGroup = sender as RadioGroup;
            //index of which button was clicked in the group
            int placeHolder;
            placeHolder = selectedGroup.CheckedRadioButtonId;
            RadioButton clickedButton = FindViewById<RadioButton>(placeHolder);
            //check the clicked button
            clickedButton.Checked = true;
            int childIndex = selectedGroup.IndexOfChild(clickedButton);
            //device which group was modified
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

        private void CheckboxClicked(object sender, EventArgs e)
        {
            CheckBox clicked = sender as CheckBox;
            if ((clicked == rotation || wheel == position) && clicked.Checked)
            {
                wheel.Checked = true;
            }
            else if ((clicked == outer || clicked == inner || clicked == lower || clicked == shootWell || clicked == shootBarely || clicked == shootTrench || clicked == shootInitiation) && clicked.Checked)
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
                shootWell.Checked = false;
                shootBarely.Checked = false;
                shootTrench.Checked = false;
                shootInitiation.Checked = false;
            }
        }
    }
}