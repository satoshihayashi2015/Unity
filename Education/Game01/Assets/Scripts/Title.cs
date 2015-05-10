using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 32;
        labelStyle.alignment = TextAnchor.MiddleCenter;
        GUIStyleState labelStyleState = new GUIStyleState();
        labelStyleState.textColor = Color.white;
        labelStyle.normal = labelStyleState;
        GUI.Label(
            new Rect((Screen.width - 400) / 2, (Screen.height - 200) / 2, 400, 60)
            , "リバーシ"
            , labelStyle
        );

        if (GUI.Button(new Rect((Screen.width - 200) / 2, (Screen.height - 40) / 2, 200, 40), "GAME START"))
        {
            Application.LoadLevel("Main");
        }

    }
}
