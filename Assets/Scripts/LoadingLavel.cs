using UnityEngine;
using System.Collections;

public class LoadingLavel : MonoBehaviour
{
    string levelName;
    AsyncOperation async;
    int progress = 0;

    //public MovieTexture startMovie;
    public Texture2D background;
    float SW, SH;

    //public Texture2D[] loadAnimate;
    //float time;
    //float fps = 30.0f;
    //private int frame;

    void Start()
    {
        SW = Screen.width * 0.01f;
        SH = Screen.height * 0.01f;

        //startMovie.loop = true;
        //startMovie.Play(); //播放影片

        levelName = PlayerPrefs.GetString("loadLevel");
        StartCoroutine(loadScene());//非同步load場景
    }

    void Update()
    {
        progress = (int)(async.progress * 100);
        //print(progress);
        //print("123" + Application.GetStreamProgressForLevel(levelName));
    }

    void OnGUI()
    {
        SW = Screen.width * 0.01f;
        SH = Screen.height * 0.01f;

        GUI.DrawTexture(new Rect(0, 0, SW * 100, SH * 100), background, ScaleMode.StretchToFill, true); //背景
        //GUI.DrawTexture(new Rect(SW * 37f, SH * 85f, SW * 30f, SH * 4f), startMovie, ScaleMode.StretchToFill, true); //影片

        //Draw2DAniman(loadAnimate);
    }

    //void Draw2DAniman(Texture2D[] texture) //2d動畫
    //{
    //    time += Time.deltaTime;
    //    if (time >= 1.0 / fps)
    //    {
    //        frame++;
    //        time = 0;

    //        if (frame >= texture.Length)
    //            frame = 0;
    //    }

    //    GUI.DrawTexture(new Rect(SW * 37f, SH * 85f, SW * 30f, SH * 4f), texture[frame]);
    //}

    IEnumerator loadScene()
    {
        async = Application.LoadLevelAsync(levelName); //非同步load場景
        yield return async;
    }
}
