using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaPowerExpanded
{
   
    public class CompTidalPowerPlant : CompPowerPlant
    {
        protected override float DesiredPowerOutput
        {
            get
            {
               
                return base.DesiredPowerOutput * VanillaPowerExpanded_Settings.tidalOutputMultiplier;
            }
        }

           }
}
