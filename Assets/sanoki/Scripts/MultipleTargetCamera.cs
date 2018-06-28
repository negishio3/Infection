using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public Camera cam;//使用するカメラ

    public List<GameObject> targets;//画面内に収めたいオブジェクト

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
        GetCenterPoint();
        cam.transform.LookAt(GetCenterPoint());
        //Debug.Log("X(" + GetPoint().x + "):Y(" + GetPoint().y + "):Z(" + GetPoint().z + ")");
        centerObj = new GameObject("Empty Game Object");//からのオブジェクトを生成して
        centerObj.name = "CenterPoint";
        centerObj.transform.position = GetPoint();
        transform.parent = centerObj.transform;
        getScreenPos();
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Debug.Log("ID:"+farCharacter());
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
    private Vector3[] getScreenPos()
    {
        Vector3[] screenPos = new Vector3[4];
        for(int i = 0; i < screenPos.Length; i++)
        {
            screenPos[i] = Camera.main.WorldToScreenPoint(targets[i].transform.position);
            //Debug.Log((i+1)+"番目：X(" + screenPos[i].x + ")：Y(" + screenPos[i].y + ")");
        }
        return screenPos;
    }

    private float GetGreatestDistance()
    {
        Bounds bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].transform.position);
        }
        return bounds.size.x;
    }

    /// <summary>
    /// 中心座標の計算
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCenterPoint()
    {
        Vector3 centerPoint = Vector3.zero;//中心座標
        if (targets.Count == 1) return getScreenPos()[0];//ターゲットが一体しかいないならターゲットが中心座標
        for (int i = 0; i < targets.Count; i++)
        {
            centerPoint += getScreenPos()[i];
        }
        centerPoint /= 4;
        return centerPoint;

    }
    private Vector3 GetPoint()
    {
        Vector3 centerPoint = Vector3.zero;
        for(int i = 0; i < targets.Count; i++)
        {
            centerPoint += targets[i].transform.position;
        }
        return centerPoint/4;
    }
    //一番遠いオブジェクトを調べる
    private int farCharacter()
    {
        float farObj = 0;
        float newPos = 0;
        int farObjID = 0;
        for(int i = 0; i < targets.Count; i++)
        {
            newPos = targets[i].transform.position.magnitude;
            if (farObj < newPos)
            {
                farObjID = i;
            }
        }
        return farObjID;
    }
    private string getDistantCharacter()
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("Player").
            OrderBy(b => Vector3.Distance(b.transform.position, transform.position)).ToArray();

        return a[a.Length - 1].name;
    }

}