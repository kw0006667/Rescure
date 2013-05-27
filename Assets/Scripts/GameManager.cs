using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private BotControlScript botCtrl;
    public GameObject player;
    public Texture2D DieBackground;
    public Texture2D DieTitle;

    // Use this for initialization
    void Start()
    {
        botCtrl = player.GetComponent<BotControlScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (botCtrl.isDie)
        {
            //GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, Screen.width * 0.5f, Screen.height * 0.5f), "你已經死了");
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.DieBackground);
            GUI.DrawTexture(new Rect(Screen.width * 0.5f - 256, Screen.height * 0.5f - 32, 512, 64), this.DieTitle);
            if (GUI.Button(new Rect(Screen.width * 0.5f - 64, Screen.height * 0.5f + 50, 128, 32), "Restart"))
            {
                Application.LoadLevel(0);
            }
        }
    }



}
