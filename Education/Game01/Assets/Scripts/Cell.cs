using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cell : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        string[] pos = name.Replace("cell", "").Split(new string[] { "_" }, System.StringSplitOptions.RemoveEmptyEntries);
        if (pos.Length != 2) { return; }

        string rowStr = pos[0];
        string colStr = pos[1];
        int row = 0;
        int col = 0;

        if (!int.TryParse(rowStr, out row)) { return; }
        if (!int.TryParse(colStr, out col)) { return; }

        TrySet(row, col, Manager.gameMode);

        int Setable_Black_Count = Manager.SetableList(Manager.GameMode.BLACK).Count;
        int Setable_White_Count = Manager.SetableList(Manager.GameMode.WHITE).Count;

        if (Setable_Black_Count == 0 && Setable_White_Count == 0)
        {
            print("終了");
            Manager.gameMode = Manager.GameMode.RESULT;
            Result();
        }
    }

    private static void Result()
    {
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

        //白の勝ち・黒の勝ち・引き分けを判定する
        if (whiteCount > blackCount)
        {
            Message.MessageText = string.Format("「白{0}:黒{1}で白の勝ちです！」", whiteCount, blackCount);
        }
        else if (blackCount > whiteCount)
        {
            Message.MessageText = string.Format("「白{0}:黒{1}で黒の勝ちです！」", whiteCount, blackCount);
        }
        else
        {
            Message.MessageText = string.Format("「白{0}:黒{1}で引き分けです！」", whiteCount, blackCount);
        }
    }

    private void TrySet(int row, int col, Manager.GameMode gameMode)
    {
        if (Manager.fieldState[row, col] != Manager.CellStateType.FIELD) { return; }
        if (!Manager.CanSandwich(row, col, gameMode)) { return; }

        switch (Manager.gameMode)
        {
            case Manager.GameMode.WHITE:
                Manager.fieldState[row, col] = Manager.CellStateType.WHITE;
                Manager.ChangeField(row, col);
                Manager.DrawFieldS();
                Manager.gameMode = Manager.GameMode.BLACK;
                Message.MessageText = Manager.BLACK_TURN_MESSAGE;
                break;
            case Manager.GameMode.BLACK:
                Manager.fieldState[row, col] = Manager.CellStateType.BLACK;
                Manager.ChangeField(row, col);
                Manager.DrawFieldS();
                Manager.gameMode = Manager.GameMode.WHITE;
                Message.MessageText = Manager.WHITE_TURN_MESSAGE;
                break;
        }
    }
}
