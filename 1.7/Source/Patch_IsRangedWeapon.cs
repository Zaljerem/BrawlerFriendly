using HarmonyLib;
using System.Reflection;
using Verse;

namespace BrawlerFriendly
{
    [HarmonyPatch]
    public static class Patch_IsRangedWeapon
    {
       
        static MethodBase TargetMethod()
        {
            return typeof(ThingDef)
                .GetProperty("IsRangedWeapon", BindingFlags.Public | BindingFlags.Instance)
                ?.GetGetMethod(); 
        }

        
        public static bool Prefix(ref bool __result, ThingDef __instance)
        {
            if (__instance.IsWeapon && __instance.HasModExtension<DefModExtension_BrawlerFriendly>())
            {
                //Log.Message($"[BrawlerFriendly] Overriding IsRangedWeapon for {__instance.defName} due to BrawlerFriendly extension.");
                __result = false;
                return false; 
            }

            return true;
        }
    }
}
