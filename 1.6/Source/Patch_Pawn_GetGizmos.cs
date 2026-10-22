using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using UnityEngine;

namespace BrawlerFriendly
{
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.GetGizmos))]
    public static class Patch_Pawn_GetGizmos
    {
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Pawn __instance)
        {
            foreach (var g in __result)
                yield return g;

            if (!__instance.Drafted || !__instance.IsColonistPlayerControlled) yield break;

            ThingWithComps eq = __instance.equipment?.Primary;
            if (eq == null) yield break;

            if (!eq.def.HasModExtension<DefModExtension_BrawlerFriendly>()) yield break;

            // Find a melee verb for this weapon
            Verb meleeVerb = eq.GetComp<CompEquippable>()?
                                .AllVerbs.FirstOrDefault(v => v.IsMeleeAttack);

            if (meleeVerb == null) yield break; // don’t show gizmo if weapon can’t melee

            yield return new Command_Target
            {
                defaultLabel = "Weapon melee attack",
                defaultDesc = "Order this pawn to melee-attack the chosen target using their weapon.",
                icon = ContentFinder<Texture2D>.Get("UI/Commands/AttackMelee"),
                targetingParams = TargetingParameters.ForAttackAny(),
                hotKey = KeyBindingDefOf.Misc1,
                action = targ =>
                {
                    if (!targ.IsValid) return;

                    // Use vanilla melee job driver
                    Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, targ);
                    job.verbToUse = meleeVerb;
                    job.playerForced = true;

                    __instance.jobs.TryTakeOrderedJob(job);
                }
            };
        }
    }
}
