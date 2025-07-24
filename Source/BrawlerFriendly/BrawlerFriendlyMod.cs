using HarmonyLib;
using Verse;

namespace BrawlerFriendly
{
    public class BrawlerFriendlyMod : Mod
    {
        
        public BrawlerFriendlyMod(ModContentPack content) : base(content)
        {            
            new Harmony("zal.brawlerfriendly").PatchAll();

        }        

    }
}
