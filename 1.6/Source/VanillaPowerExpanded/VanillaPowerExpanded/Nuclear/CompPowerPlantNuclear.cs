﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System.Text;

namespace VanillaPowerExpanded
{
    public class CompPowerPlantNuclear : CompPowerTrader
    {

        protected CompRefuelable refuelableComp;

        protected CompBreakdownable breakdownableComp;

        protected CompVariableHeatPusher variableHeatPusherComp;

        public float radiationRadius = 0;

        public int tickRadiation = 0;

        public const float radiationRadiusBase = 5f;

        public const float tickRadiationBase = 1250;

        public int temperatureRightNow = 0;
        public const int criticalTempWarning = 80;
        public const int criticalTemp = 100;

        public bool signalMeltdown = false;

        public bool noReactorRoom = false;

        public bool onlyBreakOnceDuringSolarFlare = false;



        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<float>(ref this.radiationRadius, "radiationRadius", 0, false);
            Scribe_Values.Look<int>(ref this.tickRadiation, "tickRadiation", 0, false);
            Scribe_Values.Look<bool>(ref this.signalMeltdown, "signalMeltdown", false, false);
            Scribe_Values.Look<bool>(ref this.onlyBreakOnceDuringSolarFlare, "onlyBreakOnceDuringSolarFlare", false, false);

        }

        protected virtual float DesiredPowerOutputAndRadius
        {
            get
            {
                
                if (this.breakdownableComp.BrokenDown && signalMeltdown)
                {

                    radiationRadius = radiationRadiusBase * 7 * VanillaPowerExpanded_Settings.nuclearRadiationMultiplier;
                    tickRadiation = (int)Math.Round(tickRadiationBase)/3;
                    return 0f;
                }
                if (this.breakdownableComp.BrokenDown && !signalMeltdown)
                {

                    radiationRadius = 0;
                    tickRadiation = 0;
                    return 0f;
                }
                if (noReactorRoom)
                {
                    radiationRadius = 0;
                    tickRadiation = 0;
                    return 0f;
                }
                if (!base.PowerOn)
                {
                    return 0f;
                }
               
                if (!this.refuelableComp.HasFuel)
                {
                    return 0f;
                }
                if (this.refuelableComp.FuelPercentOfMax <= 0.5)
                {
                    signalMeltdown = false;
                    radiationRadius = 0;
                    tickRadiation = 0;
                    variableHeatPusherComp.HeatPerSecondVariable = variableHeatPusherComp.Props.heatPerSecond;
                    return -base.Props.PowerConsumption * VanillaPowerExpanded_Settings.nuclearOutputMultiplier;
                } else
                {
                    signalMeltdown = false;
                    float powerAdditional;
                    powerAdditional = (this.refuelableComp.FuelPercentOfMax) * base.Props.PowerConsumption;
                    radiationRadius = (radiationRadiusBase + ((this.refuelableComp.FuelPercentOfMax - 0.5f) * radiationRadiusBase *5))* VanillaPowerExpanded_Settings.nuclearRadiationMultiplier;
                    variableHeatPusherComp.HeatPerSecondVariable = variableHeatPusherComp.Props.heatPerSecond + (variableHeatPusherComp.Props.heatPerSecond * this.refuelableComp.FuelPercentOfMax);                
                    tickRadiation = (int)Math.Round((tickRadiationBase * (1.0f - this.refuelableComp.FuelPercentOfMax)) + tickRadiationBase);
                    return (-base.Props.PowerConsumption - powerAdditional)  *VanillaPowerExpanded_Settings.nuclearOutputMultiplier;
                }
                
            }
        }

