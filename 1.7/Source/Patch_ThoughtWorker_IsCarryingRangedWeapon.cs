using HarmonyLib;
using RimWorld;
using Verse;

namespace BrawlerFriendly
{
    [HarmonyPatch(typeof(ThoughtWorker_IsCarryingRangedWeapon), "CurrentStateInternal")]
    public static class Patch_ThoughtWorker_IsCarryingRangedWeapon
    {
        public static bool Prefix(Pawn p, ref ThoughtState __result)
        {
            
            var weapon = p?.equipment?.Primary;
            if (weapon == null)
                return true;

        
            if (weapon.def.HasModExtension<DefModExtension_BrawlerFriendly>())
            {
                //Log.Message($"[BrawlerFriendly] Suppressing thought for {p.LabelShortCap} using brawler-friendly {weapon.def.defName}");
                __result = ThoughtState.Inactive;
                return false; 
            }

            return true; 
        }
    }
}
