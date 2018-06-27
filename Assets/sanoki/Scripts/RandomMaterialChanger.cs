﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterialChanger : MonoBehaviour {

    Material[] _materials;

    public List<Material> _materialLis;

    private void Start()
    {
        _materials = GetComponent<Renderer>().materials;
        _materialLis.AddRange(Resources.LoadAll<Material>("ResultZombieMaterial"));
        ReplaceMaterial(0, _materialLis[Random.Range(0, _materialLis.Count)]);
    }

    private void ReplaceMaterial(int index, Material mat)
    {

        Renderer renderer = GetComponent<Renderer>();

        Material[] mats = renderer.materials;

        if (index < 0 || mats.Length <= index) return;

        mats[index] = mat;

        renderer.materials = mats;

    }
}