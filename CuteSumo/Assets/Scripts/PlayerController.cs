using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	Player player;

	Vector2 mouseDownPos, launchVector;
	bool isMouseDown = false;
	LevelManager levelManager;
	
	void Start () {
		levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
	}
	
	Vector2 MousePosition(){
		Vector3 pos = Input.mousePosition;
		pos.z = -10;
		return Camera.main.ScreenToWorldPoint(pos);
	}
	
	protected void OnMouseDown()
	{
		mouseDownPos = MousePosition();
		isMouseDown = true;
		player.StopMoving();
	}
	
	protected void OnMouseOver()
	{
		if (isMouseDown)
			player.AimTo(MousePosition() - mouseDownPos);
	}
	
	protected void OnMouseUp()
	{
		player.StartMoving();
		isMouseDown = false;
		player.LaunchTo(MousePosition() - mouseDownPos);
	}
}
