using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurificationTrap : ItemBase {
    [SerializeField]
    private GameObject purification;

    public override void Execution(GameObject obj)
    {
        Instantiate(purification, transform.position, Quaternion.identity);

    }
}
