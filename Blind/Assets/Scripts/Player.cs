using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField]
	float speed;
	[SerializeField]
	GameObject camera, cameraParent;

	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	void Update(){
		Vector3 vel = camera.transform.forward * speed;
		rb.velocity = new Vector3 (vel.x, rb.velocity.y, vel.z);
		cameraParent.transform.position = transform.position;
	}
}
