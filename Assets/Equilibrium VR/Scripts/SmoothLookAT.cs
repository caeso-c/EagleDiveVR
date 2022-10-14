using UnityEngine;
using System.Collections;

public class SmoothLookAT : MonoBehaviour {

    public GameObject targetObj;
    public int speed = 5;
    public bool EagleBot=false;
    public bool EagleRot = false;
    public int ch_idx = 0;
    private float RandomDirectionX = 0; //EagleBot Direction
    private float RandomDirectionY = 0; //EagleBot Direction
    private float TimeDirection = 0;

	void Start ()
        {
            if(targetObj == null)
                targetObj = GameObject.Find("[VRTK_SDKManager]/SDKSetups/SteamVR/[CameraRig]/Camera (eye)/Target");

            if (EagleBot)
            {
                RandomDirectionX = Random.Range(-20,5);
                RandomDirectionY = Random.Range(-20, 10);
            }

        }
	
	void FixedUpdate () {

        if (!EagleRot)

        {
            if (EagleBot)
            {
                TimeDirection += Time.deltaTime;
                if (TimeDirection >= 1)
                {
                    RandomDirectionX = Random.Range(-20, 15);
                    RandomDirectionY = Random.Range(-20, 10);
                    TimeDirection = 0;
                }
                var targetRotation = Quaternion.LookRotation(targetObj.transform.position - transform.position) * Quaternion.Euler(RandomDirectionX, RandomDirectionY, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            }
            else
            {
                var targetRotation_1 = Quaternion.LookRotation(targetObj.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation_1, speed * Time.deltaTime);
            }
        }

        if(EagleRot)
        {
            Quaternion HeadRotation = targetObj.transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, HeadRotation, speed * Time.deltaTime);
        }



    }
}