using UnityEngine;
using System.Collections;

//////////////////Describes bags on tree in Level 2 this script used between bag gameobject and water gameobject

public class BagLogic : MonoBehaviour {

    public GameObject Splash; //watersplash particle if bag will collided with water
    public bool Water=false; //splitting script usage between water and bag
    public AudioClip Rope; //brocken rope sound

    void OnTriggerEnter(Collider other)
    {
        if (!Water)
        if (other.CompareTag("Item")) //Item holder placed in the Eagle root bone
        {
                GetComponent<AudioSource>().PlayOneShot(Rope, 2); //play sound of cracked rope
                gameObject.GetComponent<SimpleRotation>().enabled = true; //fake animated action
            transform.parent.gameObject.GetComponent<Animation>().Play(); //play rope animation
            Destroy(other.gameObject); //destroy item
            gameObject.GetComponent<Rigidbody>().isKinematic = false; //disable kinematic physics for free fly bag down
                Camera.main.gameObject.GetComponent<GameLogic>().BagsLeft -= 1; //send to gamelogic script the bag was down
                for (int i=0;i< Camera.main.gameObject.GetComponent<GameLogic>().TapMessages.Length;i++)
                Camera.main.gameObject.GetComponent<GameLogic>().TapMessages[i].SetActive(false); //tap messages interface
                Destroy(gameObject, 20);//destroy kicked bag after 20 seconds
            }

        if (Water)
        if (other.CompareTag("Bag")) //If bag collides with water we play Splash effect
        {
                other.gameObject.GetComponent<BagLogic>().Splash.SetActive(true); //play splash particle
                other.gameObject.GetComponent<AudioSource>().Play(); //play splash sound
                other.gameObject.GetComponent<Collider>().isTrigger = false; //remove trigger
                other.gameObject.GetComponent<SimpleRotation>().enabled = false; //disable simplerotation script on bag
            }
    }
}
