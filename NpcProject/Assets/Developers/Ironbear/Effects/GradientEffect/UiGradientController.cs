using UnityEngine;
using UnityEngine.UI;

public class UiGradientController : MonoBehaviour
{
    [SerializeField]
    private Material gradientMat;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        //ResizeImageToScreenSize();
    }

    private void ResizeImageToScreenSize()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 이미지 비율에 따라 크기 조정
        float imageWidth = image.sprite.rect.width;
        float imageHeight = image.sprite.rect.height;
        float imageAspectRatio = imageWidth / imageHeight;

        float newImageWidth, newImageHeight;

        // 이미지가 가로로 더 긴 경우
        if (screenWidth / screenHeight > imageAspectRatio)
        {
            newImageWidth = screenWidth;
            newImageHeight = screenWidth / imageAspectRatio;
        }
        // 이미지가 세로로 더 긴 경우
        else
        {
            newImageHeight = screenHeight;
            newImageWidth = screenHeight * imageAspectRatio;
        }

        // 이미지 크기 적용
        RectTransform imageRectTransform = image.rectTransform;
        imageRectTransform.sizeDelta = new Vector2(newImageWidth, newImageHeight);
    }
}
