using UnityEngine;
using DG.Tweening;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SubCubeController : MonoBehaviour
{
    private float distance = 1f;
    private float floatDuration = 1f;

    [SerializeField]
    private GameObject core;
    [SerializeField]
    private Animator animator;

    private Vector3 curVec;
    private Vector3 coreVec;
    private Vector3 vec;

    private Tweener floatTweener;
    private Tweener turnTweener;
    private Sequence sequence;

    private float turnInterval = 1.5f;
    private readonly Vector2 FLOAT_DURATION_RANGE = new Vector2(0.8f, 1.5f);
    private readonly Vector2 DISTANCE_RANGE = new Vector2(0.3f, 2f);

    void Start()
    {
        distance = Mathf.Floor(Random.Range(DISTANCE_RANGE.x, DISTANCE_RANGE.y) * 100) / 100;
        floatDuration = Mathf.Floor(Random.Range(FLOAT_DURATION_RANGE.x, FLOAT_DURATION_RANGE.y) * 100) / 100;
        curVec = transform.position;
        coreVec = core.transform.position;

        vec = (curVec - coreVec).normalized;

        floatTweener = transform.DOLocalMoveY(distance, floatDuration).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);

        /*
        sequence = DOTween.Sequence();
        sequence.AppendInterval(turnInterval);
        sequence.SetAutoKill(false);
        sequence.Append(transform.DORotate(new Vector3(0, 0, 360), 3, RotateMode.FastBeyond360));
        sequence.SetLoops(-1);
        DOVirtual.DelayedCall(turnInterval, CallTurnTweenerRecursive);
        sequence.Play();
        */
    }

    private void CallTurnTweenerRecursive()
    {
        sequence.Append(turnTweener);
        DOVirtual.DelayedCall(turnInterval, CallTurnTweenerRecursive);
    }
    public void OpenAnim() 
    {
        animator.SetBool("isOpen", true);
    }
    public void CloseAnim() 
    {
        animator.SetBool("isOpen", false);
    
    }
    public void SuccessAnim() 
    {
       //transform.DOKill();
     
        animator.SetBool("isSuccess", true);
    }
}
