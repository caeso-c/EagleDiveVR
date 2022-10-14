using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

//Turn off from main camera for VR mode

public class CameraRotation : MonoBehaviour {

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    public float yaw = 0.0f;
    public float pitch = 0.0f;


    void Start()
    {

#if UNITY_ANDROID
        Input.gyro.enabled = true; //using gyroscope for non VR mode
#endif

    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

            var mouseInput = Pointer.current;
            if (mouseInput == null)
            {
                Debug.Log("Can't find mouse!");
            }
            else
            {
                float mouseXval = mouseInput.position.x.ReadValue();
                float mouseYval = mouseInput.position.y.ReadValue();
                yaw += speedH * mouseXval;
                pitch -= speedV * mouseYval;
                transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
                Debug.Log("Flying: Yaw:"+yaw+" Pitch:"+pitch);
            }
        
#endif

#if UNITY_ANDROID

            yaw -= speedH * Input.gyro.rotationRateUnbiased.y;
            pitch -= speedV * Input.gyro.rotationRateUnbiased.x;
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
#endif

    }
}
