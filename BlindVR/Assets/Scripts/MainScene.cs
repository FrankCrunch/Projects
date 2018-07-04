using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour {
	
	IEnumerator WaitAndEndScene(){
		yield return new WaitForSeconds(2f);
		EndScene();
	}
	
	public void PointerEnter(){
		StartCoroutine(WaitAndEndScene());
	}
	
	public void PointerExit(){
		StopAllCoroutines();
	}
	
	public void EndScene(){
		#if UNITY_EDITOR
		// Application.Quit() does not work in the editor so
		// UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
		UnityEditor.EditorApplication.isPlaying = false;
        #else
		   Application.Quit();
        #endif
	}
}
