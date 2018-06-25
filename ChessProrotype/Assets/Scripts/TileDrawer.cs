using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileDrawer: MonoBehaviour {
	
	//Скрипт занимается отрисовкой информации, полученной из tileInfo, и обрабатывает события мыши.
	//Если тайлу надо взаимодействовать с другими тайлами, то он вызывает public методы BoardManager.
	[SerializeField]
	Sprite[] whiteSprites, blackSprites;
	[SerializeField]
	Image figureImage;
	
	[HideInInspector]
	public TileInfo tileInfo; //Здесь скрипт получает информацию о тайле и фигуре на тайле и отражает ее на экране.
	
	Image tileImage;
	bool entered = false;
	
	void Start(){
		figureImage.sprite = whiteSprites[0];
		tileImage = GetComponent<Image>();
	}
	
	public void OnPointerClick()
	{
		BoardManager.SelectFigure(ref tileInfo);
	}
	
	public void OnPointerEnter()
	{
		entered = true;
	}
	
	public void OnPointerExit()
	{
		entered = false;
	}
	
	void Draw(){
		if (tileInfo.tileIsWhite) tileImage.color = Color.white;
		else tileImage.color = Color.gray;
		
		if (tileInfo.figureIsWhite) figureImage.sprite = whiteSprites[(int) tileInfo.type];
		else figureImage.sprite = blackSprites[(int) tileInfo.type];
		
		if (entered) tileImage.color = Color.yellow;
		
		if (tileInfo.type != FigureType.empty && tileInfo.selection == SelectionType.selected) 
			tileImage.color = Color.green;
	}
	
	void Update () {	
		Draw();
	}
}
