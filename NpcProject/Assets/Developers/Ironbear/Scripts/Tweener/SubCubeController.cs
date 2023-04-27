using UnityEngine;
using DG.Tweening;

public class SubCubeController : MonoBehaviour
{
    public float distance = 1f;
    public float floatDuration = 1f;

    [SerializeField]
    private GameObject core;

    private Vector3 curVec;
    private Vector3 coreVec;
    private Vector3 vec;

    private Tweener breathTweener;
    private Tweener floatTweener;
    private Tweener turnTweener;
    private Sequence sequence;

    private float turnInterval = 1.5f;
    private int breathCycle = 2;

    void Start()
    {
        curVec = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        coreVec = new Vector3(core.transform.position.x, core.transform.position.y, core.transform.position.z);

        vec = (curVec - coreVec).normalized;

        //breathTweener = transform.DOMove(vec * 2, breathCycle).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        floatTweener = transform.DOLocalMoveY(distance, floatDuration).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        //turnTweener = transform.DORotate(new Vector3(0, 0, 360), 3, RotateMode.FastBeyond360);

        sequence = DOTween.Sequence();
        sequence.AppendInterval(turnInterval);
        sequence.SetAutoKill(false);
        sequence.Append(transform.DORotate(new Vector3(0, 0, 360), 3, RotateMode.FastBeyond360));
        sequence.SetLoops(-1);
        DOVirtual.DelayedCall(turnInterval, CallTurnTweenerRecursive);
        sequence.Play();
    }

    private void CallTurnTweenerRecursive()
    {
        sequence.Append(turnTweener);
        //sequence.AppendCallback(CallTurnTweenerRecursive).SetDelay(turnInterval);
        DOVirtual.DelayedCall(turnInterval, CallTurnTweenerRecursive);
    }
}
