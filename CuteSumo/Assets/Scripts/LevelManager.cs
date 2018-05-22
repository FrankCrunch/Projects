using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour {

	[SerializeField]
	GameObject enemy;
	[SerializeField]
	float secondsForEnemy;
	[SerializeField]
	TextMeshProUGUI scoreField;
	
	int score = 0;
	List<GameObject> enemies;
	GameObject player;
	
	public void ChangeSecondsForEnemy(Text text){
		secondsForEnemy = float.Parse(text.text);
	}

	void Start () {
		StartCoroutine(WaitAndLandEnemy());
		enemies = new List<GameObject>();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	IEnumerator WaitAndLandEnemy(){
		while (true){
			yield return new WaitForSeconds(secondsForEnemy);
			enemies.Add(Instantiate(enemy, 
				player.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0), 
				Quaternion.identity));
		}
	}
	
	public void IncScore(){
		score++;
		scoreField.text = "Score: " + score;
	}
	
	public void ClearScore(){
		score = 0;
		scoreField.text = "Score: " + score;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Enemy"){
			Destroy(other.transform.parent.gameObject);
			IncScore();
		}
		
		if (other.gameObject.tag == "Player"){
			other.gameObject.transform.position = new Vector2(0,0);
			other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
			ClearScore();
		}
	}
	
	void Update () {
		
	}
}
