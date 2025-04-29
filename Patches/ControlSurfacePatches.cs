using F35Conversion.Scripts;
using HarmonyLib;
using UnityEngine;
using static F35Conversion.Logger;

namespace F35Conversion.Patches;

[HarmonyPatch(typeof(AeroController.ControlSurfaceTransform), "Init")]
public class ControlSurfacePatches : MonoBehaviour
{
    private static bool Prefix(AeroController.ControlSurfaceTransform __instance)
    {
        Log("Prefix called for: " + __instance.transform.name);

        if (__instance.transform == null)
        {
            Log("Transform is null");
            return true;
        }

        var actor = __instance.transform.root.GetComponent<Actor>();
        if (actor == null)
        {
            Log("Actor is null");
            return true;
        }

        if (!actor.isPlayer && actor.team == Teams.Enemy)
        {
            Log("Actor is not player and is enemy");
            return true;
        }

        switch (__instance.transform.name)
        {
            case "canardLeft":
            {
                Log("Processing canardLeft");
                var leftCanard = __instance.transform.parent;

                _fakeLeftCan = Instantiate(leftCanard.gameObject, leftCanard.parent, true);
                _fakeLeftCan.transform.name = "FakeCanardLeft";

                _fakeLeftCan.transform.localPosition = leftCanard.transform.localPosition;
                _fakeLeftCan.transform.localRotation = leftCanard.transform.localRotation;
                _fakeLeftCan.transform.localScale = leftCanard.transform.localScale;

                _fakeLeftCan.transform.localPosition = new Vector3(1.01f, 1.12f, 8.16f);
                _fakeLeftCan.transform.Rotate(0f, 0f, 4.679f, Space.Self);
                _fakeLeftCan.transform.localScale = new Vector3(1.615172f, 1.488211f, 1f);

                foreach (var w in _fakeLeftCan.GetComponentsInChildren<Wing>(true)) Destroy(w);

                // Add the CanardMirror component to copy rotations
                var mirror = _fakeLeftCan.AddComponent<CanardMirror>();
                mirror.sourceCanard = __instance.transform;

                var meshes = leftCanard.GetComponentsInChildren<MeshRenderer>(true);

                foreach (var mesh in meshes) mesh.enabled = false;

                Log("F35 - Copied left");
                break;
            }
            case "canardRight":
            {
                Log("Processing canardRight");
                var rightCanard = __instance.transform.parent;

                _fakeRightCan = Instantiate(rightCanard.gameObject, rightCanard.parent, true);
                _fakeRightCan.transform.name = "fakeCanardRight";

                _fakeRightCan.transform.localPosition = rightCanard.transform.localPosition;
                _fakeRightCan.transform.localRotation = rightCanard.transform.localRotation;
                _fakeRightCan.transform.localScale = rightCanard.transform.localScale;

                _fakeRightCan.transform.localPosition = new Vector3(-1.01f, 1.12f, 8.16f);
                _fakeRightCan.transform.Rotate(0f, 0f, -4.679f, Space.Self);
                _fakeRightCan.transform.localScale = new Vector3(1.615172f, 1.488211f, 1f);

                foreach (var w in _fakeRightCan.GetComponentsInChildren<Wing>(true)) Destroy(w);

                // Add the CanardMirror component to copy rotations
                var mirror = _fakeRightCan.AddComponent<CanardMirror>();
                mirror.sourceCanard = __instance.transform;

                var meshes = rightCanard.GetComponentsInChildren<MeshRenderer>(true);

                foreach (var mesh in meshes) mesh.enabled = false;

                Log("F35 - Copied right");
                break;
            }
            default:
                LogWarn("Unhandled transform name: " + __instance.transform.name);
                break;
        }

        return true;
    }

    private static GameObject _fakeLeftCan;
    private static GameObject _fakeRightCan;
}