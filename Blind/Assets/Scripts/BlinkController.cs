using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlinkController : MonoBehaviour {

	Renderer renderer;

	void Start () {
		renderer = GetComponent<Renderer>();
		StartCoroutine(Blink());
	}
	
	IEnumerator Blink(){
		while (true) {
			renderer.material.DOFade(0.3f, 0.5f);
			renderer.material.DOFade(0f, 0.5f).SetDelay(0.5f);
			yield return new WaitForSeconds(1f);
		}
	}

}
