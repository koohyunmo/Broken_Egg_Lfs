using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindEgg : MonoBehaviour
{
    int _count = 1;
    [SerializeField] public List<Sprite> sprites = new List<Sprite>();

    // Start is called before the first frame update

    public void CheckImagesFolder()
    {
        while (true)
        {
            Sprite t2D = GetTex(_count.ToString());

            if (t2D == null)
                return;
            else
            {
                sprites.Add(t2D);
                _count++;
            }

        }
    }

    public List<Sprite> GetSprites(List<Sprite> list)
    {
        GetTex(_count.ToString());
        CheckImagesFolder();

        list = sprites;

        return list;
    }



    protected Sprite GetTex(string path)
    {
        Sprite sprite = null;

        if (path.Contains("Images/") == false)
        {
            path = $"Images/{path}";
        }

        if (true)
        {
            sprite = Managers.Resource.Load<Sprite>(path);

        }

        if (sprite == null)
            Debug.Log($"Texture2D Missing !{path}");


        return sprite;
    }


}
