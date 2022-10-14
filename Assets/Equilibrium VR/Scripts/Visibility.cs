using UnityEngine;
using System.Collections;

public class Visibility : MonoBehaviour {

    public bool visible=false; //Show or hide gameObject object in game

	// Use this for initialization
	void Start () {

        if (!visible) GetComponent< MeshRenderer > ().enabled = false;
        if (visible) GetComponent< MeshRenderer > ().enabled = true;

    }
}
