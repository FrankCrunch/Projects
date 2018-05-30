public class TileInfo{
	//Класс содержит информацию о тайле и фигуре на тайле.

	public FigureType type;	
	public bool figureIsWhite, tileIsWhite;
	public int col, row;
	public SelectionType selection;

	public TileInfo (FigureType type, bool figureIsWhite, int col, int row){
		this.type = type;
		this.figureIsWhite = figureIsWhite;
		this.col = col;
		this.row = row;
	}
}

public enum FigureType {empty, king, queen, rooks, bishop, knight, pawn}; 
public enum SelectionType {idle, selected, readyToMove};
