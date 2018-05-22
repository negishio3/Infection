using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader_sanoki : MonoBehaviour
{
    ///////////////////////////////////////////////////////////
    /// このscriptはシーン上のCanvasにアタッチしてください。/// 
    ///////////////////////////////////////////////////////////

    float red, green, blue; //RGBを操作するための変数
    public static string next_Scene;//遷移先のシーン名を入れる
    public static bool isFade = false;//フェード中かどうか
    int fade;//FadeIn：１ FadeOut：０

    Color fadeColor;//フェード時の色

    float fadeSpeed = 1.5f;//フェードの速さ

    void Start()
    {
        fade = 0;//シーンの読み込み時にフェードアウトさせる
        StartCoroutine(SceneFade(fadeSpeed, false));//フェードさせる
    }

    /// <summary>
    /// 引数のシーンにフェードして遷移する
    /// </summary>
    /// <param name="nextSceneName">遷移先のシーン名</param>
    public void StageSelect(string nextSceneName)
    {
        if (!isFade)//フェード中でないなら
        {
            isFade = true;//フェード中に変更する
            next_Scene = nextSceneName;//引数を遷移先に指定
            fade = 1;//フェードインさせる
            StartCoroutine(SceneFade(fadeSpeed,true));//フェードさせる
        }
    }

    /// <summary>
    /// フェードだけする
    /// </summary>
    /// <param name="fadeSpeed">フェードの速度</param>
    public void OnFade(float fadeSpeed)
    {
        if (!isFade)//フェード中でないなら
        {
            isFade = true;//フェード中に変更
            fade = 1;
            StartCoroutine(SceneFade(fadeSpeed, false));
        }
    }

    //フェード用のGUI
    void OnGUI()
    {   
        GUI.color = fadeColor;
        GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),Texture2D.whiteTexture);
    }

    /// <summary>
    /// フェード用コルーチン
    /// </summary>
    /// <param name="seconds">何秒かけてフェードさせるのか</param>
    /// <param name="sceneChange">シーン遷移させるのかどうか(true:させる　false:させない)</param>
    /// <returns></returns>
    public IEnumerator SceneFade(float seconds,bool sceneChange)
    {
        float t = 0.0f;
        switch (fade)
        {
            case 1:
                //Debug.Log("フェードイン");
                while (t < 1.0f)
                {
                    t += Time.deltaTime / seconds;
                    fadeColor.a = Mathf.Lerp(0.0f, 1.0f, t);
                    yield return null;
                }
                if (sceneChange) { SceneManager.LoadScene(next_Scene); }
                else
                {
                    fade = 0;
                    StartCoroutine(SceneFade(fadeSpeed, false));
                }
                break;
            case 0:
                while (t < 1.0f)
                {
                    t += Time.deltaTime / seconds;
                    fadeColor.a = Mathf.Lerp(1.0f, 0.0f, t);
                    yield return null;
                }
                isFade = false;
                break;
        }
    }
}