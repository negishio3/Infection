using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultZombieMove : MonoBehaviour {
    BoxCollider col;
    Animator ani;//animator
    Vector3 pos;//座標
    Vector3 qua;//回転
    float moveSpeed = 10.0f;//落ちる速度
    bool moveflg = true;//動くフラグ
    private void Start()
    {
        col = GetComponent<BoxCollider>();
        ani = GetComponent<Animator>();//Animatorコンポーネントを取得
        pos = transform.position;//初期位置を代入
        qua.y = Random.value*360;
        transform.Rotate(qua);
    }

    void Update()
    {
        if (moveflg)
        {
            pos.y -= moveSpeed * Time.deltaTime;//Y座標の値をひたすらマイナス
            transform.position = pos;//オブジェクトの座標に反映
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "grownd")
        {
            moveflg = false;
            pos.y = transform.position.y+col.size.y / 2;
            transform.position = pos;
            ani.SetTrigger("landing");//地面に触れたら落下モーションを再生
        }
    }

}
