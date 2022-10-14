using UnityEngine;
 using System.Collections;
 
 public class Checkpoint : MonoBehaviour 
 {
    //Using for text mesh look at player camera
     private GameObject target;
     private Vector3 targetPoint;
     private Quaternion targetRotation;
 
     void Start () 
     {
         target = GameObject.Find("/GameManager1").GetComponent<GameLogic>().lookTargetObj;
     }
 
     void Update()
     {
        if (target != null)
        {
            targetPoint = target.transform.position - transform.position;
            targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
        }
        else
        {
			target = GameObject.Find("/GameManager1").GetComponent<GameLogic>().lookTargetObj;
        } 

     }
 }