using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//////////////////////////////pick item by eagle and shooting with eagle of player with tap or mouse button

public class EagleItemSpawner : MonoBehaviour {

    public Transform clone;
    public bool gotItem = false;
    public float CurrentForce, ChargeForce, MaxForce;
    public Image PowerBar; //Image source
    private Image pwrB; //Image component
    private bool capSound = false; //Play once dropdown sound fx
    private bool Mes = true;

    void Start()
    {
        pwrB = PowerBar.GetComponent<Image>();
    }

    public void SpawnItem( Transform Item) //get item from Pick Item script
    {
        clone = Instantiate(Item, transform.position, transform.rotation) as Transform;
        clone.parent = transform;
        clone.gameObject.GetComponent<Collider>().enabled = false;
        gotItem = true;
        Camera.main.gameObject.GetComponent<GameLogic>().gotItem = gotItem;
        if (Mes)
        {
            for (int i = 0; i < Camera.main.gameObject.GetComponent<GameLogic>().TapMessages.Length; i++)
                Camera.main.gameObject.GetComponent<GameLogic>().TapMessages[i].SetActive(true); //showing tap messages on bags
            Mes = false;
        }
    }
	
	void Update () {
        if (gotItem)
        {
            if (Input.GetMouseButton(0))
            {
                if (!capSound)
                {
                    //Camera.main.gameObject.GetComponent<AudioSource>().PlayOneShot(Camera.main.gameObject.GetComponent<GameLogic>().SFX[0], 1.5f);
                    //Camera.main.gameObject.GetComponent<GameLogic>().MusicHolder.GetComponent<AudioSource>().volume = 0.3f;
                    capSound = true;
                }
                Time.timeScale = 0.2f;
                
                CurrentForce += ChargeForce*Time.deltaTime; 

                if (CurrentForce >= MaxForce)
                    CurrentForce = 0;

                PowerBar.gameObject.SetActive(true);
                pwrB.fillAmount = CurrentForce / MaxForce;
            }
                if (Input.GetMouseButtonUp(0))
            {
                Time.timeScale = 1;
                //Camera.main.gameObject.GetComponent<GameLogic>().MusicHolder.GetComponent<Animation>().Play("FadeInMusic");
                //Camera.main.gameObject.GetComponent<AudioSource>().PlayOneShot(Camera.main.gameObject.GetComponent<GameLogic>().SFX[1], 1.5f);
                capSound = false;
                PowerBar.gameObject.SetActive(false);
                gotItem = false;
                Camera.main.gameObject.GetComponent<GameLogic>().gotItem = gotItem;
                clone.gameObject.GetComponent<Collider>().enabled = true;
                clone.parent = null;
                clone.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                clone.gameObject.GetComponent<Rigidbody>().AddForce(clone.transform.forward * CurrentForce);
                clone.gameObject.GetComponent<SimpleRotation>().enabled = true;
                Destroy(clone.gameObject, 5);
                CurrentForce = 0;
            }
        }
	}

}
