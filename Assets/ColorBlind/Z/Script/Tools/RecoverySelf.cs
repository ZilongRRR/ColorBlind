using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverySelf : MonoBehaviour {
    public float RecoveryTime = 1f;


    void OnEnable() {
        StartCoroutine(Recovery());
    }

    IEnumerator Recovery() {
        yield return new WaitForSeconds(RecoveryTime);
        JObjectPool.Instance.Recovery(this.gameObject);
    }
}
