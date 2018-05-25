using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartCreate : MonoBehaviour {
    [SerializeField,Header("プレイヤー")]
    private GameObject PlayerObj;
    [SerializeField,Header("AIプレイヤー")]
    private GameObject AIPlayerObj;
    [SerializeField,Header("沸き位置")]
    private Vector3[] poss=new Vector3[4];

    public static List<GameObject> PlayerList = new List<GameObject>();
    void Start()
    {
        PlayerCreate(poss);
    }


    void Update()
    {

    }

    void PlayerCreate(Vector3[] pos)
    {
        for(int i = 0; i < 4; i++)
        {
            GameObject cObj;
            if (EntrySystem.entryFlg[i])
            {
                cObj = (GameObject)Instantiate(PlayerObj, pos[i], Quaternion.identity);
                cObj.GetComponent<PlayerNumber>().PlayerNum = i + 1;
                PlayerList.Add(cObj);
            }
            else
            { 
                cObj = (GameObject)Instantiate(AIPlayerObj, pos[i], Quaternion.identity);
                cObj.GetComponent<PlayerNumber>().PlayerNum = i + 1;
                AreaSystem.AIPlayerList.Add(cObj.GetComponent<AIPlayer>());
                PlayerList.Add(cObj);
            }
        }
    }
}
