using Harmony;
using UnityEngine;

namespace F_35_Conversion.Patches
{
    [HarmonyPatch(typeof(AeroController.ControlSurfaceTransform), "Init")]
    public class ControlSurfacePatches : MonoBehaviour
    {
        static void Postfix(AeroController.ControlSurfaceTransform __instance)
        {
            Debug.Log("Patched INIT!");
            Debug.Log(__instance.transform.name);
            if (__instance.transform == null) return;
            switch (__instance.transform.name)
            {
                case "canardLeft":
                {
                    var leftCanard = __instance.transform.parent;

                    leftCanard.localPosition = new Vector3(1.01f, 1.12f, 8.16f);
                    leftCanard.Rotate(0f, 0f, 4.679f, Space.Self);
                    leftCanard.localScale = new Vector3(1.615172f, 1.488211f, 1f);

                    Debug.Log("Moved left");
                    break;
                }
                case "canardRight":
                {
                    var rightCanard = __instance.transform.parent;

                    rightCanard.localPosition = new Vector3(-1.01f, 1.12f, 8.16f);
                    rightCanard.Rotate(0f, 0f, -4.679f, Space.Self);
                    rightCanard.localScale = new Vector3(1.615172f, 1.488211f, 1f);

                    Debug.Log("Moved right");
                    break;
                }
            }
            
            __instance.pitchFactor = 0.5f;
            // __instance.brakeFactor = -0.08f;
            // __instance.flapsFactor = -0.15f;
            // __instance.AoAFactor = -0.9f;
            __instance.axis = new Vector3(-1, 0, 0);
        }
    }
}