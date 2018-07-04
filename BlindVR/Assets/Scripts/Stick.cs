using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stick : MonoBehaviour {

	Renderer renderer;
	
	void Start () {
		renderer = GetComponent<Renderer>();
	}
	
	void OnTriggerEnter(Collider other)
	{
		renderer.material.color = Color.red;
	}
	
	void OnTriggerExit(Collider other)
	{
		renderer.material.color = Color.white;
	}
}
