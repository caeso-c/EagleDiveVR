using UnityEngine;
using System.Collections;

public class PickItem : MonoBehaviour {

    public Transform Item; //Attach transform or prefab item to this variable and script must be placed on item zones

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemHolder")) //Item holder placed in the Eagle root bone
        {
            other.gameObject.GetComponent<EagleItemSpawner>().SpawnItem(Item);
        }
    }
}
