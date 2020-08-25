using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace F_35_Conversion
{
    // Todo: Improve F-35 model
    class F_35Conversion : VTOLMOD
    {
        private Transform leftCanard, rightCanard;

        private void Awake()
        {
            VTOLAPI.SceneLoaded += Loaded;
            VTOLAPI.MissionReloaded += Reloaded;
        }

        private void Reloaded()
        {
            Loaded(VTOLAPI.currentScene);
        }

        private IEnumerator WaitForScenarioReady()
        {
            while (VTMapManager.fetch == null || !VTMapManager.fetch.scenarioReady)
            {
                yield return null;
            }

            Convert();
        }

        private void Loaded(VTOLScenes scene)
        {
            if (VTOLAPI.GetPlayersVehicleEnum() == VTOLVehicles.F45A)
            {
                switch (scene)
                {
                    case VTOLScenes.Akutan:
                    case VTOLScenes.CustomMapBase:
                        StartCoroutine(WaitForScenarioReady());
                        break;
                }
            }
        }

        private void Convert()
        {
            GameObject currentVehicle = VTOLAPI.GetPlayersVehicleGameObject();
            MoveCanards(currentVehicle);
            ModifyCanardRotation(currentVehicle);
        }

        private void MoveCanards(GameObject currentVehicle)
        {

            leftCanard = currentVehicle.transform.Find("sevtf_layer_2/CanardLeftPart");
            rightCanard = currentVehicle.transform.Find("sevtf_layer_2/CanardRightPart");

            leftCanard.localPosition = new Vector3(1.01f, 1.12f, 8.16f);
            leftCanard.Rotate(0f, 0f, 4.679f, Space.Self);
            leftCanard.localScale = new Vector3(1.615172f, 1.488211f, 1f);

            rightCanard.localPosition = new Vector3(-1.01f, 1.12f, 8.16f);
            rightCanard.Rotate(0f, 0f, -4.679f, Space.Self);
            rightCanard.localScale = new Vector3(1.615172f, 1.488211f, 1f);
        }

        private void ModifyCanardRotation(GameObject currentVehicle)
        {
            AeroController aeroController = currentVehicle.GetComponent<AeroController>();
            if (aeroController != null)
            {
                AeroController.ControlSurfaceTransform canardLeft = aeroController.controlSurfaces[0];
                AeroController.ControlSurfaceTransform canardRight = aeroController.controlSurfaces[1];

                canardLeft.pitchFactor /= -1f;
                canardLeft.brakeFactor /= -1f;
                canardLeft.flapsFactor /= -1f;

                canardRight.pitchFactor /= -1f;
                canardRight.brakeFactor /= -1f;
                canardRight.flapsFactor /= -1f;
            }
        }
    }
}
