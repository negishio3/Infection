using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieInstant : MonoBehaviour {
    public GameObject cam;
    public GameObject[] instantPos;//0:赤 1:青 2:緑 3:黄
    public GameObject[] zombiePre;//ゾンビプレハブ
    public GameObject[] playerZom;//プレイヤーキャラ
    BoxCollider col;//ボックスコライダー
    Vector3 pos;//座標
    public int[] scores;//0:赤 1:青 2:緑 3:黄
    public int[] playerID= { 0, 1, 2, 3 };
    public int Count;

    private void Start()
    {

         col=zombiePre[0].GetComponent<BoxCollider>();//ボックスコライダーを取得
    }
    void Update () {
        if (!SceneFader_sanoki.isFade && Count == 0)
        {
            Instans();
        }
	}
    string autTagCnange(int preID)
    {
        string preTag = "None";
        switch (preID)
        {
            case 0:
                preTag = "Red";
                break;
            case 1:
                preTag = "Blue";
                break;
            case 2:
                preTag = "Green";
                break;
            case 3:
                preTag = "Yellow";
                break;
        }
        return preTag;
    }
    public void Instans()
    {
        StartCoroutine(ScoreCount(MobChangeSystem.scoreCount));//生成コルーチンを呼び出し
    }
    private IEnumerator ScoreCount(int[] score)
    {
        Count = 1;
        int highScore = score[0];//プレイヤー1のスコアを取得
        int highScorePlayer = 0;
        for (int i = 1; i < instantPos.Length; i++)//プレイヤーの数だけ繰り返す
        {
            if (highScore < score[i])//スコアの大きい方を比較
            {
                highScore = score[i];//ハイスコアの更新
                highScorePlayer = i;
            }
        }
        for (int j = 0; j <= highScore; j++)//ハイスコアの数だけ繰り返す
        {
            for (int k = 0; k < instantPos.Length; k++)//生成位置の数だけ繰り返す
            {
                if (j < score[k])//各プレイヤーのスコア以下なら
                {
                    pos = instantPos[k].transform.position;//生成位置座標を取得
                    //生成
                    Instantiate(
                        zombiePre[k],                        // プレハブ
                        instantPos[k].transform.position, // 座標
                        Quaternion.identity               // 回転
                        ).tag = autTagCnange(k);
                    pos.y += col.size.y;//生成位置をゾンビプレハブ１個分上にずらす
                    instantPos[k].transform.position = pos;
                }
            }
            yield return new WaitForSeconds(0.1f);//0.1秒待つ
        }
        int maxScore = SortArray(score, playerID)[0] ;
        FindObjectOfType<ResultCam_sanoki>().camMove(maxScore);

    }
    int[] SortArray(int[] score,int[] playerID)
    {
        bool isEnd = false;
        while (!isEnd)
        {
            bool loopSwap = false;
            for (int i = 0; i < score.Length - 1; i++)
            {
                if (score[i] < score[i + 1])
                {
                    int x = score[i];
                    int ID = playerID[i];
                    score[i] = score[i + 1];
                    playerID[i] = playerID[i + 1];
                    score[i + 1] = x;
                    playerID[i + 1] = ID;
                    loopSwap = true;
                }
            }
            if (!loopSwap) // Swapが一度も実行されなかった場合はソート終了
            {
                isEnd = true;
            }
        }
        return playerID;
    }
}
