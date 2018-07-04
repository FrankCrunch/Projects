using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spider : MonoBehaviour {
	
	[SerializeField]
	Vector3 destination;
	
	Animation animation;

	void Start () {
		animation = GetComponent<Animation>();
		
		transform.DOMove(destination, 4f, false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
