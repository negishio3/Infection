using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MobChangeSystem : MonoBehaviour
{
    public Text a;
    [SerializeField]
    private GameObject[] objs;
    private static GameObject[] mobZombies=new GameObject[5];
    private static int[] scoreCount=new int[4];
    //0は市民,1～4がゾンビ

    
    
    void Start()
    {
        for(int i = 0; i < objs.Length; i++)
        {
            mobZombies[i] = objs[i];
        }
    }

    void Update()
    {
        a.text = "1P:" + scoreCount[0] + "  2P:" + scoreCount[1] + "  3P" + scoreCount[2] + "  4P" + scoreCount[3];
    }

    public static void MobCount(int num, out int count)//mobオブジェクトの番号がnumと一致するオブジェクトの数をcountに渡すはず
    {
        GameObject[] mobs;
        mobs = GameObject.FindGameObjectsWithTag("Mob").
        Where(e => e.GetComponent<PlayerNumber>().PlayerNum == num).
        ToArray();
        count = mobs.Length;
    }

    public static void MobChanger(Vector3 pos, int num)
    {
        Instantiate(mobZombies[num], pos, Quaternion.identity);
    }

    public static void MobDelete()
    {
        for(int i = 0; i < 4; i++)
        {
            int a=0;
            MobCount(i + 1, out a);
            scoreCount[i] = a;
        }
        foreach (GameObject mob in GameObject.FindGameObjectsWithTag("Mob"))
        {
            Destroy(mob);
        }
    }
}

