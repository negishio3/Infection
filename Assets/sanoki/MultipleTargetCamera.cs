using System.Collections.Generic;
using UnityEngine;

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

    private Vector3 velocity;

    private void Reset()
    {
        cam = GetComponent<Camera>();//カメラコンポーネントを取得
    }

    private void LateUpdate()
    {
        if (targets.Count == 0) return;//例外処理

        Move();
        Zoom();
    }

    private void Zoom()
    {
        var newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    private void Move()
    {
        var centerPoint = GetCenterPoint();
        var newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
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
    private Vector3 GetCenterPoint()
    {
        if (targets.Count == 1) return targets[0].position;
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
}