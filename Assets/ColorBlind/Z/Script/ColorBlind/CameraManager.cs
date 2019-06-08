using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class CameraManager : MonoBehaviour {
    public Camera sceneCamera;
    Vector3 prePosition;
    // Start is called before the first frame update
    void Start () { }

    // Update is called once per frame
    void Update () {
        // if (Input.GetKeyDown (KeyCode.Q)) {
        //     DoLookAllOffset (maxPosition, minPosition, new Vector2 (1, 0.7f), new Vector2 (1, 1), 0.8f);
        // }
        // if (Input.GetKeyDown (KeyCode.W)) {
        //     Debug.Log (sceneCamera.WorldToViewportPoint (t1.transform.position));
        //     Debug.Log (sceneCamera.WorldToViewportPoint (t2.transform.position));
        // }
    }
    // 有偏移時設定 
    // ratio如果都為 (1, 1) 為沒偏移
    public void DoLookAllOffset (Vector3 maxPosition, Vector3 minPosition, Vector2 maxOffsetRatio, Vector2 minOffsetRatio, float viewpointRatio = 1f, float duration = 1f) {
        maxPosition.x /= maxOffsetRatio.x;
        maxPosition.y /= maxOffsetRatio.y;
        minPosition.x /= minOffsetRatio.x;
        minPosition.y /= minOffsetRatio.y;
        DoLookAll (maxPosition, minPosition, viewpointRatio, duration);
    }

    public void DoLookAll (Vector3 maxPosition, Vector3 minPosition, float viewpointRatio = 1f, float duration = 1f) {
        prePosition = sceneCamera.transform.position;
        // 計算中心點
        Vector3 center = (maxPosition + minPosition) / 2;
        // 擴展
        maxPosition = (maxPosition - center) / viewpointRatio + center;
        minPosition = (minPosition - center) / viewpointRatio + center;
        // 計算高跟寬的一半
        float halfHeight = Mathf.Abs (maxPosition.y - center.y);
        float halfWidth = Mathf.Abs (maxPosition.x - center.x);
        // 符合高度的鏡頭距離
        float cameraDistanceH = GetDistanceFromHeight (halfHeight);
        // 符合寬度的鏡頭距離
        float cameraDistanceW = GetDistanceFromHeight (halfWidth / sceneCamera.aspect);
        // 比較哪個距離比較遠
        float cameraDistance = Mathf.Max (cameraDistanceH, cameraDistanceW);
        // 設置鏡頭位置
        center.z -= cameraDistance;
        sceneCamera.transform.DOMove (center, duration);
    }

    public void DoReset (float duration = 1f) {
        sceneCamera.transform.DOMove (prePosition, duration);
    }
    // 透過 FOV 與視野高度取得鏡頭距離
    private float GetDistanceFromHeight (float height) {
        return height / Mathf.Tan ((sceneCamera.fieldOfView * 0.5f) * Mathf.Deg2Rad);
    }
}