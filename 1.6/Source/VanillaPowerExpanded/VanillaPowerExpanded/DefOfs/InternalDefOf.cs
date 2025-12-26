using System;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace VanillaPowerExpanded
{
    [DefOf]
    public static class InternalDefOf
    {
        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }
      
        [MayRequire("VanillaExpanded.VExplorationE")]
        public static TileMutatorDef VEE_MoreSolarPower;
        [MayRequire("VanillaExpanded.VExplorationE")]
        public static TileMutatorDef VEE_LessSolarPower;
    }
}
