using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour
{

    public static string MessageText = "";

    public GameObject message;
    private GUIText messagetext;

    // Use this for initialization
    void Start()
    {
        messagetext = message.GetComponent<GUIText>();
    }

    // Update is called once per frame
    void Update()
    {
        messagetext.text = MessageText;
    }
    
    void OnMouseDown()
    {
        print("メッセージを押しました。");
    }
}
