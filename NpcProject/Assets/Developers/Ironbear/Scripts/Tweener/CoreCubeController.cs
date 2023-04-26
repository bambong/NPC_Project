using UnityEngine;
using DG.Tweening;

public class CoreCubeController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cubes;

    private Tweener floatTweener;
    private Tweener turnTweener;
    private GameObject selectedGameObject;

    private float minDelay = 1f;
    private float maxDelay = 5f;

    private float waveDelay = 0.1f;

    private Sequence waveSequence;

    void Start()
    {
        var sequence = DOTween.Sequence();

        /*
        var group = new GameObject("WaveGroup");
        foreach(var obj in cubes)
        {
            obj.transform.SetParent(group.transform);
        }
        

        waveSequence = DOTween.Sequence();
        for(int i=0; i<cubes.Length; i++)
        {
            var obj = cubes[i];
        }
        */

        int randomIndex = Random.Range(0, cubes.Length);
        selectedGameObject = cubes[randomIndex];

        //CallRandomAnimation();
    }

    private void Turning()
    {
        turnTweener = selectedGameObject.transform.DORotate(new Vector3(0, 0, 360), 3, RotateMode.FastBeyond360);
    }

    private void CallRandomAnimation()
    {
        float delay = Random.Range(minDelay, maxDelay);

        DOVirtual.DelayedCall(delay, () =>
        {
            turnTweener.OnComplete(CallRandomAnimation);
        });
    }

}
