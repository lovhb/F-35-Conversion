using System.IO;
using System.Reflection;
using ModLoader.Framework;
using ModLoader.Framework.Attributes;

namespace F35Conversion
{
    [ItemId("ketkev.F35Convertion")]
    public class Main : VtolMod
    {
        public string modFolder;
        private void Awake()
        {
            modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        public override void UnLoad()
        {
            // Unloaded
        }
    }
}