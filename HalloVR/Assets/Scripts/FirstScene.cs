using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FirstScene : MonoBehaviour {

	[SerializeField]
	InputField fullName, email;
	
	void Awake(){
		UnityEngine.XR.XRSettings.enabled = false;
	}
	
	void Start(){
		UnityEngine.XR.XRSettings.enabled = false;
		fullName.text = PlayerPrefs.GetString("FullName");
		email.text = PlayerPrefs.GetString("Email");
	}

	public void PressingStart(){
		PlayerPrefs.SetString("FullName", fullName.text);
		PlayerPrefs.SetString("Email", email.text);
		UnityEngine.XR.XRSettings.enabled = true;
		SceneManager.LoadScene("Main");
	}
}
