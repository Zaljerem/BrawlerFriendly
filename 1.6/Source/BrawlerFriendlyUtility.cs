using Verse;

namespace BrawlerFriendly
{
    public static class BrawlerFriendlyUtility
    {
        public static bool IsBrawlerFriendlyWeapon(ThingWithComps weapon)
        {
            return weapon?.def?.GetModExtension<DefModExtension_BrawlerFriendly>() != null;
        }
    }
}
