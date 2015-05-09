using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	
	public GameObject cellPrefab;
	
	private const int CELL_SIZE = 64;
	private const int FIELD_ROW_COUNT = 8; 
	private const int FIELD_COL_COUNT = 8;
	private const int WALL_COUNT = 1;
	public const int ROW_COUNT = FIELD_ROW_COUNT + (WALL_COUNT * 2);
	public const int COL_COUNT = FIELD_COL_COUNT + (WALL_COUNT * 2);
	
	public static string CELL_NAME_FORMAT = "cell{0}_{1}";
	
	public static GameMode gameMode = GameMode.BLACK;
	public static CellStateType[,] fieldState = new CellStateType[ROW_COUNT, COL_COUNT];
	
	public enum GameMode{
		NONE
		,WHITE
		,BLACK
		,RESULT
	}
	
	public enum CellStateType {
		WALL
		,FIELD
		,WHITE
		,BLACK
		,RI
		,BA
		,A
		,SHI
		,TRANSPARENT
	}
	
	// Use this for initialization
	void Start () {
		CreateField ();
		DrawField ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void CreateField() {
		for (int row = 0; row < ROW_COUNT; row++) {
			for (int col = 0; col < COL_COUNT; col++) {
				GameObject cell = (GameObject)Instantiate(cellPrefab);
				
				//Position Setting
				Vector3 p = cell.transform.position;
				p.y = (float)(-row * CELL_SIZE * 0.01);
				p.x = (float)(+col * CELL_SIZE * 0.01);
				cell.transform.position = p;
				
				cell.name = string.Format(CELL_NAME_FORMAT, row, col);
				
				fieldState[row,col] = CellStateType.FIELD;
				
				//Wall
				if (row==0 ||row==ROW_COUNT-1 || col==0 || col==COL_COUNT-1) {
					fieldState[row,col] = CellStateType.WALL;
				}
				
				//Black and White
				if ((row==4 && col==5) || (row==5 && col==4)) {
					fieldState[row,col] = CellStateType.BLACK;
				}
				if ((row==4 && col==4) || (row==5 && col==5)) {
					fieldState[row,col] = CellStateType.WHITE;
				}
			}
		}
	}
	
	public void DrawField(){
		Manager.DrawFieldS ();
	}
	
	public static void DrawFieldS(){
		for (int row = 0; row < ROW_COUNT; row++) {
			for (int col = 0; col < COL_COUNT; col++) {
				
				string goName = string.Format(CELL_NAME_FORMAT, row, col);
				GameObject go = GameObject.Find(goName);
				
				SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
				string imageName = "SpriteSample_" + (int)fieldState[row,col];
				sr.sprite = GetSprite("Images/SpriteSample", imageName);
			}
		}
	}
	
	void OnMouseDown(){
		
	}
	
	// スプライトの取得
	// @param fileName ファイル名
	// @param spriteName スプライト名
	public static Sprite GetSprite(string fileName, string spriteName) {
		Sprite[] sprites = Resources.LoadAll<Sprite>(fileName);
		return System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(spriteName));
	}
}
