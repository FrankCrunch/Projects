using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spider : MonoBehaviour {
	
	[SerializeField]
	ParticleSystem ps;

	void Start () {
		StartCoroutine(RepeatScreaming());
	}
	
	IEnumerator RepeatScreaming(){
		while (true){	
			SoundManager.PlaySpiderSound();
			for (int i = 0; i < 6; i++){
				ps.Emit(100);
				yield return new WaitForSeconds(2f);
			}
		}
	}
}
