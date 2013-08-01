using UnityEngine;
using System.Collections;

public class StartMenuGUI : MonoBehaviour
{
    public MovieTexture startMovie;
    public Texture2D background;

    //public Texture2D[] title = new Texture2D[4];
    //public float timeleft;
    //float alltime = 1.2f;

    void Start()
    {
        if (PlayerPrefs.HasKey("loadLevel")) //要被load的場景
            PlayerPrefs.DeleteKey("loadLevel");

        startMovie.loop = true;
        startMovie.Play();

        //timeleft = alltime;
    }

    void FixedUpdate()
    {
        //timeleft -= Time.fixedDeltaTime;
        //if (timeleft <= 0)
        //    timeleft = alltime;
    }

    void OnGUI()
    {
        float SW = Screen.width * 0.01f;
        float SH = Screen.height * 0.01f;

        GUI.DrawTexture(new Rect(0, 0, SW * 100, SH * 100), background, ScaleMode.StretchToFill, true); //背景
        GUI.DrawTexture(new Rect(SW * 37f, SH * 85f, SW * 30f, SH * 4f), startMovie, ScaleMode.StretchToFill, true); //影片


        //if (timeleft >= alltime * 3 / 4)
        //    GUI.DrawTexture(new Rect(SW * 37f, SH * 85f, SW * 30f, SH * 4f), title[0]);
        //if (alltime * 3 / 4 > timeleft && timeleft >= alltime * 2 / 4)
        //    GUI.DrawTexture(new Rect(SW * 37f, SH * 85f, SW * 30f, SH * 4f), title[1]);
        //if (alltime * 2 / 4 > timeleft && timeleft >= alltime * 1 / 4)
        //    GUI.DrawTexture(new Rect(SW * 37f, SH * 85f, SW * 30f, SH * 4f), title[2]);
        //if (alltime * 1 / 4 > timeleft && timeleft >= alltime * 0)
        //    GUI.DrawTexture(new Rect(SW * 37f, SH * 85f, SW * 30f, SH * 4f), title[3]);

        
        //SendMessage("StopMovie");

        if (Input.anyKey)
        {
            startMovie.Stop();

            PlayerPrefs.SetString("loadLevel", "test");//要被load的場景
            Application.LoadLevel("Loading"); 
        }

    }

    //IEnumerator StopMovie()
    //{
    //    Time.timeScale = 1.0f;
    //    yield return new WaitForSeconds(8.0f);
    //}
}
