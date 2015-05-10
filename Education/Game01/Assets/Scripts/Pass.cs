using UnityEngine;
using System.Collections;

public class Pass : MonoBehaviour
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
        int Setable_Black_Count = Manager.SetableList(Manager.GameMode.BLACK).Count;
        int Setable_White_Count = Manager.SetableList(Manager.GameMode.WHITE).Count;

        switch (Manager.gameMode)
        {
            case Manager.GameMode.NONE:
            case Manager.GameMode.RESULT:
                break;
            case Manager.GameMode.WHITE:
                if (Setable_White_Count==0)
                {
                    Manager.gameMode = Manager.GameMode.BLACK;
                    Message.MessageText = "「黒の番です。」";
                }
                break;
            case Manager.GameMode.BLACK:
                if (Setable_Black_Count==0)
                {
                    Manager.gameMode = Manager.GameMode.WHITE;
                    Message.MessageText = "「白の番です。」";
                }
                break;
        }
    }
}
