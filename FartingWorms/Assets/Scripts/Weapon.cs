using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public void TakeUp(int owner)
    {
        transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Isolated");
        for (int i = 1; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.layer = 8 + owner;
    }

    public void Drop()
    {
        transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");
        for (int i = 1; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Floor");
    }

    public void TakeOff()
    {

    }

	void Start () {
		
	}
	
	void Update () {
		
	}
}
