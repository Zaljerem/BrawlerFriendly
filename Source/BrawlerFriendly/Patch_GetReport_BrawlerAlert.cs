using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace BrawlerFriendly
{
    [HarmonyPatch(typeof(Alert_BrawlerHasRangedWeapon), nameof(Alert_BrawlerHasRangedWeapon.GetReport))]
    public static class Patch_GetReport_BrawlerAlert
    {
        public static void Postfix(ref AlertReport __result)
        {
            if (!__result.active)
                return;

            var originalList = __result.culpritsPawns;
            var filtered = new List<GlobalTargetInfo>();

            foreach (GlobalTargetInfo target in originalList)
            {
                if (target.Thing is not Pawn pawn)
                    continue;

                var primary = pawn.equipment?.Primary;
                if (primary == null || !BrawlerFriendlyUtility.IsBrawlerFriendlyWeapon(primary))
                {
                    filtered.Add(pawn);
                }
                else
                {
                    //Log.Message($"[BrawlerFriendly] Suppressing alert for {pawn.LabelShortCap} holding brawler-friendly weapon: {primary.LabelCap}");
                }
            }

            if (filtered.Count == 0)
            {
                //Log.Message("[BrawlerFriendly] All brawlers filtered, alert suppressed.");
                __result = AlertReport.Inactive;
            }
            else
            {
                //Log.Message($"[BrawlerFriendly] {filtered.Count} brawlers remaining in alert list.");
                __result = AlertReport.CulpritsAre(filtered);
            }
        }
    }
}
