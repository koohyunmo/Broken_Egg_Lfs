using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMask : MonoBehaviour
{
    public Image image;

    void Start()
    {
        image = GetComponent<Image>();

        Texture2D texture = image.sprite.texture;

        // 이미지의 알파 채널을 이용하여 크기 조정
        Rect rect = image.sprite.rect;
        int width = Mathf.RoundToInt(rect.width);
        int height = Mathf.RoundToInt(rect.height);
        Texture2D croppedTexture = new Texture2D(width, height);
        Color[] pixels = texture.GetPixels(Mathf.RoundToInt(rect.x), Mathf.RoundToInt(rect.y), width, height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        // 이미지 컴포넌트에 크롭한 텍스처를 할당
        Sprite croppedSprite = Sprite.Create(croppedTexture, new Rect(0, 0, width, height), Vector2.zero);
        image.sprite = croppedSprite;
    }

}
