using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Resizing : MonoBehaviour {
	
	void Start()
	{
		StartCoroutine(PlaySound());
	}
	
	IEnumerator PlaySound(){
		while (true){	
			SoundManager.PlayTableSound();
			yield return new WaitForSeconds(49f);
		}
	}
	
	void Update()
	{
		SoundManager.ChangeVolume(transform.localScale.x/2);
	}
	
	public void OnPointerEnter()
	{
		DOTween.Kill(this);
		transform.DOScale(2, 3f);
	}
	
	public void OnPointerExit()
	{
		DOTween.Kill(this);
		transform.DOScale(1, 3f);
	}
}
