using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardinalScout2020
{
    /// <summary>
    /// This class displays detailed data for a team.
    /// </summary>
    [Activity(Label = "DetailedTeamData", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class DetailedTeamData: Activity
    {
        //Declare objects for controls.
        private GridView gridStats;
        private GridView gridAuto;
        private GridView gridMatches;
        private TextView textTitle;
        private LinearLayout linearMatches;

        //Placeholder for the current compiled data.
        private CompiledEventData currentCompiled;
       
        /// <summary>
        /// Initialize the activity.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set the correct screen.
            SetContentView(Resource.Layout.Team_Details);

            //Get controls from layout.
            gridStats = FindViewById<GridView>(Resource.Id.gridViewStats);
            gridAuto = FindViewById<GridView>(Resource.Id.gridViewAuto);
            gridMatches = FindViewById<GridView>(Resource.Id.gridViewMatches);
            textTitle = FindViewById<TextView>(Resource.Id.textTeam);
            linearMatches = FindViewById<LinearLayout>(Resource.Id.linearMatches);
            
            //Get current compiled data.
            currentCompiled = ScoutDatabase.GetCurrentCompiled();

            int currentTeam = currentCompiled.CurrentTeam;
            //Display current team in the title.
            textTitle.TextFormatted = TextUtils.ConcatFormatted(FormatString.SetNormal("Viewing Stats for Team: '"), FormatString.SetBold(currentTeam.ToString()), FormatString.SetNormal("'"));
            
            //Get team-specific details from compiled data.                    
            SpannableString shootPerc = currentCompiled.GetShootPercentForTeamFormatted(currentTeam);
            SpannableString prefPort = currentCompiled.GetPrimaryPortForTeamFormatted(currentTeam);
            SpannableString shootPos = currentCompiled.GetLocationForTeamFormatted(currentTeam);
            SpannableString climbPerc = currentCompiled.GetClimbForTeamFormatted(currentTeam);
            SpannableString climbAdj = currentCompiled.GetAdjustForTeamFormatted(currentTeam);
            SpannableString rotation = currentCompiled.GetWheelRotationForTeamFormatted(currentTeam);
            SpannableString position = currentCompiled.GetWheelPositionForTeamFormatted(currentTeam);
            SpannableString recPerc = currentCompiled.GetRecPercentForTeamFormatted(currentTeam);
            SpannableString initiationPerc = currentCompiled.GetInitiationLineForTeamFormatted(currentTeam);
            SpannableString autoHighPerc = currentCompiled.GetHighAutoForTeamFormatted(currentTeam);
            SpannableString autoLowPerc = currentCompiled.GetLowAutoForTeamFormatted(currentTeam);
            SpannableString autoNonePerc = currentCompiled.GetNoneAutoForTeamFormatted(currentTeam);

            /*First Box*/

            //Slash that will be used between certain properties.
            SpannableString slash = FormatString.SetNormal(" / ");

           
            List<SpannableString> statsDisp = new List<SpannableString>()
            {
                FormatString.SetNormal("Shoot % / Port / Location"),
                FormatString.SetNormal("Climb % / Adjust Climb"),
                FormatString.SetNormal("Rotation / Position"),
                FormatString.SetNormal("Recommendation %"),

            };

            //Format shooter data.
            SpannableStringBuilder shootData = new SpannableStringBuilder(shootPerc);
            shootData.Append(slash);
            shootData.Append(prefPort);
            shootData.Append(slash);
            shootData.Append(shootPos);
            statsDisp.Add(new SpannableString(shootData));

            //Format climber data.
            SpannableStringBuilder climbData = new SpannableStringBuilder(climbPerc);
            climbData.Append(slash);
            climbData.Append(climbAdj);
            statsDisp.Add(new SpannableString(climbData));

            //Format spinner data.
            SpannableStringBuilder spinData = new SpannableStringBuilder(rotation);
            spinData.Append(slash);
            spinData.Append(position);
            statsDisp.Add(new SpannableString(spinData));

            statsDisp.Add(recPerc);       

            //Display general stats in first grid box.
            ArrayAdapter gridStatsAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, statsDisp);
            gridStats.Adapter = gridStatsAdapt;

            /*Second Box (Auto)*/

            List<SpannableString> autoDisp = new List<SpannableString>
            {
                FormatString.SetBold("Crossed Initiation Line"),
                initiationPerc,

                FormatString.SetBold("Scored 0 Balls"),
                autoNonePerc,

                FormatString.SetBold("Scored 1-3 Balls"),
                autoLowPerc,

                FormatString.SetBold("Scored 4+ Balls"),
                autoHighPerc
            };


            //Display autonomous stats in the second box.
            ArrayAdapter gridSandstormAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, autoDisp);
            gridAuto.Adapter = gridSandstormAdapt;

            /*Third box (matches)*/

            //Get compiled data (matches) for the active team.            
            List<CompiledEventData> compiled = currentCompiled.GetCompiledForTeam(currentTeam);

            string[] properties = new string[]
            {
                "Result of Team's Alliance",
                "Position",
                "Table",
                "Crossed Initiation Line",
                "Auto",
                "Can Shoot",
                "Outer Port",
                "Inner Port",
                "Lower Port",               
                "Shoots from Trench",
                "Shoots from Initiation Line",
                "Shoots from Against Power Port",
                "Climb",
                "Adjusts on Climbing Bar",
                "Can Spin Color Wheel",
                "Rotation Control",
                "Position Control",
                "Fits Under Trench",
                "Good Driveteam",
                "Recommended"
            };

            //Set the number of columns based on number of matches.
            gridMatches.NumColumns = compiled.Count + 1;
            List<SpannableString> display = new List<SpannableString>();            

            //Rows.
            {
                //Match number
                display.Add(FormatString.SetBold("Match Number:"));
                for (int i = 0; i < compiled.Count; i++)
                {
                    display.Add(FormatString.SetBold(compiled[i].MatchNumber.ToString()));
                }
                int p = 0;

                //Result
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].Result == 0)
                    {
                        display.Add(FormatString.SetColor(compiled[j].GetResult(), Constants.appGreen));
                    }
                    else if (compiled[j].Result == 1)
                    {
                        display.Add(FormatString.SetColor(compiled[j].GetResult(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].GetResult(), Constants.appYellow));
                    }
                }
                p++;

                //Position
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].DSPosition >= 3)
                    {
                        display.Add(FormatString.SetColor(compiled[j].GetPosition(), Constants.appBlue));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].GetPosition(), Constants.appOrange));
                    }
                }
                p++;

                //Table
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].IsTable)
                    {
                        display.Add(FormatString.SetColorBold(compiled[j].IsTable.ToString().ToUpper(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].IsTable.ToString().ToUpper(), Constants.appGreen));
                    }
                }
                p++;

                //Crossed Line
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].InitiationCrossed)
                    {
                        display.Add(FormatString.SetColor(compiled[j].InitiationCrossed.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].InitiationCrossed.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Auto
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].AutoResult == 0)
                    {
                        display.Add(FormatString.SetColor(compiled[j].GetAuto(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].GetAuto(), Constants.appGreen));
                    }
                }
                p++;

                //Shoot
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].Shoot)
                    {
                        display.Add(FormatString.SetColor(compiled[j].Shoot.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].Shoot.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Outer
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].ShootOuter)
                    {
                        display.Add(FormatString.SetColor(compiled[j].ShootOuter.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].ShootOuter.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Inner
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].ShootInner)
                    {
                        display.Add(FormatString.SetColor(compiled[j].ShootInner.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].ShootInner.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Lower
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].ShootLower)
                    {
                        display.Add(FormatString.SetColor(compiled[j].ShootLower.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].ShootLower.ToString(), Constants.appRed));
                    }
                }
                p++;                

                //From Trench
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].ShootTrench)
                    {
                        display.Add(FormatString.SetColor(compiled[j].ShootTrench.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].ShootTrench.ToString(), Constants.appRed));
                    }
                }
                p++;

                //From Line
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].ShootInitiation)
                    {
                        display.Add(FormatString.SetColor(compiled[j].ShootInitiation.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].ShootInitiation.ToString(), Constants.appRed));
                    }
                }
                p++;

                //From Port
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].ShootPort)
                    {
                        display.Add(FormatString.SetColor(compiled[j].ShootPort.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].ShootPort.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Climb
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].Climb)
                    {
                        display.Add(FormatString.SetColor(compiled[j].Climb.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].Climb.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Adjust
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].AdjustClimb)
                    {
                        display.Add(FormatString.SetColor(compiled[j].AdjustClimb.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].AdjustClimb.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Wheel
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].Wheel)
                    {
                        display.Add(FormatString.SetColor(compiled[j].Wheel.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].Wheel.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Rotation
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].RotationControl)
                    {
                        display.Add(FormatString.SetColor(compiled[j].RotationControl.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].RotationControl.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Position
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].PositionControl)
                    {
                        display.Add(FormatString.SetColor(compiled[j].PositionControl.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].PositionControl.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Under Trench
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].UnderTrench)
                    {
                        display.Add(FormatString.SetColor(compiled[j].UnderTrench.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].UnderTrench.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Good drivers
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].GoodDrivers)
                    {
                        display.Add(FormatString.SetColor(compiled[j].GoodDrivers.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.SetColor(compiled[j].GoodDrivers.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Recommended
                display.Add(FormatString.SetBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].WouldRecommend == 0)
                    {
                        display.Add(FormatString.SetColorBold(compiled[j].GetRecommendation(), Constants.appGreen));
                    }
                    else if (compiled[j].WouldRecommend == 1)
                    {
                        display.Add(FormatString.SetColorBold(compiled[j].GetRecommendation(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.SetColorBold(compiled[j].GetRecommendation(), Constants.appYellow));
                    }
                }
            }

            //Put matches in the third grid.
            ArrayAdapter gridMatchesAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridMatches.Adapter = gridMatchesAdapt;

            //Adjust width of the matches grid.
            float scale = Resources.DisplayMetrics.Density;
            FrameLayout.LayoutParams _params = new FrameLayout.LayoutParams((int)(compiled.Count * 500 * scale), Android.Views.ViewGroup.LayoutParams.WrapContent);
            linearMatches.LayoutParameters = _params;
        }
    }
}
    
