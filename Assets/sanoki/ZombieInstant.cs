using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieInstant : MonoBehaviour {
    public GameObject cam;
    public GameObject[] instantPos;//0:赤 1:青 2:緑 3:黄
    public GameObject zombiePre;//ゾンビプレハブ
    BoxCollider col;//ボックスコライダー
    Vector3 pos;//座標
    public int[] score;//0:赤 1:青 2:緑 3:黄
    private void Start()
    {
         col=zombiePre.GetComponent<BoxCollider>();//ボックスコライダーを取得
    }
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instans();
        }
	}
    string autTagCnange(int preId)
    {
        string preTag = "None";
        switch (preId)
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
        StartCoroutine(ScoreCount());//生成コルーチンを呼び出し
    }
    private IEnumerator ScoreCount()
    {
        int highScore = score[0];//プレイヤー1のスコアを取得

        for (int i = 1; i < instantPos.Length; i++)//プレイヤーの数だけ繰り返す
        {
            if (highScore < score[i])//スコアの大きい方を比較
            {
                highScore = score[i];//ハイスコアの更新
            }
        }
        for (int j = 0; j <= highScore; j++)//ハイスコアの数だけ繰り返す
        {
            for (int k = 0; k < instantPos.Length; k++)//生成位置の数だけ繰り返す
            {
                if (j <= score[k])//各プレイヤーのスコア以下なら
                {
                    pos = instantPos[k].transform.position;//生成位置座標を取得
                    //生成
                    Instantiate(
                        zombiePre,                        // プレハブ
                        instantPos[k].transform.position, // 座標
                        Quaternion.identity               // 回転
                        ).tag = autTagCnange(k);
                    pos.y += col.size.y;//生成位置をゾンビプレハブ１個分上にずらす
                    instantPos[k].transform.position = pos;
                }
            }
            yield return new WaitForSeconds(0.1f);//0.1秒待つ
        }

    }
}
