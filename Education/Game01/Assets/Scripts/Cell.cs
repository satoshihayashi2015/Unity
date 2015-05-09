using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown(){
		
		//		// Change to Sprite 0
		//		Sprite sp = GetSprite("Images/SpriteSample", "SpriteSample_0");
		//
		//		// SpriteRendererを取得する
		//		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		//
		//		// Spriteを変更する
		//		sr.sprite = sp;
		
		print (name);
		
		string[] pos = name.Replace ("cell", "").Split (new string[]{"_"}, System.StringSplitOptions.RemoveEmptyEntries);
		if (pos.Length != 2) {return;}
		
		string rowStr = pos[0];
		string colStr = pos[1];
		int row = 0;
		int col = 0;
		
		if (!int.TryParse (rowStr, out row)) {return;}
		if (!int.TryParse (colStr, out col)) {return;}
		
		TrySet (row,col);
	}
	
	
	// スプライトの取得
	// @param fileName ファイル名
	// @param spriteName スプライト名
	public Sprite GetSprite(string fileName, string spriteName) {
		Sprite[] sprites = Resources.LoadAll<Sprite>(fileName);
		return System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(spriteName));
	}
	
	private void TrySet(int row, int col){
		
		if (Manager.fieldState [row, col] != Manager.CellStateType.FIELD) {return;}
		if (!CanSandwich(row,col)) {return;}
		
		switch (Manager.gameMode) {
		case Manager.GameMode.WHITE:
			Manager.fieldState [row, col] = Manager.CellStateType.WHITE;
			Manager.gameMode = Manager.GameMode.BLACK;
			break;
		case Manager.GameMode.BLACK:
			Manager.fieldState [row, col] = Manager.CellStateType.BLACK;
			Manager.gameMode = Manager.GameMode.WHITE;
			break;
		}
		
		Manager.DrawFieldS ();
	}
	
	private bool CanSandwich(int row, int col){
		
		Manager.CellStateType player_type = (Manager.gameMode == Manager.GameMode.BLACK) ? Manager.CellStateType.BLACK : Manager.CellStateType.WHITE;
		Manager.CellStateType enemy_type = (Manager.gameMode == Manager.GameMode.BLACK) ? Manager.CellStateType.WHITE : Manager.CellStateType.BLACK; 
		
		int current_row = row;
		int current_col = col;
		int get_enemy_count = 0;
		
		//north
		while(current_row>0){
			current_row--;
			if (Manager.fieldState[current_row,current_col]==enemy_type) {
				get_enemy_count++;
			}
			else if(Manager.fieldState[current_row,current_col]==player_type && get_enemy_count>0) {
				return true;
			}
			else {
				break;
			}
		}
		
		//reset
		current_row = row;
		current_col = col;
		get_enemy_count = 0;
		
		//east
		while(current_col<Manager.COL_COUNT-1){
			current_col++;
			if (Manager.fieldState[current_row,current_col]==enemy_type) {
				get_enemy_count++;
			}
			else if(Manager.fieldState[current_row,current_col]==player_type && get_enemy_count>0) {
				return true;
			}
			else {
				break;
			}
		}
		
		//reset
		current_row = row;
		current_col = col;
		get_enemy_count = 0;
		
		//south
		while(current_row<Manager.ROW_COUNT-1){
			current_row++;
			if (Manager.fieldState[current_row,current_col]==enemy_type) {
				get_enemy_count++;
			}
			else if(Manager.fieldState[current_row,current_col]==player_type && get_enemy_count>0) {
				return true;
			}
			else {
				break;
			}
		}
		
		//reset
		current_row = row;
		current_col = col;
		get_enemy_count = 0;
		
		//west
		while(current_col>0){
			current_col--;
			if (Manager.fieldState[current_row,current_col]==enemy_type) {
				get_enemy_count++;
			}
			else if(Manager.fieldState[current_row,current_col]==player_type && get_enemy_count>0) {
				return true;
			}
			else {
				break;
			}
		}
		
		//reset
		current_row = row;
		current_col = col;
		get_enemy_count = 0;
		
		//north west
		while (current_row>0) {
			current_row--;
			while (current_col>0) {
				current_col--;
				if (Manager.fieldState [current_row, current_col] == enemy_type) {
					get_enemy_count++;
				} else if (Manager.fieldState [current_row, current_col] == player_type && get_enemy_count > 0) {
					return true;
				} else {
					break;
				}
			}
		}
		
		//reset
		current_row = row;
		current_col = col;
		get_enemy_count = 0;
		
		//north east
		while (current_row>0) {
			current_row--;
			while (current_col<Manager.COL_COUNT-1) {
				current_col++;
				if (Manager.fieldState [current_row, current_col] == enemy_type) {
					get_enemy_count++;
				} else if (Manager.fieldState [current_row, current_col] == player_type && get_enemy_count > 0) {
					return true;
				} else {
					break;
				}
			}
		}
		
		//reset
		current_row = row;
		current_col = col;
		get_enemy_count = 0;
		
		//south west
		while (current_row<Manager.ROW_COUNT-1) {
			current_row++;
			while (current_col>0) {
				current_col--;
				if (Manager.fieldState [current_row, current_col] == enemy_type) {
					get_enemy_count++;
				} else if (Manager.fieldState [current_row, current_col] == player_type && get_enemy_count > 0) {
					return true;
				} else {
					break;
				}
			}
		}
		
		//reset
		current_row = row;
		current_col = col;
		get_enemy_count = 0;
		
		//south east
		while (current_row<Manager.ROW_COUNT-1) {
			current_row++;
			while (current_col<Manager.COL_COUNT-1) {
				current_col++;
				if (Manager.fieldState [current_row, current_col] == enemy_type) {
					get_enemy_count++;
				} else if (Manager.fieldState [current_row, current_col] == player_type && get_enemy_count > 0) {
					return true;
				} else {
					break;
				}
			}
		}
		
		return false;
	}
	
}
