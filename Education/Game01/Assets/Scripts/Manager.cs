using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
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

    public const string WHITE_TURN_MESSAGE = "「白の番です。」";
    public const string BLACK_TURN_MESSAGE = "「黒の番です。」";

    public enum GameMode
    {
        NONE
        ,WHITE
        ,BLACK
        ,RESULT
    }

    public enum CellStateType
    {
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
    void Start()
    {
        CreateField();
        DrawField();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateField()
    {
        for (int row = 0; row < ROW_COUNT; row++)
        {
            for (int col = 0; col < COL_COUNT; col++)
            {
                GameObject cell = (GameObject)Instantiate(cellPrefab);

                //Position Setting
                Vector3 p = cell.transform.position;
                p.y = (float)(-row * CELL_SIZE * 0.01);
                p.x = (float)(+col * CELL_SIZE * 0.01);
                cell.transform.position = p;

                cell.name = string.Format(CELL_NAME_FORMAT, row, col);

                fieldState[row, col] = CellStateType.FIELD;

                //Wall
                if (row == 0 || row == ROW_COUNT - 1 || col == 0 || col == COL_COUNT - 1)
                {
                    fieldState[row, col] = CellStateType.WALL;
                }

                //Black and White
                if ((row == 4 && col == 5) || (row == 5 && col == 4))
                {
                    fieldState[row, col] = CellStateType.BLACK;
                }
                if ((row == 4 && col == 4) || (row == 5 && col == 5))
                {
                    fieldState[row, col] = CellStateType.WHITE;
                }
            }
        }

        Message.MessageText = BLACK_TURN_MESSAGE;
    }

    public void DrawField()
    {
        Manager.DrawFieldS();
    }

    public static void DrawFieldS()
    {
        for (int row = 0; row < ROW_COUNT; row++)
        {
            for (int col = 0; col < COL_COUNT; col++)
            {
                string goName = string.Format(CELL_NAME_FORMAT, row, col);
                GameObject go = GameObject.Find(goName);

                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                string imageName = "SpriteSample_" + (int)fieldState[row, col];
                sr.sprite = GetSprite("Images/SpriteSample", imageName);
            }
        }

        //白と黒の個数を出す
        int whiteCount = 0;
        int blackCount = 0;
        for (int rowpos = 0; rowpos < Manager.ROW_COUNT; rowpos++)
        {
            for (int colpos = 0; colpos < Manager.COL_COUNT; colpos++)
            {
                switch (Manager.fieldState[rowpos, colpos])
                {
                    case Manager.CellStateType.WHITE:
                        whiteCount++;
                        break;
                    case Manager.CellStateType.BLACK:
                        blackCount++;
                        break;
                }
            }
        }

        GameObject White1 = GameObject.Find("White1");
        GameObject White10 = GameObject.Find("White10");
        GameObject Black1 = GameObject.Find("Black1");
        GameObject Black10 = GameObject.Find("Black10");

        string W_st = (whiteCount).ToString("00");
        string B_st = (blackCount).ToString("00");

        string w1 = string.Format("SpriteSample_1{0}", W_st.Substring(1, 1));
        string w10 = string.Format("SpriteSample_1{0}", W_st.Substring(0, 1));
        string b1 = string.Format("SpriteSample_1{0}", B_st.Substring(1, 1));
        string b10 = string.Format("SpriteSample_1{0}", B_st.Substring(0, 1));

        White1.GetComponent<SpriteRenderer>().sprite = GetSprite("Images/SpriteSample", w1);
        White10.GetComponent<SpriteRenderer>().sprite = GetSprite("Images/SpriteSample", w10);
        Black1.GetComponent<SpriteRenderer>().sprite = GetSprite("Images/SpriteSample", b1);
        Black10.GetComponent<SpriteRenderer>().sprite = GetSprite("Images/SpriteSample", b10);

    }

    public static void ResetField()
    {
        for (int row = 0; row < ROW_COUNT; row++)
        {
            for (int col = 0; col < COL_COUNT; col++)
            {
                fieldState[row, col] = CellStateType.FIELD;

                //Wall
                if (row == 0 || row == ROW_COUNT - 1 || col == 0 || col == COL_COUNT - 1)
                {
                    fieldState[row, col] = CellStateType.WALL;
                }

                //Black and White
                if ((row == 4 && col == 5) || (row == 5 && col == 4))
                {
                    fieldState[row, col] = CellStateType.BLACK;
                }
                if ((row == 4 && col == 4) || (row == 5 && col == 5))
                {
                    fieldState[row, col] = CellStateType.WHITE;
                }
            }
        }

        gameMode = GameMode.BLACK;
        Message.MessageText = BLACK_TURN_MESSAGE;

        DrawFieldS();
    }

    void OnMouseDown()
    {

    }

    public class CellState
    {
        public int Row = 0;
        public int Col = 0;
        public Manager.CellStateType CSType = Manager.CellStateType.FIELD;
        public CellState(int row, int col, Manager.CellStateType cstype)
        {
            this.Row = row;
            this.Col = col;
            this.CSType = cstype;
        }
    }

    public static void ChangeField(int row, int col)
    {
        //フィールド更新
        Manager.CellStateType player_type = (Manager.gameMode == Manager.GameMode.BLACK) ? Manager.CellStateType.BLACK : Manager.CellStateType.WHITE;
        Manager.CellStateType enemy_type = (Manager.gameMode == Manager.GameMode.BLACK) ? Manager.CellStateType.WHITE : Manager.CellStateType.BLACK;

        int current_row = row;
        int current_col = col;
        List<Manager.CellState> csList = new List<Manager.CellState>();

        //north
        while (current_row > 0)
        {
            current_row--;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                csList.Add(new Manager.CellState(current_row, current_col, player_type));
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && csList.Count > 0)
            {
                //更新実行
                foreach (Manager.CellState cs in csList)
                {
                    Manager.fieldState[cs.Row, cs.Col] = cs.CSType;
                }
                break;
            }
            else
            {
                break;
            }
        }

        current_row = row;
        current_col = col;
        csList.Clear();

        //east
        while (current_col < Manager.COL_COUNT - 1)
        {
            current_col++;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                csList.Add(new Manager.CellState(current_row, current_col, player_type));
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && csList.Count > 0)
            {
                //更新実行
                foreach (Manager.CellState cs in csList)
                {
                    Manager.fieldState[cs.Row, cs.Col] = cs.CSType;
                }
                break;
            }
            else
            {
                break;
            }
        }

        current_row = row;
        current_col = col;
        csList.Clear();

        //south
        while (current_row < Manager.ROW_COUNT - 1)
        {
            current_row++;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                csList.Add(new Manager.CellState(current_row, current_col, player_type));
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && csList.Count > 0)
            {
                //更新実行
                foreach (Manager.CellState cs in csList)
                {
                    Manager.fieldState[cs.Row, cs.Col] = cs.CSType;
                }
                break;
            }
            else
            {
                break;
            }
        }

        current_row = row;
        current_col = col;
        csList.Clear();

        //west
        while (current_col > 0)
        {
            current_col--;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                csList.Add(new Manager.CellState(current_row, current_col, player_type));
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && csList.Count > 0)
            {
                //更新実行
                foreach (Manager.CellState cs in csList)
                {
                    Manager.fieldState[cs.Row, cs.Col] = cs.CSType;
                }
                break;
            }
            else
            {
                break;
            }
        }

        current_row = row;
        current_col = col;
        csList.Clear();

        //north west
        while (current_row > 0 && current_col > 0)
        {
            current_row--;
            current_col--;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                csList.Add(new Manager.CellState(current_row, current_col, player_type));
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && csList.Count > 0)
            {
                //更新実行
                foreach (Manager.CellState cs in csList)
                {
                    Manager.fieldState[cs.Row, cs.Col] = cs.CSType;
                }
                break;
            }
            else
            {
                break;
            }
        }

        current_row = row;
        current_col = col;
        csList.Clear();

        //north east
        while (current_row > 0 && current_col < Manager.COL_COUNT - 1)
        {
            current_row--;
            current_col++;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                csList.Add(new Manager.CellState(current_row, current_col, player_type));
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && csList.Count > 0)
            {
                //更新実行
                foreach (Manager.CellState cs in csList)
                {
                    Manager.fieldState[cs.Row, cs.Col] = cs.CSType;
                }
                break;
            }
            else
            {
                break;
            }
        }

        current_row = row;
        current_col = col;
        csList.Clear();

        //south west
        while (current_row < Manager.ROW_COUNT - 1 && current_col > 0)
        {
            current_row++;
            current_col--;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                csList.Add(new Manager.CellState(current_row, current_col, player_type));
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && csList.Count > 0)
            {
                //更新実行
                foreach (Manager.CellState cs in csList)
                {
                    Manager.fieldState[cs.Row, cs.Col] = cs.CSType;
                }
                break;
            }
            else
            {
                break;
            }
        }

        current_row = row;
        current_col = col;
        csList.Clear();

        //south east
        while (current_row < Manager.ROW_COUNT - 1 && current_col < Manager.COL_COUNT - 1)
        {
            current_row++;
            current_col++;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                csList.Add(new Manager.CellState(current_row, current_col, player_type));
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && csList.Count > 0)
            {
                //更新実行
                foreach (Manager.CellState cs in csList)
                {
                    Manager.fieldState[cs.Row, cs.Col] = cs.CSType;
                }
                break;
            }
            else
            {
                break;
            }
        }
    }

    public static bool CanSandwich(int row, int col, Manager.GameMode gameMode)
    {

        Manager.CellStateType player_type = (gameMode == Manager.GameMode.BLACK) ? Manager.CellStateType.BLACK : Manager.CellStateType.WHITE;
        Manager.CellStateType enemy_type = (gameMode == Manager.GameMode.BLACK) ? Manager.CellStateType.WHITE : Manager.CellStateType.BLACK;

        int current_row = row;
        int current_col = col;
        int get_enemy_count = 0;

        //north
        while (current_row > 0)
        {
            current_row--;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                get_enemy_count++;
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && get_enemy_count > 0)
            {
                return true;
            }
            else
            {
                break;
            }
        }

        //reset
        current_row = row;
        current_col = col;
        get_enemy_count = 0;

        //east
        while (current_col < Manager.COL_COUNT - 1)
        {
            current_col++;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                get_enemy_count++;
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && get_enemy_count > 0)
            {
                return true;
            }
            else
            {
                break;
            }
        }

        //reset
        current_row = row;
        current_col = col;
        get_enemy_count = 0;

        //south
        while (current_row < Manager.ROW_COUNT - 1)
        {
            current_row++;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                get_enemy_count++;
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && get_enemy_count > 0)
            {
                return true;
            }
            else
            {
                break;
            }
        }

        //reset
        current_row = row;
        current_col = col;
        get_enemy_count = 0;

        //west
        while (current_col > 0)
        {
            current_col--;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                get_enemy_count++;
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && get_enemy_count > 0)
            {
                return true;
            }
            else
            {
                break;
            }
        }

        //reset
        current_row = row;
        current_col = col;
        get_enemy_count = 0;

        //north west
        while (current_row > 0 && current_col > 0)
        {
            current_row--;
            current_col--;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                get_enemy_count++;
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && get_enemy_count > 0)
            {
                return true;
            }
            else
            {
                break;
            }
        }

        //reset
        current_row = row;
        current_col = col;
        get_enemy_count = 0;

        //north east
        while (current_row > 0 && current_col < Manager.COL_COUNT - 1)
        {
            current_row--;
            current_col++;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                get_enemy_count++;
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && get_enemy_count > 0)
            {
                return true;
            }
            else
            {
                break;
            }
        }

        //reset
        current_row = row;
        current_col = col;
        get_enemy_count = 0;

        //south west
        while (current_row < Manager.ROW_COUNT - 1 && current_col > 0)
        {
            current_row++;
            current_col--;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                get_enemy_count++;
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && get_enemy_count > 0)
            {
                return true;
            }
            else
            {
                break;
            }
        }

        //reset
        current_row = row;
        current_col = col;
        get_enemy_count = 0;

        //south east
        while (current_row < Manager.ROW_COUNT - 1 && current_col < Manager.COL_COUNT - 1)
        {
            current_row++;
            current_col++;
            if (Manager.fieldState[current_row, current_col] == enemy_type)
            {
                get_enemy_count++;
            }
            else if (Manager.fieldState[current_row, current_col] == player_type && get_enemy_count > 0)
            {
                return true;
            }
            else
            {
                break;
            }
        }

        return false;
    }

    public static List<Manager.CellState> SetableList(Manager.GameMode gameMode)
    {
        List<Manager.CellState> list = new List<Manager.CellState>();
        for (int row = 0; row < Manager.ROW_COUNT; row++)
        {
            for (int col = 0; col < Manager.COL_COUNT; col++)
            {
                if (Manager.fieldState[row, col] != Manager.CellStateType.FIELD) { continue; }
                if (Manager.CanSandwich(row, col, gameMode))
                {
                    list.Add(new Manager.CellState(row, col, Manager.CellStateType.FIELD));
                }
            }
        }
        return list;
    }


    // スプライトの取得
    // @param fileName ファイル名
    // @param spriteName スプライト名
    public static Sprite GetSprite(string fileName, string spriteName)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(fileName);
        return System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(spriteName));
    }
}
