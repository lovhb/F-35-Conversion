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
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            string sceneName = arg0.name;
            if ((sceneName == "CustomMapBase" || sceneName == "Akutan") && VTOLAPI.GetPlayersVehicleEnum() == VTOLVehicles.F45A)
            {
                StartCoroutine(waiter());
            }
        }

        private void convert()
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
