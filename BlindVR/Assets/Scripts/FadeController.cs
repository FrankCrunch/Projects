﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeController : MonoBehaviour{
	
	Renderer renderer;

	void Start () {
		renderer = GetComponent<Renderer>();
		renderer.material.DOFade(0, 0f);
	}
	
	void OnParticleCollision(GameObject other)
	{
		if (other.gameObject.tag == "Stick") {
			renderer.material.DOFade(1, 1f);
			renderer.material.DOFade(0, 3f).SetDelay(1f);
		}	
	}
}
