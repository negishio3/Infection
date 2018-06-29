using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public Camera cam;//使用するカメラ

    public GameObject[] targets;//画面内に収めたいオブジェクト

    public float offset;//オフセット値
    private void Reset()
    {
        cam = GetComponent<Camera>();//カメラコンポーネントを取得
    }

    //public void addTarget(GameObject target)
    //{
    //    targets.Add(target);
    //}

    private void Update()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");
        transform.position = Vector3.Lerp(transform.position, calcCameraPosition(), 1.0f);
    }

    //一番遠いオブジェクトを調べる
    private Vector3 calcCameraPosition()
    {
        //if (targets.Count== 0) return targets[0].transform.position;
        Vector3[] targetPosArray = new Vector3[targets.Length];
        Vector3 centerPointWorld = new Vector3();
        Vector3 centerPointScreen = new Vector3();
        for(int i = 0; i < targets.Length; i++)
        {
            targetPosArray[i] = targets[i].transform.position;
            targetPosArray[i].x *= (float)Screen.height / Screen.width;
            centerPointWorld += targetPosArray[i];
        }
        centerPointWorld /= targets.Length;
        centerPointScreen = Camera.main.WorldToScreenPoint(centerPointWorld);
        float farDistans = new float();
        for (int i = 0; i < targets.Length; i++)
        {
            float newDistans = (targetPosArray[i] - centerPointWorld).magnitude;
            if (farDistans < newDistans)
            {
                farDistans = newDistans;
            }
        }
        float cameraLength = farDistans / Mathf.Tan(Camera.main.fieldOfView / 2 * Mathf.Deg2Rad);
        return centerPointWorld - transform.forward * (cameraLength + offset);
    }
}