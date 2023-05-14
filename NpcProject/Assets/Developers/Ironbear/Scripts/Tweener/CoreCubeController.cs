using UnityEngine;
using DG.Tweening;

public class CoreCubeController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cubes;
    [SerializeField]
    private Vector3 targetScale;

    private float scaleDuration = 1f;
    private float minScale = 0.5f;
    private float maxScale = 2f;


    void Start()
    {
        PlayRandomAnimation();
    }

    private void PlayRandomAnimation()
    {
        int randomIndex = Random.Range(0, cubes.Length);
        GameObject selectedGameObject = cubes[randomIndex];

        int randomAnimationIndex = Random.Range(1, 4);
        
        switch(randomAnimationIndex)
        {
            case 0:
                PlayFloatAnimation(selectedGameObject);
                break;
            case 1:
                PlayTurnZAnimation(selectedGameObject);
                break;
            case 2:
                PlayScaleAnimation(selectedGameObject);
                break;
            case 3:
                PlayTurnYAnimation(selectedGameObject);
                break;
                   
        }
    }

    private void PlayFloatAnimation(GameObject target)
    {
        target.transform.DOMoveY(target.transform.position.y + 1f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }

    private void PlayTurnZAnimation(GameObject target)
    {
        target.transform.DORotate(new Vector3(0, 0, 360), 3f, RotateMode.FastBeyond360)
            .OnComplete(() =>
            {
                PlayRandomAnimation();
            });
    }

    private void PlayTurnYAnimation(GameObject target)
    {
        target.transform.DORotate(new Vector3(0, 360, 0), 3f, RotateMode.FastBeyond360)
            .OnComplete(() =>
            {
                PlayRandomAnimation();
            });
    }

    private void PlayScaleAnimation(GameObject target)
    {
        Vector3 initialScale = target.transform.localScale;
        float scales = Random.Range(minScale, maxScale);
        Vector3 targetScale = new Vector3(scales, scales, scales);

        target.transform.DOScale(targetScale, scaleDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                target.transform.DOScale(initialScale, scaleDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    PlayRandomAnimation();
                });
            });
    }
}
