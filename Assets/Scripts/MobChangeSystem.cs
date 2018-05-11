using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MobChangeSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objs;
    private static GameObject[] mobZombies=new GameObject[5];
    //0は市民,1～4がゾンビ
    void Start()
    {
        for(int i = 0; i < objs.Length; i++)
        {
            mobZombies[i] = objs[i];
        }
    }

    void MobCount(string tag, int num, out int count)//tagのオブジェクトの番号がnumと一致するオブジェクトの数をcountに渡すはず
    {
        GameObject[] mobs;
        mobs = GameObject.FindGameObjectsWithTag(tag).
        Where(e => e.GetComponent<PlayerNumber>().PlayerNum == num).
        ToArray();
        count = mobs.Length;
    }

    public static void MobChanger(Vector3 pos, int num)
    {
        Instantiate(mobZombies[num], pos, Quaternion.identity);
    }
}

