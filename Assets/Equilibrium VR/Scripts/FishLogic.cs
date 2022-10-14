using UnityEngine;
using System.Collections;

//////////////////////////////This script must be using between fish zone spawn in hierarchy and fish prefab

public class FishLogic : MonoBehaviour {

    public bool FishZone = false;
    public Transform[] FishPrefab;
    private float Randx, Randz;
    public AudioClip GetFish;
    private int PrefNum = 0;
    public Transform TargetView;
    public float speedval = 0;
    public Transform FishZoneTransform;
    public int fishCount = 0;
    public GameObject Canv; //for non functionality getting the fish

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemHolder")) //If eagle head touches with water
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().GotFish();
            GameObject.Find("GameManager1").GetComponent<GameLogic>().FishCount += 1;
            FishZoneTransform.gameObject.GetComponent<FishLogic>().fishCount -= 1;
            Destroy(gameObject);
        }
    }

    public void GenerateFish() //fish generation
    {
        if (FishZone) //if fish zone
        for (int i = 0; i < 4; i++) //generate 4 fishes
        {
            Randx = Random.Range(50,100);
            Randz = Random.Range(50, 100);
                PrefNum = Random.Range(0, 2); //gold or white fish
                fishCount += 1;
                Transform tempFish = Instantiate(FishPrefab[PrefNum], new Vector3(transform.position.x + Randx, transform.position.y, transform.position.z + Randz), transform.rotation) as Transform; //closest position in the fish zone
                tempFish.gameObject.GetComponent<FishLogic>().TargetView = TargetView;
                tempFish.gameObject.GetComponent<FishLogic>().speedval = Random.Range(1, 10); //speed of fish
                tempFish.gameObject.GetComponent<FishLogic>().FishZoneTransform = transform;
        }

    }

    void Update()
    {
        if (fishCount <= 2)
        {
            GenerateFish();
        }
    }

    void LateUpdate()
    {
        if (!FishZone) //if fish
        {
            float Dist = Vector3.Distance(transform.position+new Vector3(speedval*2, 0, speedval*2), TargetView.transform.position);

            float speed = Dist / speedval;

            transform.position += transform.forward * speed * Time.deltaTime;
            var targetRotation = Quaternion.LookRotation(TargetView.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
        }
    }
}
