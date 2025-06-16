using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;


namespace VanillaPowerExpanded
{
    public class CompPowerPlantAdvSteam: CompPowerPlantSteam
    {

        protected override float DesiredPowerOutput
        {
            get
            {

                return base.DesiredPowerOutput * VanillaPowerExpanded_Settings.advGeothermalOutputMultiplier;
            }
        }


    }
}
