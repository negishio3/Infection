using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AreaSystem : MonoBehaviour
{
    [SerializeField, Header("プレイヤー")]
    private GameObject PlayerObj;
    [SerializeField, Header("AIプレイヤー")]
    private GameObject AIPlayerObj;

    private List<GameObject> PlayerList = new List<GameObject>();

    private static List<AIPlayer> aiPlayerList = new List<AIPlayer>();

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject areaobj;
    private float times;//エリア変更感覚

    private int spcount = 8;//沸き数

    private Queue<GameObject> areaQueue=new Queue<GameObject>();
    public List<Vector3> poslist = new List<Vector3>(4);



    public static List<AIPlayer> AIPlayerList
    {
        get { return aiPlayerList; }
        set { aiPlayerList = value; }
    }

    void Start()
    {
        StartCoroutine(AreaEnumerator());
    }

    void Update()
    {

    }

    void AreaChange(Vector3 pos)
    {
        MobChangeSystem.MobDelete();
        GameObject area= Instantiate(areaobj, new Vector3(pos.x,1.5f,pos.z), Quaternion.identity);
        areaQueue.Enqueue(area);
        foreach (GameObject pl in GameObject.FindGameObjectsWithTag("Player"))
        {
            Destroy(pl);
        }
        cam.transform.position = new Vector3(pos.x, 40f, pos.z - 20);
        for (int i = 0; i < 4; i++)
        {
            Vector3 spwpos;
            MobSpawnPos(pos, out spwpos);
            GameObject obj;
            if (EntrySystem.entryFlg[i])
            {
                obj=(GameObject)Instantiate(PlayerObj, spwpos, Quaternion.identity);
            }
            else
            {
                obj=(GameObject)Instantiate(AIPlayerObj, spwpos, Quaternion.identity);
            }
            obj.GetComponent<PlayerNumber>().PlayerNum = i + 1;
        }
        for (int i = 0; i < spcount; i++)
        {
            Vector3 spwpos;
            MobSpawnPos(pos, out spwpos);
            MobChangeSystem.MobChanger(spwpos, 0);
        }
    }

    void MobSpawnPos(Vector3 areapos, out Vector3 sppos)
    {
        float x = 0, z = 0;
        int c=0;
        Ray ray;
        RaycastHit hit;
        do
        {
            if (c > 20)
            {
                x = areapos.x;z = areapos.z;
                break;
            }
            x = Random.Range(areapos.x + 10, areapos.x - 10);
            z = Random.Range(areapos.z + 10, areapos.z - 10);
            ray = new Ray(new Vector3(x, 20, z), Vector3.down);
            Physics.Raycast(ray, out hit);
            c++;
        } while (hit.collider.tag != "Area");
        sppos = new Vector3(x, 1.5f, z);
    }

    IEnumerator AreaEnumerator()
    {
        for (int i = 0; i < 4; i++)
        {
            AreaChange(poslist[i]);
            if (i != 0) { Destroy(areaQueue.Dequeue()); }
            yield return new WaitForSeconds(5f);
        }
        //リザルトへ
    }

    void PlayerCreate(Vector3[] pos)
    {
        for (int i = 0; i < 4; i++)
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
