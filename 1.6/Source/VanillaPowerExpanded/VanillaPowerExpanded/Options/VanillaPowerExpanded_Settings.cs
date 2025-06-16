using RimWorld;
using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;



namespace VanillaPowerExpanded
{
    public class VanillaPowerExpanded_Settings : ModSettings
    {
        public static bool disableNuclearMeltdowns = false;
        public const float nuclearOutputMultiplierBase = 1;
        public static float nuclearOutputMultiplier = nuclearOutputMultiplierBase;
        public const float nuclearRadiationMultiplierBase = 1;
        public static float nuclearRadiationMultiplier = nuclearRadiationMultiplierBase;

        public const float advSolarOutputMultiplierBase = 1;
        public static float advSolarOutputMultiplier = advSolarOutputMultiplierBase;

        public const float fuelPerSoulBase = 2;
        public static float fuelPerSoul = fuelPerSoulBase;

        public const float tidalOutputMultiplierBase = 1;
        public static float tidalOutputMultiplier = tidalOutputMultiplierBase;
        public const float tidalSeparationBase = 40;
        public static float tidalSeparation = tidalSeparationBase;

        public const float advWindmillOutputMultiplierBase = 1;
        public static float advWindmillOutputMultiplier = advWindmillOutputMultiplierBase;

        public const float advWatermillOutputMultiplierBase = 1;
        public static float advWatermillOutputMultiplier = advWatermillOutputMultiplierBase;

        public const float advGeothermalOutputMultiplierBase = 1;
        public static float advGeothermalOutputMultiplier = advGeothermalOutputMultiplierBase;

        private static Vector2 scrollPosition = Vector2.zero;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref disableNuclearMeltdowns, "disableNuclearMeltdowns", false, true);
            Scribe_Values.Look(ref nuclearOutputMultiplier, "nuclearOutputMultiplier", nuclearOutputMultiplierBase, true);
            Scribe_Values.Look(ref nuclearRadiationMultiplier, "nuclearRadiationMultiplier", nuclearRadiationMultiplierBase, true);

            Scribe_Values.Look(ref advSolarOutputMultiplier, "advSolarOutputMultiplier", advSolarOutputMultiplierBase, true);

            Scribe_Values.Look(ref fuelPerSoul, "fuelPerSoul", fuelPerSoulBase, true);

            Scribe_Values.Look(ref tidalOutputMultiplier, "tidalOutputMultiplier", tidalOutputMultiplierBase, true);
            Scribe_Values.Look(ref tidalSeparation, "tidalSeparation", tidalSeparationBase, true);

            Scribe_Values.Look(ref advWindmillOutputMultiplier, "advWindmillOutputMultiplier", advWindmillOutputMultiplierBase, true);

            Scribe_Values.Look(ref advWatermillOutputMultiplier, "advWatermillOutputMultiplier", advWatermillOutputMultiplierBase, true);

