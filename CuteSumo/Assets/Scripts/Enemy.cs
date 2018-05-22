using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	[SerializeField]
	float timeToLand, landRadius, landImpulse;
	[SerializeField]
	GameObject body;
	
	Rigidbody2D rb;
	LineRenderer line;
	
	bool justLanded;
	
	public void ChangeLandImpulse(Text text){
		landImpulse = float.Parse(text.text);
	}
	
	public void ChangeLandTime(Text text){
		timeToLand = float.Parse(text.text);
	}
	
	public void ChangeFriction(Text text){
		rb.drag = float.Parse(text.text);
	}
	
	void Start(){
		rb = GetComponent<Rigidbody2D>();
		line = GetComponent<LineRenderer>();
		StartCoroutine(WaitAndLand());
	}
	
	void DrawCircle(Vector2 center, float radius){
		float theta = 0f;
		float thetaScale = 0.01f;
		int size = (int)((1f / thetaScale) + 1f);
		line.SetVertexCount(size); 
		for(int i = 0; i < size; i++){          
			theta += (2.0f * Mathf.PI * thetaScale);         
			float x = center.x + radius * Mathf.Cos(theta);
			float y = center.y + radius * Mathf.Sin(theta);          
			line.SetPosition(i, new Vector3(x, y, 0));
		}
	}
	
	IEnumerator WaitAndLand(){
		for (int i = 0; i < 40; i++){
			DrawCircle(transform.position, landRadius*i/40);
			yield return new WaitForSeconds(timeToLand / 40);
		}
		
		body.SetActive(true);
		line.SetVertexCount(0);
		justLanded = true;
		
		yield return new WaitForSeconds(0.1f);
		justLanded = false;
	}
	
	void OnCollisionEnter2D(Collision2D collisionInfo)
	{
		if (collisionInfo.gameObject.tag == "Borders"){
			Destroy(gameObject);
		}
		
		if (justLanded){
			Vector2 direction = collisionInfo.gameObject.transform.position - transform.position;
			direction.Normalize();
			collisionInfo.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * landImpulse, ForceMode2D.Impulse);
		}
	}
	
	void FixedUpdate()
	{
	
	}
}
