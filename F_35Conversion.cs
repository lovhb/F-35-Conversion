using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace F_35_Conversion
{
    class F_35Conversion : VTOLMOD
    {
        private Transform leftCanard, rightCanard, leftCanardAero, rightCanardAero;

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

                //canardLeft.pitchFactor = 0.5f;
                canardLeft.axis = new Vector3(-1, 0, 0);

                //canardRight.pitchFactor = 0.5f;
                canardRight.axis = new Vector3(-1, 0, 0);
                //canardRight.brakeFactor = -0.08f;
                //canardRight.flapsFactor = -0.15f;
                //canardRight.AoAFactor = 0;

            }

            leftCanardAero = currentVehicle.transform.Find("sevtf_layer_2/CanardLeftPart/canardLeft/canardLeft_model/leftCanardAero");
            rightCanardAero = currentVehicle.transform.Find("sevtf_layer_2/CanardRightPart/canardRight/canardRight_model/rightCanardAero");

            Wing leftWing = leftCanardAero.GetComponent<Wing>();
            Wing rightWing = rightCanardAero.GetComponent<Wing>();

            //leftWing.liftCoefficient = -0.26f;
            //rightWing.liftCoefficient = -0.26f;
        }
    }
}
