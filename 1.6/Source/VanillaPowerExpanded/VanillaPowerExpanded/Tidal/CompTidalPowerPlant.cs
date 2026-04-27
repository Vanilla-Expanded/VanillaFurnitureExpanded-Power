using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using VEF.Maps;
using VEF.AestheticScaling;
using VEF.Things;
using Verse.Noise;
using VEF;

namespace VanillaPowerExpanded
{

    public class CompTidalPowerPlant : CompPowerPlant
    {

        public float cachedMutatorMultiplier = -1;
        public float cachedGameConditionMultiplier = 1;

        public float GetMutatorTideMultiplier
        {
            get
            {
                if(cachedMutatorMultiplier == -1 && parent.Map!=null)
                {
                    cachedMutatorMultiplier = 1;
                    foreach (TileMutatorDef mutator in parent.Map.Tile.Tile.Mutators)
                    {
                        TileMutatorExtension extension = mutator.GetModExtension<TileMutatorExtension>();

                        if (extension != null && extension.tideStrengthMultiplier != 1)
                        {
                            cachedMutatorMultiplier *= extension.tideStrengthMultiplier;
                        }

                    }

                }
                return cachedMutatorMultiplier;
            }

        }

        protected override float DesiredPowerOutput
        {
            get
            {

                return base.DesiredPowerOutput * VanillaPowerExpanded_Settings.tidalOutputMultiplier * GetMutatorTideMultiplier * cachedGameConditionMultiplier;
            }
        }

        public override void CompTickInterval(int delta)
        {
            base.CompTickInterval(delta);
            if (parent.IsHashIntervalTick(2000 * delta))
            {
                if (parent.Map != null) {
                    if (parent.Map.gameConditionManager.ActiveConditions.Count > 0)
                    {
                        foreach(GameCondition condition in parent.Map.gameConditionManager.ActiveConditions)
                        {
                            MapConditionExtension extension = condition.def.GetModExtension<MapConditionExtension>();
                            if (extension != null)
                            {
                                cachedGameConditionMultiplier *= extension.tideStrengthMultiplier;
                            }

                        }


                    }


                }

            }
        }



    }
}
