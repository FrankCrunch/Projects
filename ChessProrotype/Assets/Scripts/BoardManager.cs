using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
	//Скрипт создает тайлы, которые отрисовывают поле, и больше их не трогает.
	//Управление полем происходит с помощью изменения массива tilesInfo.

	public TileDrawer tileDrawer; //Префаб тайла, копии которого мы создаем.
	
	[HideInInspector]
	public TileInfo[,] tilesInfo; //Изменяя этот массив, изменяем поле. Каждый тайл имеет ссылку на информацию о себе в этом массиве.
	
	[HideInInspector]
	public static BoardManager instance;
	
	int selectedRow = -1, selectedCol = -1;
	List<TileInfo> savedMoves;
	
	void Awake(){
		instance = this;
	}
	
	void Start(){
		savedMoves = new List<TileInfo>();
		
		//Создадим поле
		tilesInfo = new TileInfo[8, 8];
		
		//Расставим фигуры
		tilesInfo[0, 0] = new TileInfo(FigureType.king, true, 0, 0);
		tilesInfo[1, 0] = new TileInfo(FigureType.knight, true, 1, 0);
		tilesInfo[2, 0] = new TileInfo(FigureType.bishop, true, 2, 0);
		tilesInfo[3, 0] = new TileInfo(FigureType.queen, true, 3, 0);
		tilesInfo[4, 0] = new TileInfo(FigureType.king, true, 4, 0);
		tilesInfo[5, 0] = new TileInfo(FigureType.bishop, true, 5, 0);
		tilesInfo[6, 0] = new TileInfo(FigureType.knight, true, 6, 0);
		tilesInfo[7, 0] = new TileInfo(FigureType.rooks, true, 7, 0);
		for (int i = 0; i < 8; i++)
			tilesInfo[i, 1] = new TileInfo(FigureType.pawn, true, i, 1);
			
		tilesInfo[0, 7] = new TileInfo(FigureType.rooks, false, 0, 7);
		tilesInfo[1, 7] = new TileInfo(FigureType.knight, false, 1, 7);
		tilesInfo[2, 7] = new TileInfo(FigureType.bishop, false, 2, 7);
		tilesInfo[3, 7] = new TileInfo(FigureType.queen, false, 3, 7);
		tilesInfo[4, 7] = new TileInfo(FigureType.king, false, 4, 7);
		tilesInfo[5, 7] = new TileInfo(FigureType.bishop, false, 5, 7);
		tilesInfo[6, 7] = new TileInfo(FigureType.knight, false, 6, 7);
		tilesInfo[7, 7] = new TileInfo(FigureType.rooks, false, 7, 7);
		for (int i = 0; i < 8; i++)
			tilesInfo[i, 6] = new TileInfo(FigureType.pawn, false, i, 6);
			
		for (int i = 0; i < 8; i++)
			for (int j = 2; j < 6; j++)
				tilesInfo[i, j] = new TileInfo(FigureType.empty, false, i, j); 
		
		//Покрасим тайлы
		bool isWhite = true;
		for (int j = 0; j < 8; j++){
			isWhite = !isWhite;
			for (int i = 0; i < 8; i++){
				tilesInfo[i, j].tileIsWhite = isWhite;
				isWhite = !isWhite;
			}
		}
				
		//Создадим тайлы, которые будут отрисовывать поле
		RectTransform rt = tileDrawer.GetComponent<RectTransform>();
		float width = rt.rect.width;
		float height = rt.rect.height;
		for (int i = 0; i < 8; i++){
			for (int j = 0; j < 8; j++){
				TileDrawer tile = Instantiate(tileDrawer, 
					new Vector3((i - 3.5f) * width, (j - 3.5f) * height, 0), Quaternion.identity);
				tile.transform.SetParent(gameObject.transform, false);
				
				//Дадим тайлу ссылку на информацию, которая касается только его.
				tile.tileInfo = tilesInfo[i, j];
			}
			
		}
	}
	
	void SaveTileInfo(TileInfo info){
		savedMoves.Add(new TileInfo(info.type, info.figureIsWhite, info.col, info.row));
	}
	
	//Метод проводит выделение тайла или его перемещение. При этом сохраняются изменения на поле.
	public void SelectFigure(int col, int row){
		if (selectedCol != -1 && (selectedCol != col || selectedRow != row)) {
			TileInfo selectedTile = tilesInfo[selectedCol, selectedRow];
			TileInfo destTile = tilesInfo[col, row];
			
			SaveTileInfo(selectedTile);
			SaveTileInfo(destTile);
			
			selectedTile.selection = SelectionType.idle;
			selectedCol = selectedRow = -1;
			
			destTile.type = selectedTile.type;
			destTile.figureIsWhite = selectedTile.figureIsWhite;
			
			selectedTile.type = FigureType.empty;
		} 
		else if (tilesInfo[col, row].type != FigureType.empty && (selectedCol != col || selectedRow != row)){
			tilesInfo[col, row].selection = SelectionType.selected;
			selectedCol = col;
			selectedRow = row;
		} 
		else if (selectedCol == col || selectedRow == row) {
			tilesInfo[selectedCol, selectedRow].selection = SelectionType.idle;
			selectedCol = selectedRow = -1;
		}
	}
	
	public void UndoMove(){
		if (savedMoves.Count > 0) {
			for (int i = savedMoves.Count - 1; i >= savedMoves.Count - 2; i--){
				TileInfo info = savedMoves[i];
				tilesInfo[info.col, info.row].type = info.type;
				tilesInfo[info.col, info.row].figureIsWhite = info.figureIsWhite;
			}
			savedMoves.RemoveRange(savedMoves.Count - 2, 2);
		}
	}
	
	public void Restart(){
		for (int i = 0; i < savedMoves.Count; i++){
			TileInfo info = savedMoves[i];
			tilesInfo[info.col, info.row].type = info.type;
			tilesInfo[info.col, info.row].figureIsWhite = info.figureIsWhite;
		}
		savedMoves.Clear();
	}
	
	void Update () {
		
	}
} 
