using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EntrySystem : MonoBehaviour {
    public static bool[] entryFlg = { false, false, false, false };
    [SerializeField]
    private Text[] text;
    private string entryText = "ENTRY";
    private string noEntryText = "NPC";
    [SerializeField]
    private string sceneName;

    public bool[] EntryFlg
    {
        get { return entryFlg; }
        set { entryFlg = value; }
    }

	void Start () {
		for(int i = 0; i < 4; i++)
        {
            text[i].text = noEntryText;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            entryFlg[0] = true;
            text[0].text = entryText;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            entryFlg[1] = true;
            text[1].text = entryText;
        }
        if (Input.GetButtonDown("Fire3"))
        {
            entryFlg[2] = true;
            text[2].text = entryText;
        }
        if (Input.GetButtonDown("Fire4"))
        {
            entryFlg[3] = true;
            text[3].text = entryText;
        }


        if (Input.GetButtonDown("Jump1"))
        {
            entryFlg[0] = false;
            text[0].text = noEntryText;
        }
        if (Input.GetButtonDown("Jump2"))
        {
            entryFlg[1] = false;
            text[1].text = noEntryText;
        }
        if (Input.GetButtonDown("Jump3"))
        {
            entryFlg[2] = false;
            text[2].text = noEntryText;
        }
        if (Input.GetButtonDown("Jump4"))
        {
            entryFlg[3] = false;
            text[3].text = noEntryText;
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
