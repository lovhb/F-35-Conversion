using System.IO;
using System.Reflection;
using HarmonyLib;
using ModLoader;
using ModLoader.Framework;
using ModLoader.Framework.Attributes;

namespace F35Conversion
{
    [ItemId("ketkev.F35Convertion")]
    public class Main : VtolMod
    {
        public string ModFolder;
        private void Awake()
        {
            ModFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        public override void UnLoad()
        {
            // Unloaded
        }
    }
}