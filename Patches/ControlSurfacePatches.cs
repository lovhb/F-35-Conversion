using HarmonyLib;
using UnityEngine;
using static F35Conversion.Logger;

namespace F35Conversion.Patches
{
    [HarmonyPatch(typeof(AeroController.ControlSurfaceTransform), "Init")]
    public class ControlSurfacePatches : MonoBehaviour
    {
        static bool Prefix(AeroController.ControlSurfaceTransform __instance)
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

                        _fakeLeftCan = GameObject.Instantiate(leftCanard.gameObject, leftCanard.parent, true);
                        _fakeLeftCan.transform.name = "FakeCanardLeft";

                        _fakeLeftCan.transform.localPosition = leftCanard.transform.localPosition;
                        _fakeLeftCan.transform.localRotation = leftCanard.transform.localRotation;
                        _fakeLeftCan.transform.localScale = leftCanard.transform.localScale;

                        _fakeLeftCan.transform.localPosition = new Vector3(1.01f, 1.12f, 8.16f);
                        _fakeLeftCan.transform.Rotate(0f, 0f, 4.679f, Space.Self);
                        _fakeLeftCan.transform.localScale = new Vector3(1.615172f, 1.488211f, 1f);

                        foreach (Wing w in _fakeLeftCan.GetComponentsInChildren<Wing>(true))
                        {
                            Destroy(w);
                        }

                        CreateControlSurface(
                            __instance.transform.GetComponentInParent<AeroController>(),
                            _fakeLeftCan.transform, __instance);

                        var meshes = leftCanard.GetComponentsInChildren<MeshRenderer>(true);

                        foreach (var mesh in meshes)
                        {
                            mesh.enabled = false;
                        }

                        Log("F35 - Copied left");
                        break;
                    }
                case "canardRight":
                    {
                        Log("Processing canardRight");
                        var rightCanard = __instance.transform.parent;

                        _fakeRightCan = GameObject.Instantiate(rightCanard.gameObject, rightCanard.parent, true);
                        _fakeRightCan.transform.name = "fakeCanardRight";

                        _fakeRightCan.transform.localPosition = rightCanard.transform.localPosition;
                        _fakeRightCan.transform.localRotation = rightCanard.transform.localRotation;
                        _fakeRightCan.transform.localScale = rightCanard.transform.localScale;

                        _fakeRightCan.transform.localPosition = new Vector3(-1.01f, 1.12f, 8.16f);
                        _fakeRightCan.transform.Rotate(0f, 0f, -4.679f, Space.Self);
                        _fakeRightCan.transform.localScale = new Vector3(1.615172f, 1.488211f, 1f);

                        foreach (var w in _fakeRightCan.GetComponentsInChildren<Wing>(true))
                        {
                            Destroy(w);
                        }

                        CreateControlSurface(
                            __instance.transform.GetComponentInParent<AeroController>(),
                            _fakeRightCan.transform, __instance);

                        var meshes = rightCanard.GetComponentsInChildren<MeshRenderer>(true);

                        foreach (var mesh in meshes)
                        {
                            mesh.enabled = false;
                        }

                        Log("F35 - Copied right");
                        break;
                    }
                default:
                    LogWarn("Unhandled transform name: " + __instance.transform.name);
                    break;
            }

            return true;
        }

        private static void CreateControlSurface(AeroController controller, Transform surface,
            AeroController.ControlSurfaceTransform toCopy)
        {
            Log("Creating control surface for: " + surface.name);

            var newSurface = new AeroController.ControlSurfaceTransform
            {
                transform = surface,
                axis = new Vector3(1, 0, 0),
                maxDeflection = toCopy.maxDeflection,
                actuatorSpeed = toCopy.actuatorSpeed,
                pitchFactor = 1f,
                rollFactor = toCopy.rollFactor,
                yawFactor = toCopy.yawFactor,
                oneDirectional = toCopy.oneDirectional,
                aoaLimit = toCopy.aoaLimit,
                brakeFactor = toCopy.brakeFactor,
                trim = toCopy.trim,
                flapsFactor = toCopy.flapsFactor
            };

            controller.controlSurfaces = AddItemToArray(controller.controlSurfaces, newSurface);
            controller.controlSurfaces[controller.controlSurfaces.Length - 1].Init();
            controller.OnEnable();
        }

        private static AeroController.ControlSurfaceTransform[] AddItemToArray(
            AeroController.ControlSurfaceTransform[] original, AeroController.ControlSurfaceTransform itemToAdd)
        {
            Log("Adding item to array: " + itemToAdd.transform.name);

            var finalArray =
                new AeroController.ControlSurfaceTransform[original.Length + 1];

            original.CopyTo(finalArray, 0);

            finalArray[finalArray.Length - 1] = itemToAdd;

            return finalArray;
        }

        private static GameObject _fakeLeftCan;
        private static GameObject _fakeRightCan;
    }
}