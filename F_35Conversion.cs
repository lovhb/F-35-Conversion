using UnityEngine;
using UnityEngine.SceneManagement;

namespace F_35_Conversion
{
    // Todo: Add postpatch to VTMapManager.RestartCurrentScenario to keep modded F-45 after mission restart
    // Todo: Improve F-35 model
    class F_35Conversion : VTOLMOD
    {
        private VTOLAPI api;
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

        private void Loaded(VTOLScenes scene)
        {
            if (VTOLAPI.GetPlayersVehicleEnum() == VTOLVehicles.F45A)
            {
                switch (scene)
                {
                    case VTOLScenes.Akutan:
                    case VTOLScenes.CustomMapBase:
                        Convert();
                        break;
                }
            }
            if ((sceneName == "CustomMapBase" || sceneName == "Akutan") && VTOLAPI.GetPlayersVehicleEnum() == VTOLVehicles.F45A)
            {
                StartCoroutine(waiter());
            }
        }

        private void Convert()
        {
            if ()
            {

                GameObject currentVehicle = VTOLAPI.GetPlayersVehicleGameObject();

                leftCanard = currentVehicle.transform.Find("sevtf_layer_2/CanardLeftPart");
                rightCanard = currentVehicle.transform.Find("sevtf_layer_2/CanardRightPart");

                leftCanard.localPosition = new Vector3(0.855f, 1.12f, 7.907f);
                leftCanard.Rotate(0f, 6.03f, 0f, Space.Self);

                rightCanard.localPosition = new Vector3(-0.855f, 1.12f, 7.907f);
                rightCanard.Rotate(0f, -6.03f, 0f, Space.Self);

                leftCanard.Find("canardLeft").localPosition = new Vector3(0f, -0.926f, 0f);
                rightCanard.Find("canardRight").localPosition = new Vector3(0f, -0.926f, 0f);
            }
        }
    }
}
