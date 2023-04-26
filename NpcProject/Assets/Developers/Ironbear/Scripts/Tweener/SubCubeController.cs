using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SubCubeController : MonoBehaviour
{
    public float distance = 1f;
    public float floatDuration = 1f;

    [SerializeField]
    private GameObject core;
    [SerializeField]
    private int breathCycle = 3;

    private Vector3 curVec;
    private Vector3 coreVec;
    private Vector3 vec;

    private Tweener breathTweener;
    private Tweener floatTweener;
    private Tweener turnTweener;
    private Sequence sequence;
    private float turnDelay = 3f;

    void Start()
    {
        curVec = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        coreVec = new Vector3(core.transform.position.x, core.transform.position.y, core.transform.position.z);

        vec = (curVec - coreVec).normalized;

        //breathTweener = transform.DOMove(vec * 2, breathCycle).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        floatTweener = transform.DOLocalMoveY(distance, floatDuration).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        turnTweener = transform.DORotate(new Vector3(0, 0, 360), 3, RotateMode.FastBeyond360);

        sequence = DOTween.Sequence();
        sequence.SetAutoKill(false);
        sequence.Append(floatTweener);
        InvokeRepeating(nameof(PlayTurnTweener), turnDelay, turnDelay);
    }

    private void PlayTurnTweener()
    {
        sequence.Insert(sequence.Duration(), turnTweener);
        sequence.Play();
    }
}