        public override void PostDrawExtraSelectionOverlays()
        {
            if (this.radiationRadius > 0)
            {
                GenDraw.DrawCircleOutline(this.parent.Position.ToVector3Shifted(), this.radiationRadius, SimpleColor.Green);
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            this.refuelableComp = this.parent.GetComp<CompRefuelable>();
            this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
            this.variableHeatPusherComp = this.parent.GetComp<CompVariableHeatPusher>();

            if (base.Props.PowerConsumption < 0f && !this.parent.IsBrokenDown() && FlickUtility.WantsToBeOn(this.parent))
            {
                base.PowerOn = true;
            }
        }

        public override void CompTick()
        {
            base.CompTick();

           

            this.UpdateDesiredPowerOutput();

            if (!onlyBreakOnceDuringSolarFlare) {
                if (this.parent.Map.gameConditionManager.ElectricityDisabled(this.parent.Map))
                {
                    this.breakdownableComp.DoBreakdown();
                    onlyBreakOnceDuringSolarFlare = true;
                }
            }
            if (!this.parent.Map.gameConditionManager.ElectricityDisabled(this.parent.Map))
            {                
                onlyBreakOnceDuringSolarFlare = false;
            }


            Room room = this.parent.PositionHeld.GetRoom(this.parent.Map);
            if (room != null)
            {
                if ((room.OutdoorsForWork || (!this.parent.Map.roofGrid.Roofed(this.parent.PositionHeld))) || room.OpenRoofCount > 0)
                {
                    noReactorRoom = true;
                }
                else noReactorRoom = false;
            } 

            if (!noReactorRoom) {
                if (this.radiationRadius > 0)
                {
                    if (Find.TickManager.TicksGame % tickRadiation == 0)
                    {
                        int num = GenRadial.NumCellsInRadius(this.radiationRadius);
                        for (int i = 0; i < num; i++)
                        {
                            AffectCell(this.parent.Position + GenRadial.RadialPattern[i]);
                        }
                    }
                }


                float result;
                GenTemperature.TryGetTemperatureForCell(this.parent.Position, this.parent.Map, out result);
                temperatureRightNow = (int)Math.Round(result);
                if ((temperatureRightNow > criticalTemp) && !signalMeltdown && !VanillaPowerExpanded_Settings.disableNuclearMeltdowns)
                {
                    signalMeltdown = true;
                    this.parent.Map.weatherManager.curWeather = WeatherDef.Named("VPE_RadioactiveFog");
                    this.parent.Map.weatherManager.TransitionTo(WeatherDef.Named("VPE_RadioactiveFog"));
                    this.breakdownableComp.DoBreakdown();
                    this.refuelableComp.ConsumeFuel(refuelableComp.Fuel);
                    List<ThingDef> links = new List<ThingDef>();
                    links.Add(ThingDef.Named("VPE_NuclearGenerator"));
                    Find.LetterStack.ReceiveLetter("VPE_MeltdownLetterLabel".Translate(), "VPE_MeltdownLetter".Translate(), LetterDefOf.NegativeEvent, this.parent, null, null, links, null);
                }

            }
            


        }

        public void AffectCell(IntVec3 c)
        {
           
            if (c.InBounds(this.parent.Map))
            {
                HashSet<Thing> hashSet = new HashSet<Thing>(c.GetThingList(this.parent.Map));
                if (hashSet != null)
                {
                    foreach (Thing thing in hashSet)
                    {
                        Pawn affectedPawn = thing as Pawn;
                        if (affectedPawn != null && affectedPawn.RaceProps.IsFlesh)
                        {
                            float num = 0.028758334f;
                            num *= 1-(affectedPawn.GetStatValue(StatDefOf.ToxicResistance, true));
                            if (num != 0f)
                            {
                                float num2 = Mathf.Lerp(0.85f, 1.15f, Rand.ValueSeeded(affectedPawn.thingIDNumber ^ 74374237));
                                num *= num2;
                                HealthUtility.AdjustSeverity(affectedPawn, HediffDefOf.ToxicBuildup, num);
                            }

                        }
                    }
                }





               

            }
        }

        public void UpdateDesiredPowerOutput()
        {
           
            base.PowerOutput = this.DesiredPowerOutputAndRadius;
        }

        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.CompInspectStringExtra());
            stringBuilder.AppendLine();


            if (noReactorRoom)
            {
                stringBuilder.Append("VPE_NoReactorRoom".Translate());
                if (this.parent.GetComp<CompBreakdownable>().BrokenDown && signalMeltdown)
                {
                    stringBuilder.AppendLine();
                    CompPlantHarmRadiusIfBroken comp = this.parent.GetComp<CompPlantHarmRadiusIfBroken>();
                    stringBuilder.Append("VPE_Meltdown".Translate() + ": " + comp.CurrentRadius.ToString("0.0") + " meters");
                }
                return stringBuilder.ToString();
                
            }
            else
            {
                if (temperatureRightNow < 80)
                {
                    stringBuilder.Append("VPE_TempInReactorRoom".Translate(temperatureRightNow));
                }
                else stringBuilder.Append("VPE_TempInReactorRoomCritical".Translate(temperatureRightNow));

                if (this.radiationRadius > 0)
                {
                    stringBuilder.AppendLine();

                    stringBuilder.Append("VPE_RadiationProduced".Translate((int)Math.Round(this.radiationRadius)));

                }
                if (this.parent.GetComp<CompBreakdownable>().BrokenDown && signalMeltdown)
                {
                    stringBuilder.AppendLine();
                    CompPlantHarmRadiusIfBroken comp = this.parent.GetComp<CompPlantHarmRadiusIfBroken>();
                    stringBuilder.Append("VPE_Meltdown".Translate() + ": " + comp.CurrentRadius.ToString("0.0") + " meters");
                }

                return stringBuilder.ToString();

            }
            
            
        }


    }
}