            Scribe_Values.Look(ref advGeothermalOutputMultiplier, "advGeothermalOutputMultiplier", advGeothermalOutputMultiplierBase, true);

        }
        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();

            var scrollContainer = inRect.ContractedBy(10);
            scrollContainer.height -= ls.CurHeight;
            scrollContainer.y += ls.CurHeight;
            Widgets.DrawBoxSolid(scrollContainer, Color.grey);
            var innerContainer = scrollContainer.ContractedBy(1);
            Widgets.DrawBoxSolid(innerContainer, new ColorInt(42, 43, 44).ToColor);
            var frameRect = innerContainer.ContractedBy(5);
            frameRect.y += 15;
            frameRect.height -= 15;
            var contentRect = frameRect;
            contentRect.x = 0;
            contentRect.y = 0;
            contentRect.width -= 20;

            contentRect.height = 1400;


            Listing_Standard ls2 = new Listing_Standard();

            Widgets.BeginScrollView(frameRect, ref scrollPosition, contentRect, true);
            ls2.Begin(contentRect.AtZero());

            Listing_Standard ls3 = new Listing_Standard();
            var nuclearRect = contentRect.ContractedBy(5);
            nuclearRect.height = 1350;
            var nuclearRectInt = nuclearRect.ContractedBy(20);
           
            ls3.Begin(nuclearRectInt);
            Widgets.DrawBoxSolid(nuclearRect.ExpandedBy(10), new ColorInt(33, 33, 33).ToColor);
            Text.Font = GameFont.Medium;
            ls3.Label("VPE_NuclearPlantHeader".Translate());
            Text.Font = GameFont.Small;

            ls3.CheckboxLabeled("VPE_DisableNuclearMeltdowns".Translate(), ref disableNuclearMeltdowns, "VPE_DisableNuclearMeltdownsDesc".Translate());
            var NuclearPlantMultiplierLabel = ls3.LabelPlusButton("VPE_NuclearPlantMultiplier".Translate() + ": " + nuclearOutputMultiplier + "x", "VPE_NuclearPlantMultiplierTooltip".Translate());
            nuclearOutputMultiplier = (float)Math.Round(ls3.Slider(nuclearOutputMultiplier, 0f, 20f), 2);

            if (ls3.Settings_Button("VPE_Reset".Translate(), new Rect(0f, NuclearPlantMultiplierLabel.position.y + 35, 250f, 29f)))
            {
                nuclearOutputMultiplier = nuclearOutputMultiplierBase;
            }
            var nuclearRadiationMultiplierLabel = ls3.LabelPlusButton("VPE_NuclearPlantRadiationMultiplier".Translate() + ": " + nuclearRadiationMultiplier + "x", "VPE_NuclearPlantRadiationMultiplierTooltip".Translate());
            nuclearRadiationMultiplier = (float)Math.Round(ls3.Slider(nuclearRadiationMultiplier, 0f, 20f), 2);

            if (ls3.Settings_Button("VPE_Reset".Translate(), new Rect(0f, nuclearRadiationMultiplierLabel.position.y + 35, 250f, 29f)))
            {
                nuclearRadiationMultiplier = nuclearRadiationMultiplierBase;
            }

            ls3.GapLine();

            Text.Font = GameFont.Medium;
            ls3.Label("VPE_SolarHeader".Translate());
            Text.Font = GameFont.Small;

 
            var advSolarOutputMultiplierLabel = ls3.LabelPlusButton("VPE_AdvSolarOutputMultiplier".Translate() + ": " + advSolarOutputMultiplier + "x", "VPE_AdvSolarOutputMultiplierTooltip".Translate());
            advSolarOutputMultiplier = (float)Math.Round(ls3.Slider(advSolarOutputMultiplier, 0f, 10f), 2);

            if (ls3.Settings_Button("VPE_Reset".Translate(), new Rect(0f, advSolarOutputMultiplierLabel.position.y + 35, 250f, 29f)))
            {
                advSolarOutputMultiplier = advSolarOutputMultiplierBase;
            }

            ls3.GapLine();

            Text.Font = GameFont.Medium;
            ls3.Label("VPE_SoulHeader".Translate());
            Text.Font = GameFont.Small;


            var soulFuelLabel = ls3.LabelPlusButton("VPE_SoulMultiplier".Translate() + ": " + fuelPerSoul + " hours / corpse", "VPE_SoulMultiplierTooltip".Translate());
            fuelPerSoul = (float)Math.Round(ls3.Slider(fuelPerSoul, 0f, 50f), 2);

            if (ls3.Settings_Button("VPE_Reset".Translate(), new Rect(0f, soulFuelLabel.position.y + 35, 250f, 29f)))
            {
                fuelPerSoul = fuelPerSoulBase;
            }

            ls3.GapLine();

            Text.Font = GameFont.Medium;
            ls3.Label("VPE_TidalHeader".Translate());
            Text.Font = GameFont.Small;


            var tidalLabel = ls3.LabelPlusButton("VPE_TidalMultiplier".Translate() + ": " + tidalOutputMultiplier + "x", "VPE_TidalMultiplierTooltip".Translate());
            tidalOutputMultiplier = (float)Math.Round(ls3.Slider(tidalOutputMultiplier, 0f, 10f), 2);

            if (ls3.Settings_Button("VPE_Reset".Translate(), new Rect(0f, tidalLabel.position.y + 35, 250f, 29f)))
            {
                tidalOutputMultiplier = tidalOutputMultiplierBase;
            }

            var tidalSeparationLabel = ls3.LabelPlusButton("VPE_TidalSeparation".Translate() + ": " + tidalSeparation + " tiles", "VPE_TidalSeparationTooltip".Translate());
            tidalSeparation = (float)Math.Round(ls3.Slider(tidalSeparation, 5f, 100f), 0);

            if (ls3.Settings_Button("VPE_Reset".Translate(), new Rect(0f, tidalSeparationLabel.position.y + 35, 250f, 29f)))
            {
                tidalSeparation = tidalSeparationBase;
            }


            ls3.GapLine();

            Text.Font = GameFont.Medium;
            ls3.Label("VPE_WindmillHeader".Translate());
            Text.Font = GameFont.Small;


            var advWindmillOutputMultiplierLabel = ls3.LabelPlusButton("VPE_AdvWindmillOutputMultiplier".Translate() + ": " + advWindmillOutputMultiplier + "x", "VPE_AdvWindmillOutputMultiplierTooltip".Translate());
            advWindmillOutputMultiplier = (float)Math.Round(ls3.Slider(advWindmillOutputMultiplier, 0f, 10f), 2);

            if (ls3.Settings_Button("VPE_Reset".Translate(), new Rect(0f, advWindmillOutputMultiplierLabel.position.y + 35, 250f, 29f)))
            {
                advWindmillOutputMultiplier = advWindmillOutputMultiplierBase;
            }

            ls3.GapLine();

            Text.Font = GameFont.Medium;
            ls3.Label("VPE_WatermillHeader".Translate());
            Text.Font = GameFont.Small;


            var advWatermillOutputMultiplierLabel = ls3.LabelPlusButton("VPE_AdvWatermillOutputMultiplier".Translate() + ": " + advWatermillOutputMultiplier + "x", "VPE_AdvWatermillOutputMultiplierTooltip".Translate());
            advWatermillOutputMultiplier = (float)Math.Round(ls3.Slider(advWatermillOutputMultiplier, 0f, 10f), 2);

            if (ls3.Settings_Button("VPE_Reset".Translate(), new Rect(0f, advWatermillOutputMultiplierLabel.position.y + 35, 250f, 29f)))
            {
                advWatermillOutputMultiplier = advWatermillOutputMultiplierBase;
            }

            ls3.GapLine();

            Text.Font = GameFont.Medium;
            ls3.Label("VPE_AdvGeoHeader".Translate());
            Text.Font = GameFont.Small;


            var advGeoOutputMultiplierLabel = ls3.LabelPlusButton("VPE_AdvGeoOutputMultiplier".Translate() + ": " + advGeothermalOutputMultiplier + "x", "VPE_AdvGeoOutputMultiplierTooltip".Translate());
            advGeothermalOutputMultiplier = (float)Math.Round(ls3.Slider(advGeothermalOutputMultiplier, 0f, 10f), 2);

            if (ls3.Settings_Button("VPE_Reset".Translate(), new Rect(0f, advGeoOutputMultiplierLabel.position.y + 35, 250f, 29f)))
            {
                advGeothermalOutputMultiplier = advGeothermalOutputMultiplierBase;
            }

            ls3.End();



            ls2.End();
            Widgets.EndScrollView();
            base.Write();
        }
    }

 
}
