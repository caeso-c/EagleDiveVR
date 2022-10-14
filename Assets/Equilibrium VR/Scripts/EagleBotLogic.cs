using UnityEngine;
using System.Collections;

public class EagleBotLogic : MonoBehaviour {

    ///////////Describes behavior of eaglebots when 5th battle gamemode

    public GameObject Eagle; //eaglebot mesh but not parent handler
    private int Crashes = 0; //How much player kicks this eaglebot
    public int EagleNum = 0; //EagleBot number
    public Material MainMat,NewMat; //mainmat and newmat is for change trail materials define this materials in the inspector for eaglebots
    public GameObject[] Trails; //every Eaglebot Trails
    public bool changeMaterial = false; //value gets from GameLogic
    public AudioClip EagleCry;
    public AudioClip EagleBroken;
    public float eagleSoundTimer = 5.0f;
    public AudioManager gameAudioMgr;

    void Start()
    {
        Eagle.GetComponent<Renderer>().material.EnableKeyword("_EMISSION"); //we enable emmision of standard shader of eaglebot mesh
        gameAudioMgr = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
    	eagleSoundTimer -= Time.deltaTime;

    	if (eagleSoundTimer < 0)
    	{
    		//gameAudioMgr.RndEagleSound(transform.position);
    		eagleSoundTimer = 10.0f;
    	}

        if (changeMaterial) //if 5th mode playing we change eaglebot trail material to red color once
        {
            foreach (GameObject trail in Trails)
                trail.GetComponent<TrailRenderer>().material = NewMat;

            changeMaterial = false;
        }
    }

    void PlayEagleCry()
    {
		
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item")) //Player gets current item with tag
        {
            Eagle.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white); //blink eagle color
            StartCoroutine(DownColor(.2f));
            GetComponent<AudioSource>().PlayOneShot(EagleBroken);
            Crashes += 1;
            if (Crashes >= 1)
            {
                if (EagleNum == 1)
                {
                    GetComponent<BotSpeed>().Target.gameObject.GetComponent<Animation>().CrossFade("FishModeTargetLost1", 1); //eagle flies away
                    Camera.main.gameObject.GetComponent<GameLogic>().EaglesBeated -= 1;
                }
                if (EagleNum == 2)
                {
                    GetComponent<BotSpeed>().Target.gameObject.GetComponent<Animation>().CrossFade("FishModeTargetLost2", 1); //eagle flies away
                    Camera.main.gameObject.GetComponent<GameLogic>().EaglesBeated -= 1;
                }

            }
            Destroy(other.gameObject); //destroy plank item
        }
    }

    IEnumerator DownColor(float T_Color)
    {
        yield return new WaitForSeconds(T_Color);
        Eagle.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        
    }
}
