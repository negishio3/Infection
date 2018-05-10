using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCreate : MonoBehaviour {
    [SerializeField,Header("プレイヤー")]
    private GameObject PlayerObj;
    [SerializeField,Header("AIプレイヤー")]
    private GameObject AIPlayerObj;
    [SerializeField,Header("沸き位置")]
    private Vector3[] pos=new Vector3[4];
    void Start()
    {
        PlayerCreate();
    }


    void Update()
    {

    }

    void PlayerCreate()
    {
        for(int i = 0; i < 4; i++)
        {
            GameObject obj,cObj;
            if (EntrySystem.entryFlg[i])
            { obj = PlayerObj; }
            else
            { obj = AIPlayerObj; }
            cObj=(GameObject)Instantiate(obj,pos[i], Quaternion.identity);
            cObj.GetComponent<PlayerNumber>().PlayerNum = i + 1;
        }
    }


}
