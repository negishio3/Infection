using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public Camera cam;//使用するカメラ

    public List<Transform> targets;//画面内に収めたいオブジェクト

    public Vector3 offset;//オフセット値
    public float smoothTime = 0.5f;//カメラの動くスピード

    public float minZoom = 40;//最低ズーム値
    public float maxZoom = 10;//最大ズーム値
    public float zoomLimiter = 50;//

    GameObject centerObj;
    private Vector3 centerPos;

    private Vector3 velocity;
    private void Start()
    {
        GetPoint();
        cam.transform.LookAt(GetPoint());
        //Debug.Log("X(" + GetPoint().x + "):Y(" + GetPoint().y + "):Z(" + GetPoint().z + ")");
        centerObj = new GameObject("Empty Game Object");//からのオブジェクトを生成して
        centerObj.transform.position = GetPoint();
        transform.parent = centerObj.transform;
        getObjScreenPos();
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
    }

    private void Reset()
    {
        cam = GetComponent<Camera>();//カメラコンポーネントを取得
    }

    private void Update()
    {
        if (targets.Count == 0) return;//例外処理
        centerObj.transform.position = GetPoint();
        transform.LookAt(GetPoint());
        //Move();
        Zoom();
    }

    private void Zoom()
    {
        var newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    //スクリーン座標に変換する
    private void getObjScreenPos()
    {
        Vector2[] screenPos=new Vector2[4];
        for(int i = 0; i < screenPos.Length; i++)
        {
            screenPos[i] = Camera.main.WorldToScreenPoint(targets[i].transform.position);
            Debug.Log((i+1)+"番目：X(" + screenPos[i].x + ")：Y(" + screenPos[i].y + ")");
        }
    }

    private float GetGreatestDistance()
    {
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }

    /// <summary>
    /// 中心座標の計算
    /// </summary>
    /// <returns></returns>
    private Vector3 GetPoint()
    {
        //Vector3 pos;//
        Vector3 centerPoint = Vector3.zero;//中心座標
        if (targets.Count == 1) return targets[0].position;//ターゲットが一体しかいないならターゲットが中心座標
        float maxX, maxZ;//値が一番大きい座標
        float minX, minZ;//値が一番小さい座標
        //とりあえず配列の頭のターゲットの座標を代入
        maxX = targets[0].transform.position.x;
        maxZ = targets[0].transform.position.z;
        minX = targets[0].transform.position.x;
        minZ = targets[0].transform.position.z;
        //ターゲット分回す
        for (int i = 1; i < targets.Count; i++)
        {
            //比較した座標を代入
            if (maxX < targets[i].transform.position.x) maxX = targets[i].transform.position.x;
            if (maxZ < targets[i].transform.position.z) maxZ = targets[i].transform.position.z;
            if (minX > targets[i].transform.position.x) minX = targets[i].transform.position.x;
            if (minZ > targets[i].transform.position.z) minZ = targets[i].transform.position.z;
        }
        //pos.x = (maxX - minX) / 2;
        //pos.z = (maxZ - minZ) / 2;
        centerPoint.x = minX + (maxX - minX) / 2;
        centerPoint.z = minZ + (maxZ - minZ) / 2;

        return centerPoint;

    }
    private string getDistantCharacter()
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("Player").
            OrderBy(b => Vector3.Distance(b.transform.position, transform.position)).ToArray();

        return a[a.Length - 1].name;
    }

}