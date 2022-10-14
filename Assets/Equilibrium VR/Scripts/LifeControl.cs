using UnityEngine;
using System.Collections;

public class LifeControl : MonoBehaviour {

    public AudioClip Fall; //fall sound of eagle
    public ParticleSystem SplashEffect;
    private int cap;

    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain")) //all collided objects needs to be set Terrain tag
        {
            GameObject.Find("GameManager1").GetComponent<GameLogic>().impacted = true;
            GameObject.Find("AudioManager").GetComponent<AudioManager>().HitBoundary();
			StartCoroutine(GameObject.Find("GameManager1").GetComponent<GameLogic>().ResetByFall(0.2F,0));
        }

        if (other.CompareTag("Water")) //if eagle player will collides with water play particle splash effect and splash sound once
        {
            cap += 1;
            SplashEffect.Play();
            if (cap == 2)
            {
                GameObject.Find("AudioManager").GetComponent<AudioManager>().WaterSplash(transform.position);
                cap = 0;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Water")) //all collided objects needs to be set Terrain tag
        {
            SplashEffect.Stop();
        }
    }

}
