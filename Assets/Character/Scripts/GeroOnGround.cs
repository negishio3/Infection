using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeroOnGround : MonoBehaviour {
    [SerializeField, Tooltip("owe")]
    private GameObject Owe;
    
    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Mob" && col.tag != "Player")
        {
            Vector3 hitpos= col.ClosestPointOnBounds(this.transform.position);
            Instantiate(Owe, hitpos, Quaternion.Euler(-90, 0, 0));
            Destroy(gameObject);
        }
    }
}
