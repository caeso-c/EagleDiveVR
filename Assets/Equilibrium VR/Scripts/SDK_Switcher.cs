using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDK_Switcher : MonoBehaviour {

    public GameObject manager_PC_sdk;
    public GameObject manager_VReditor_sdk;
    bool started=false;

	// Use this for initialization
	void Update ()
    {
        if(GameObject.Find("[VRTK_SDKManager]/SDKSetups/Simulator").activeInHierarchy && !started)
        {
            manager_PC_sdk.name = "GameManager";
            manager_VReditor_sdk.name = "GameManager_UNUSED";
            manager_VReditor_sdk.SetActive(false);
            Debug.Log("This should only print if No VR device is present");
            started = true;

        }
        else if (GameObject.Find("[VRTK_SDKManager]/SDKSetups/SteamVR").activeInHierarchy && !started)
        {
            manager_PC_sdk.name = "GameManager_UNUSED";
            manager_VReditor_sdk.name = "GameManager";
            manager_PC_sdk.SetActive(false);
            Debug.Log("This should only print if VR device is present");
            started = true;
        }
        else if (!started)
        {

            Debug.Log("Can't find a valid device!");
        }


    }
	

}
