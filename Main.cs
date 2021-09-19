using System.Reflection;
using Harmony;

namespace F35Conversion
{
    public class Main : VTOLMOD
    {
        public override void ModLoaded()
        {
            var harmony = HarmonyInstance.Create("Ketkev.F35");

            harmony.PatchAll(Assembly.GetExecutingAssembly());
            base.ModLoaded();
        }
    }
}