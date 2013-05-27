using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    BotControlScript botCtrl;
    Transform player;

    // Use this for initialization
    void Start()
    {
        botCtrl = player.root.GetComponent<BotControlScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        //if (botCtrl.isDie)
        //{
            GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, Screen.width * 0.5f, Screen.height * 0.5f), "你已經死了");
            if (GUI.Button(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, Screen.width * 0.2f, Screen.height * 0.2f), "Restart"))
            {
            }
        //}
    }



}
