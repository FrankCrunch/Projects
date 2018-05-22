using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	
	[SerializeField]
	float launchImpulse, maxLaunchLength, friction, stopFriction;
	
	Rigidbody2D rb;
	LineRenderer lineRenderer;

	public void ChangeLaunchImpulse(Text text){
		launchImpulse = float.Parse(text.text);
	}
	
	public void ChangeFriction(Text text){
		rb.drag = float.Parse(text.text);
		friction = rb.drag;
	}
	
	public void ChangeStopFriction(Text text){
		stopFriction = float.Parse(text.text);
	}

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		lineRenderer = GetComponent<LineRenderer>();
		
		rb.drag = friction;
	}
	
	public void AimTo(Vector2 dir){
		Vector2 pos = transform.position;
		lineRenderer.SetPosition(0, pos);
		if (dir.SqrMagnitude() > maxLaunchLength * maxLaunchLength) {
			dir.Normalize();
			dir *= maxLaunchLength;
		}
		lineRenderer.SetPosition(1, pos + dir);
	}
	
	public void LaunchTo(Vector2 dir){
		dir.Normalize();
		rb.AddForce(dir * launchImpulse, ForceMode2D.Impulse);
		
		Vector2 farPoint = new Vector2(1000, 1000);
		lineRenderer.SetPosition(0, farPoint);
		lineRenderer.SetPosition(1, farPoint);
	}
	
	public void StopMoving(){
		rb.drag = stopFriction;
	}
	
	public void StartMoving(){
		rb.drag = friction;
	}
	
	void Update () {
		
	}
}
