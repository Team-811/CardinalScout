using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardinalScout2020
{
    [Activity(Label = "DetailedTeamData", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class DetailedTeamData: Activity
    {
        //declare objects for controls
        private GridView gridStats;
        private GridView gridAuto;
        private GridView gridMatches;
        private TextView textTitle;
        private LinearLayout linearMatches;

        //placeholder for the current compiled data
        private CompiledEventData currentCompiled;

        private EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Team_Details);
            //get controls from layout
            gridStats = FindViewById<GridView>(Resource.Id.gridViewStats);
            gridAuto = FindViewById<GridView>(Resource.Id.gridViewAuto);
            gridMatches = FindViewById<GridView>(Resource.Id.gridViewMatches);
            textTitle = FindViewById<TextView>(Resource.Id.textTeam);
            linearMatches = FindViewById<LinearLayout>(Resource.Id.linearMatches);
            //get current compiled data
            currentCompiled = eData.GetCurrentCompiled();
            //put matches in order for team
            CompiledEventData[] compiledPreSort = eData.GetCompiledEventDataForIndex(eData.getTeamIndex().ID).ToArray();
            Array.Sort(compiledPreSort, delegate (CompiledEventData data1, CompiledEventData data2)
            {
                return data1.matchNumber.CompareTo(data2.matchNumber);
            });
            Array.Reverse(compiledPreSort);
            List<CompiledEventData> compiled = compiledPreSort.ToList();
            int currentTeam = compiled[0].teamNumber;
            //display current team in the title
            textTitle.TextFormatted = TextUtils.ConcatFormatted(FormatString.setNormal("Viewing Stats for Team: '"), FormatString.setBold(currentTeam.ToString()), FormatString.setNormal("'"));
            //get team-specific details from compiled data                      
            int shootPerc = currentCompiled.getShootPercentForTeam(currentTeam);
            string prefPort = currentCompiled.getPrimaryPort(currentTeam);
            string shootPos = currentCompiled.getPrimaryShoot(currentTeam);
            int climbPerc = currentCompiled.getClimbPercentForTeam(currentTeam);
            string climbAdj = currentCompiled.getAdjustForTeam(currentTeam).ToString();
            bool rotation = currentCompiled.getWheelRotationForTeam(currentTeam);
            bool position = currentCompiled.getWheelPositionForTeam(currentTeam);
            int recPerc = currentCompiled.getRecPercentForTeam(currentTeam);

            int initiationPerc = currentCompiled.getInitiationLineForTeam(currentTeam);
            int autoHighPerc = currentCompiled.getHighAutoForTeam(currentTeam);
            int autoLowPerc = currentCompiled.getLowAutoForTeam(currentTeam);
            int autoNonePerc = currentCompiled.getNoneAutoForTeam(currentTeam);


            //first two rows
            List<SpannableString> statsDisp = new List<SpannableString>()
            {
                FormatString.setNormal("Shoot % / Port / Location"),
                FormatString.setNormal("Climb % / Adjust Climb"),
                FormatString.setNormal("Rotation / Position"),
                FormatString.setNormal("Recommendation %"),

                FormatString.setBold(shootPerc.ToString()+"% / "+ prefPort + " / " + shootPos),
                FormatString.setBold(climbPerc.ToString()+"% / "+ climbAdj),
                FormatString.setBold(rotation.ToString() + " / " + position.ToString()),
                FormatString.setBold(recPerc.ToString()+"%"),
            };
            //third row (decide if a team is good based off calculations) 
            //shooting
            if (shootPerc >= Constants.shootThreshHigh)
            {
                statsDisp.Add(FormatString.setColorBold("GOOD", Constants.appGreen));
            }
            else if (shootPerc <= Constants.shootThreshLow)
            {
                statsDisp.Add(FormatString.setColorBold("BAD", Constants.appRed));
            }
            else
            {
                statsDisp.Add(FormatString.setBold("Neutral"));
            }
            //what kind of climber
            if (climbPerc >= Constants.climbThreshHigh)
            {
                statsDisp.Add(FormatString.setColorBold("GOOD", Constants.appGreen));
            }
            else if (climbPerc <= Constants.climbThreshLow)
            {
                statsDisp.Add(FormatString.setColorBold("BAD", Constants.appRed));
            }
            else
            {
                statsDisp.Add(FormatString.setBold("Neutral"));
            }
            //color wheel
            if (rotation || position)
            {
                statsDisp.Add(FormatString.setColorBold("GOOD", Constants.appGreen));
            }
            else
            {
                statsDisp.Add(FormatString.setBold("Neutral"));
            }
            //recommendation
            if (recPerc >= Constants.recommendThreshHigh)
            {
                statsDisp.Add(FormatString.setColorBold("Recommended", Constants.appGreen));
            }
            else if (recPerc <= Constants.recommendThreshLow)
            {
                statsDisp.Add(FormatString.setColorBold("Not Recommended", Constants.appRed));
            }
            else
            {
                statsDisp.Add(FormatString.setColorBold("Possible Recommend", Constants.appYellow));
            }

            //display general stats in first grid box
            ArrayAdapter gridStatsAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, statsDisp);
            gridStats.Adapter = gridStatsAdapt;

            //autonomous display    

            List<SpannableString> autoDisp = new List<SpannableString>();

            autoDisp.Add(FormatString.setBold("Crossed Initiation Line"));
            if (initiationPerc >= Constants.initiationThreshHigh)
                autoDisp.Add(FormatString.setColorBold(initiationPerc.ToString() + "% (GOOD)", Constants.appGreen));
            else if (initiationPerc <= Constants.initiationThreshLow)
                autoDisp.Add(FormatString.setColorBold(initiationPerc.ToString() + "% (GOOD)", Constants.appRed));
            else
                autoDisp.Add(FormatString.setBold(initiationPerc.ToString() + "%"));

            autoDisp.Add(FormatString.setBold("Scored 0 Balls"));
            if (autoNonePerc >= Constants.autoNoneThresh)
                autoDisp.Add(FormatString.setColorBold(autoNonePerc.ToString() + "% (BAD)", Constants.appRed));
            else
                autoDisp.Add(FormatString.setBold(autoNonePerc.ToString() + "%"));

            autoDisp.Add(FormatString.setBold("Scored 1-3 Balls"));
            if (autoLowPerc + autoHighPerc >= Constants.autoBallThresh)
                autoDisp.Add(FormatString.setColorBold(autoLowPerc.ToString() + "% (GOOD)", Constants.appGreen));
            else
                autoDisp.Add(FormatString.setBold(autoLowPerc.ToString() + "%"));

            autoDisp.Add(FormatString.setBold("Scored 4+ Balls"));
            if (autoHighPerc + autoLowPerc >= Constants.autoBallThresh)
                autoDisp.Add(FormatString.setColorBold(autoHighPerc.ToString() + "% (GOOD)", Constants.appGreen));
            else
                autoDisp.Add(FormatString.setBold(autoHighPerc.ToString() + "%"));


            //display sandstorm stats in the second grid
            ArrayAdapter gridSandstormAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, autoDisp);
            gridAuto.Adapter = gridSandstormAdapt;

            //display a list of matches the team was in and the details from each one
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
                "Shoots Well",
                "Barely Shoots",
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

            gridMatches.NumColumns = compiled.Count + 1;
            List<SpannableString> display = new List<SpannableString>();
            compiled.Reverse();
            //rows
            {
                display.Add(FormatString.setBold("Match Number:"));
                for (int i = 0; i < compiled.Count; i++)
                {
                    display.Add(FormatString.setBold(compiled[i].matchNumber.ToString()));
                }
                int p = 0;

                //Result
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].result == 0)
                    {
                        display.Add(FormatString.setColor(compiled[j].getResult(), Constants.appGreen));
                    }
                    else if (compiled[j].result == 1)
                    {
                        display.Add(FormatString.setColor(compiled[j].getResult(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].getResult(), Constants.appYellow));
                    }
                }
                p++;

                //Position
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].position >= 3)
                    {
                        display.Add(FormatString.setColor(compiled[j].getPosition(), Constants.appBlue));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].getPosition(), Constants.appOrange));
                    }
                }
                p++;

                //Table
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].isTable)
                    {
                        display.Add(FormatString.setColorBold(compiled[j].isTable.ToString().ToUpper(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].isTable.ToString().ToUpper(), Constants.appGreen));
                    }
                }
                p++;

                //Crossed Line
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].initiationCrossed)
                    {
                        display.Add(FormatString.setColor(compiled[j].initiationCrossed.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].initiationCrossed.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Auto
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].auto == 0)
                    {
                        display.Add(FormatString.setColor(compiled[j].getAuto(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].getAuto(), Constants.appGreen));
                    }
                }
                p++;

                //Shoot
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].shoot)
                    {
                        display.Add(FormatString.setColor(compiled[j].shoot.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].shoot.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Outer
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].shootOuter)
                    {
                        display.Add(FormatString.setColor(compiled[j].shootOuter.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].shootOuter.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Inner
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].shootInner)
                    {
                        display.Add(FormatString.setColor(compiled[j].shootInner.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].shootInner.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Lower
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].shootLower)
                    {
                        display.Add(FormatString.setColor(compiled[j].shootLower.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].shootLower.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Well
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].shootWell)
                    {
                        display.Add(FormatString.setColor(compiled[j].shootWell.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].shootWell.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Barely
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].shootBarely)
                    {
                        display.Add(FormatString.setColor(compiled[j].shootBarely.ToString(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].shootBarely.ToString(), Constants.appGreen));
                    }
                }
                p++;

                //From Trench
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].shootTrench)
                    {
                        display.Add(FormatString.setColor(compiled[j].shootTrench.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].shootTrench.ToString(), Constants.appRed));
                    }
                }
                p++;

                //From Line
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].shootLine)
                    {
                        display.Add(FormatString.setColor(compiled[j].shootLine.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].shootLine.ToString(), Constants.appRed));
                    }
                }
                p++;

                //From Port
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].shootPort)
                    {
                        display.Add(FormatString.setColor(compiled[j].shootPort.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].shootPort.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Climb
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].climb)
                    {
                        display.Add(FormatString.setColor(compiled[j].climb.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].climb.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Adjust
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].adjustClimb)
                    {
                        display.Add(FormatString.setColor(compiled[j].adjustClimb.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].adjustClimb.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Wheel
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].wheel)
                    {
                        display.Add(FormatString.setColor(compiled[j].wheel.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].wheel.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Rotation
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].rotationControl)
                    {
                        display.Add(FormatString.setColor(compiled[j].rotationControl.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].rotationControl.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Position
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].positionControl)
                    {
                        display.Add(FormatString.setColor(compiled[j].positionControl.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].positionControl.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Under Trench
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].underTrench)
                    {
                        display.Add(FormatString.setColor(compiled[j].underTrench.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].underTrench.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Good drivers
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].goodDrivers)
                    {
                        display.Add(FormatString.setColor(compiled[j].goodDrivers.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].goodDrivers.ToString(), Constants.appRed));
                    }
                }
                p++;

                //Recommended
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].wouldRecommend == 0)
                    {
                        display.Add(FormatString.setColorBold(compiled[j].getRecommendation(), Constants.appGreen));
                    }
                    else if (compiled[j].wouldRecommend == 1)
                    {
                        display.Add(FormatString.setColorBold(compiled[j].getRecommendation(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.setColorBold(compiled[j].getRecommendation(), Constants.appYellow));
                    }
                }
            }
            //put matches in the third grid
            ArrayAdapter gridMatchesAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridMatches.Adapter = gridMatchesAdapt;
            float scale = this.Resources.DisplayMetrics.Density;
            FrameLayout.LayoutParams _params = new FrameLayout.LayoutParams((int)(compiled.Count * 500 * scale), Android.Views.ViewGroup.LayoutParams.WrapContent);
            linearMatches.LayoutParameters = _params;
        }
    }
}
    
