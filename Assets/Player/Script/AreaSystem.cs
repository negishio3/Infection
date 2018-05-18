using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AreaSystem : MonoBehaviour {
    private static List<AIPlayer> aiPlayerList = new List<AIPlayer>(); 
    private static List<GameObject> floorList = new List<GameObject>();
    private static List<GameObject> areaObj = new List<GameObject>();

    private float times;//エリア変更感覚
    private Vector3 pos;
    private int spcount=8;//沸き数



    public static List<AIPlayer> AIPlayerList
    {
        get { return aiPlayerList; }
        set { aiPlayerList = value; }
    }

    void Start()
    {
        foreach (GameObject f in GameObject.FindGameObjectsWithTag("Floor"))
        {
            floorList.Add(f);
        }
        int x = 0, z = 0;
        x = Random.Range(-20, 20);
        z = Random.Range(-20, 20);
        pos = new Vector3(x, 0, z);
        AreaChange(pos);
    }

    void Update()
    {
        times += Time.deltaTime;
        if (times> 30)
        {
            times = 0;
            RandomPos(pos,out pos);
            AreaChange(pos);
        }
    }

    void RandomPos(Vector3 vec,out Vector3 vecout)
    {
        Vector3 vector;
        do
        {
            int x = 0, z = 0;
            x = Random.Range(-20, 20);
            z = Random.Range(-20, 20);
            vector = new Vector3(x, 0, z);
        } while (Vector3.Distance(vec, vector) < 20);
        vecout = vector;
    }

    public static void AreaChange(Vector3 areapos)
    {
        int num = 0;
        if (areaObj.Any())
        {
            for (int i = 0; i < areaObj.Count; i++) {
                areaObj[i].tag = "Floor";
                areaObj[i].GetComponent<Renderer>().material.color = Color.grey;
            }
            areaObj.Clear();
        }
        foreach (GameObject f in floorList)
        {
            if (Vector3.Distance(areapos, f.transform.position) < 30)
            {
                f.tag = "Area";
                f.GetComponent<Renderer>().material.color = Color.cyan;
                areaObj.Add(f);
                num++;
            }
        }
        foreach(AIPlayer a in aiPlayerList)
        {
            a.AreaPos = areapos;
        }
        MobChangeSystem.MobDelete();
        
        for(int j = 0; j < 12; j++)
        {
            Vector3 v;
            MobSpawnPos(areapos, out v);
            MobChangeSystem.MobChanger(v, 0);
        }
    }

    public static void MobSpawnPos(Vector3 areapos,out Vector3 sppos)
    {
        float x = 0, z= 0;
        x = Random.Range(areapos.x + 20, areapos.x - 20);
        z = Random.Range(areapos.z + 20, areapos.z - 20);
        sppos = new Vector3(x,1.5f,z);
    }
}
