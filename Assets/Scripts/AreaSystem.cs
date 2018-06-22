using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AreaSystem : MonoBehaviour
{
    [SerializeField, Header("変更間隔")]
    private float changeTime;
    [SerializeField, Header("プレイヤー")]
    private List<GameObject> PlayerList=new List<GameObject>();
    [SerializeField, Header("AIプレイヤー")]
    private List<GameObject> AIPlayerobjList=new List<GameObject>();

    [SerializeField]
    private string sceneName;

    [SerializeField]
    private GameObject areaobj;
    [SerializeField]
    private Text timeText;

    public List<Vector3> areaPosList = new List<Vector3>(4);

    [SerializeField]
    private List<GameObject> Items=new List<GameObject>();



    private float timer;//残り時間

    private const int spcount = 20;//沸き数
    private const int areDis = 8;//エリアの中心からの距離

    private Queue<GameObject> areaQueue=new Queue<GameObject>();


    private MultipleTargetCamera MTC;

    private int nowArea;

    




    void Start()
    {
        nowArea = 0;
        StartCoroutine(AreaEnumerator());
        StartCoroutine(ItemCoroutine());
        MTC = GameObject.Find("Camera").GetComponent<MultipleTargetCamera>();
        timer = changeTime+1;
    }

    void Update()
    {
        timeText.text = Mathf.Floor(timer).ToString();
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
    }


    void AreaChange(Vector3 pos)
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Item"))
        {
            Destroy(g);
        }
        MobChangeSystem.MobDelete();
        GameObject area= Instantiate(areaobj, new Vector3(pos.x,0.1f,pos.z), Quaternion.identity);
        areaQueue.Enqueue(area);
        foreach (GameObject pl in GameObject.FindGameObjectsWithTag("Player"))
        {
            Destroy(pl);
        }
        //cam.transform.position = new Vector3(pos.x, 40f, pos.z - 20);
        for (int i = 0; i < 4; i++)
        {
            Vector3 spwpos;
            Quaternion qua = RandomQua();
            MobSpawnPos(pos, out spwpos);
            GameObject obj;
            if (EntrySystem.entryFlg[i])
            {
                obj=(GameObject)Instantiate(PlayerList[i], spwpos, qua);
            }
            else
            {
                obj=(GameObject)Instantiate(AIPlayerobjList[i], spwpos, qua);
            }
            obj.GetComponent<NavMeshAgent>().enabled = true;
            obj.GetComponent<PlayerNumber>().PlayerNum = i + 1;
        }
        for (int i = 0; i < spcount; i++)
        {
            Vector3 spwpos;
            Quaternion qua=RandomQua();
            MobSpawnPos(pos, out spwpos);
            MobChangeSystem.HumanSpawn(spwpos,qua);
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
            x = Random.Range(areapos.x + areDis, areapos.x - areDis);
            z = Random.Range(areapos.z + areDis, areapos.z - areDis);
            ray = new Ray(new Vector3(x, 20, z), Vector3.down);
            Physics.Raycast(ray, out hit);
            c++;
        } while (hit.collider.tag != "Area");
        sppos = new Vector3(x, 1.5f, z);
    }

    IEnumerator AreaEnumerator()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i=0; i < 4; i++)
        {
            nowArea = i;
            AreaChange(areaPosList[i]);
            MTC.PlayerSearch();
            if (i != 0) { Destroy(areaQueue.Dequeue()); }
            yield return new WaitWhile(()=>timer>=0);
            timer = changeTime + 1;
        }
        //リザルトへ
        GameObject.Find("Canvas").GetComponent<SceneFader_sanoki>().StageSelect(sceneName);
    }



    Quaternion RandomQua()
    {
        Quaternion qu;
        float Ry = Random.Range(0f, 360f);
        qu = Quaternion.Euler(0f, Ry, 0f);
        return qu;
    }

    IEnumerator ItemCoroutine()
    {
        int item = 0;
        while (true)
        {
            yield return new WaitForSeconds(11.25f);
            item = Random.Range(0, 2);
            if (item == 0)
            {
                ItemCreate(item);
            }
            else if (item == 1 && !GameObject.Find(Items[item].name))
            {
                ItemCreate(item);
            }
        }
    }

    void ItemCreate(int item)
    {
        Vector3 spwpos;
        MobSpawnPos(areaPosList[nowArea], out spwpos);
        GameObject obj = (GameObject)Instantiate(Items[item], spwpos, Quaternion.Euler(-90,0,0));
        obj.name = Items[item].name;
    }
}
