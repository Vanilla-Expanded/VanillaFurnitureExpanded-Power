using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using VEF.Maps;
using VEF.AestheticScaling;
using VEF.Things;
using Verse.Noise;

namespace VanillaPowerExpanded
{

    public class CompTidalPowerPlant : CompPowerPlant
    {

        public float cachedMultiplier = -1;

        public float GetTideMultiplier
        {
            get
            {
                if(cachedMultiplier == -1 && this.parent.Map!=null)
                {
                    cachedMultiplier = 1;
                    foreach (TileMutatorDef mutator in this.parent.Map.Tile.Tile.Mutators)
                    {
                        TileMutatorExtension extension = mutator.GetModExtension<TileMutatorExtension>();

                        if (extension != null && extension.tideStrengthMultiplier != 1)
                        {
                            cachedMultiplier *= extension.tideStrengthMultiplier;
                        }

                    }
                }
                return cachedMultiplier;
            }

        }

        protected override float DesiredPowerOutput
        {
            get
            {

                return base.DesiredPowerOutput * VanillaPowerExpanded_Settings.tidalOutputMultiplier * GetTideMultiplier;
            }
        }

       

    }
}
