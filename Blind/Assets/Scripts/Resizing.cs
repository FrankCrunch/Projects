using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Resizing : MonoBehaviour {
	
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
