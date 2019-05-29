using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using ZTools;

public class BlockFeedback : MonoBehaviour {
    public MeshRenderer meshRenderer;
    private Sequence sequence;

    // 產生的效果
    public void Generate () {
        InitSequence ();
        transform.localScale = Vector3.zero;
        sequence.Append (transform.DOScale (new Vector3 (1, 1, 1) * 0.8f, 0.5f).SetEase (Ease.OutElastic));
    }
    // 點擊正確的效果
    public void BecomeCenter () {
        InitSequence ();
        // 修改成為可以變成透明的shader
        Color color = meshRenderer.material.color;
        meshRenderer.material = MapManager.Instance.blockCenterMaterial;
        meshRenderer.material.color = color;
        Vector3 prePos = transform.position;
        // 動態
        sequence
            .Append (transform.DOShakeRotation (0.5f, 40, 20))
            .Join (meshRenderer.material.DOFade (0, 0.5f)).AppendCallback (() => {
                transform.position = prePos;
            });
    }
    // 點擊錯誤的效果
    public void ClickError () { }
    // 初始動態
    private void InitSequence () {
        if (sequence != null) {
            sequence.Complete ();
        }
        sequence = DOTween.Sequence ();
    }
}