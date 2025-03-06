using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class HandManager : Singleton<HandManager>
{
    protected override void Awake()
    {
        base.Awake();
        Hide(true);
    }

    public void Show(bool instant = false)
    {
        Vector3 targetPos = new Vector3(0, transform.position.y, transform.position.z);

        if (instant) {
            transform.position = targetPos;
            return;
        }
        
        transform.DOMove(targetPos, 1f).SetEase(Ease.InOutQuad);
    }

    public void Hide(bool instant = false)
    {
        Vector3 targetPos = new Vector3(-50, transform.position.y, transform.position.z);

        if (instant) {
            transform.position = targetPos;
            return;
        }
        
        transform.DOMove(targetPos, 1f).SetEase(Ease.InOutQuad);
    }
}
