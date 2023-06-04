using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class CUtil
{

    /// <summary>
    /// ������Ʈ�� GetComponent�� ��Ծ��� ��� �ڵ����� ������Ʈ ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    /// <summary>
    /// �ڽ� ������Ʈ�� ��ȸ�ϸ鼭 �ش� ���ʸ� Ÿ���� ã��,���� ������Ʈ ���� ����
    /// </summary>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;

    }

    /// <summary>
    /// �ڽ� ������Ʈ�� ��ȸ�ϸ鼭 �ش� ���ʸ� Ÿ���� ã��,������Ʈ ���� ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        // recursive = ��ͷ� ã���ų�? ����
        // ���� �ڽĸ� ã���� recursive = false
        // ��� �ڽ�(�ڽ��� �ڽ��� �ڽ� ...)�� ã���� recursive = true

        if (go == null)
            return null;

        if (recursive == false)
        {
            // ���� �ڽĸ� ã��

            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }


            }

        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }


    /// <summary>
    /// Num to String
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static string BigIntegerFormatNumber(BigInteger number)
    {

        if (number >= BigInteger.Pow(10, 36)) // 100,000,000,000,000,000,000,000,000,000,000,000,000 �̻�
        {
            return (number / BigInteger.Pow(10, 36)).ToString("F1") + "D";
        }
        else if (number >= BigInteger.Pow(10, 33)) // 1,000,000,000,000,000,000,000,000,000,000,000,000 �̻�
        {
            return (number / BigInteger.Pow(10, 33)).ToString("F1") + "C";
        }
        else if (number >= BigInteger.Pow(10, 30)) // 10^30 �̻�
        {
            return (number / BigInteger.Pow(10, 30)).ToString("F1") + "BB";
        }
        else if (number >= BigInteger.Pow(10, 27)) // 10^27 �̻�
        {
            return (number / BigInteger.Pow(10, 27)).ToString("F1") + "B";
        }
        else if (number >= BigInteger.Pow(10, 24)) // 10^24 �̻�
        {
            return (number / BigInteger.Pow(10, 24)).ToString("F1") + "AA";
        }
        else if (number >= BigInteger.Pow(10, 21)) // 10^21 �̻�
        {
            return (number / BigInteger.Pow(10, 21)).ToString("F1") + "A";
        }
        else if (number >= BigInteger.Pow(10, 18)) // 10^18 �̻�
        {
            return (number / BigInteger.Pow(10, 18)).ToString("F1") + "S";
        }
        else if (number >= BigInteger.Pow(10, 15)) // 10^15 �̻�
        {
            return (number / BigInteger.Pow(10, 15)).ToString("F1") + "P";
        }
        else if (number >= BigInteger.Pow(10, 12)) // 10^12 �̻�
        {
            return (number / BigInteger.Pow(10, 12)).ToString("F1") + "T";
        }
        else if (number >= BigInteger.Pow(10, 9)) // 10^9 �̻�
        {
            return (number / BigInteger.Pow(10, 9)).ToString("F1") + "B";
        }
        else if (number >= BigInteger.Pow(10, 6)) // 10^6 �̻�
        {
            return (number / BigInteger.Pow(10, 6)).ToString("F1") + "M";
        }
        else if (number >= BigInteger.Pow(10, 3)) // 10^3 �̻�
        {
            return (number / BigInteger.Pow(10, 3)).ToString("F1") + "K";
        }
        else // 1,000 �̸�
        {
            return number.ToString();
        }
    }

    public static string LongFormatNumber(long number)
    {
        if (number >= Mathf.Pow(10, 12)) // 1,000,000,000,000 �̻�
        {
            return (number / Mathf.Pow(10, 12)).ToString("F1") + "T";
        }
        else if (number >= Mathf.Pow(10, 9)) // 1,000,000,000 �̻�
        {
            return (number / Mathf.Pow(10, 9)).ToString("F1") + "B";
        }
        else if (number >= Mathf.Pow(10, 6)) // 1,000,000 �̻�
        {
            return (number / Mathf.Pow(10, 6)).ToString("F1") + "M";
        }
        else if (number >= Mathf.Pow(10, 3)) // 1,000 �̻�
        {
            return (number / Mathf.Pow(10, 3)).ToString("F1") + "K";
        }
        else // 1,000 �̸�
        {
            return number.ToString();
        }
    }


    public static (string, string) GetNumberAndText(BigInteger number)
    {
        string code = "";
        string numStr = "";

        if (number >= BigInteger.Pow(10, 36)) // 100,000,000,000,000,000,000,000,000,000,000,000,000 �̻�
        {
            numStr = (number / BigInteger.Pow(10, 36)).ToString("0");
            code = "D";
        }
        else if (number >= BigInteger.Pow(10, 33)) // 1,000,000,000,000,000,000,000,000,000,000,000,000 �̻�
        {
            numStr = (number / BigInteger.Pow(10, 33)).ToString("0");
            code = "C";
        }
        else if (number >= BigInteger.Pow(10, 30)) // 10^30 �̻�
        {
            numStr = (number / BigInteger.Pow(10, 30)).ToString("0");
            code = "BB";
        }
        else if (number >= BigInteger.Pow(10, 27)) // 10^27 �̻�
        {
            numStr = (number / BigInteger.Pow(10, 27)).ToString("0");
            code = "B";
        }
        else if (number >= BigInteger.Pow(10, 24)) // 10^24 �̻�
        {
            numStr = (number / BigInteger.Pow(10, 24)).ToString("0");
            code = "AA";
        }
        else if (number >= BigInteger.Pow(10, 21)) // 10^21 �̻�
        {
            numStr = (number / BigInteger.Pow(10, 21)).ToString("0");
            code = "A";
        }
        else if (number >= BigInteger.Pow(10, 18)) // 10^18 �̻�
        {
            numStr = (number / BigInteger.Pow(10, 18)).ToString("0");
            code = "S";
        }
        else if (number >= BigInteger.Pow(10, 15)) // 10^15 �̻�
        {
            numStr = (number / BigInteger.Pow(10, 15)).ToString("0");
            code = "P";
        }
        else if (number >= BigInteger.Pow(10, 12)) // 10^12 �̻�
        {
            numStr = (number / BigInteger.Pow(10, 12)).ToString("0");
            code = "T";
        }
        else if (number >= BigInteger.Pow(10, 9)) // 10^9 �̻�
        {
            numStr = (number / BigInteger.Pow(10, 9)).ToString("0");
            code = "B";
        }
        else if (number >= BigInteger.Pow(10, 6)) // 10^6 �̻�
        {
            numStr = (number / BigInteger.Pow(10, 6)).ToString("0");
            code = "M";
        }
        else if (number >= BigInteger.Pow(10, 3)) // 10^3 �̻�
        {
            numStr = (number / BigInteger.Pow(10, 3)).ToString("0");
            code = "K";
        }
        else // 1,000 �̸�
        {
            numStr = number.ToString();
            code = " ";
        }

        return (numStr, code);
    }

    public static string FormatNumber(long number)
    {
        if (number >= 1000000000000) // 1�� �̻�
        {
            return (number / 1000000000000f).ToString("0.###") + "T";
        }
        else if (number >= 1000000000) // 10�� �̻�
        {
            return (number / 1000000000f).ToString("0.###") + "B";
        }
        else if (number >= 1000000) // 1�鸸 �̻�
        {
            return (number / 1000000f).ToString("0.###") + "M";
        }
        else if (number >= 1000) // 1õ �̻�
        {
            return (number / 1000f).ToString("0.###") + "k";
        }
        else // 1õ �̸�
        {
            return number.ToString();
        }
    }

    public static Color GetGradeColor(string _id)
    {

        Color c = Color.white;
        switch (Managers.Data.ItemDic[_id].Grade)
        {
            case Define.Grade.None:
                c = Color.green;
                break;
            case Define.Grade.Common:
                c = Color.green;
                break;
            case Define.Grade.Rare:
                c = Color.cyan;
                break;
            case Define.Grade.Unique:
                c = Color.magenta;
                break;
            case Define.Grade.Legend:
                c = Color.yellow;
                break;
            case Define.Grade.Hero:
                c = Color.red;
                break;
        }

        return c;
    }

    public static string GetGradeColorString(string _id)
    {

        string s = "white";
        switch (Managers.Data.ItemDic[_id].Grade)
        {
            case Define.Grade.None:
                s = "green";
                break;
            case Define.Grade.Common:
                s = "green";
                break;
            case Define.Grade.Rare:
                s = "blue";
                break;
            case Define.Grade.Unique:
                s = "purple";
                break;
            case Define.Grade.Legend:
                s = "yellow";
                break;
            case Define.Grade.Hero:
                s = "orange";
                break;
        }


        return s;
    }
    public static string GetGradeColorDamageString(string _id)
    {

        string s = "white";
        switch (Managers.Data.ItemDic[_id].Grade)
        {
            case Define.Grade.None:
                s = "white";
                break;
            case Define.Grade.Common:
                s = "white";
                break;
            case Define.Grade.Rare:
                s = "blue";
                break;
            case Define.Grade.Unique:
                s = "purple";
                break;
            case Define.Grade.Legend:
                s = "yellow";
                break;
            case Define.Grade.Hero:
                s = "orange";
                break;
        }


        return s;
    }



    /*
�� �ڵ�� TextMeshPro�� ����Ͽ� �ؽ�Ʈ�� ������ �����ϴ� �Լ��Դϴ�.

�Լ��� ���ڿ�(number, unit)�� �� ���ڿ��� ����(numberColor, unitColor)�� �Ű������� �޽��ϴ�.

�Լ� �������� ���� numberColor�� unitColor�� 16���� ���� �ڵ�� ��ȯ�ϰ�, �̸� �̿��Ͽ� �ؽ�Ʈ�� ������ ���� �ڵ� ���ڿ�(numberColorHex, unitColorHex)�� �����մϴ�.

�׸�����, TextMeshPro ��ü�� text �Ӽ��� ������ ����� ���ڿ��� �����Ͽ� �ؽ�Ʈ�� ������ �����մϴ�. ���ڿ� ������ <color> �±׸� ����Ͽ� numberColorHex�� unitColorHex ���� �����մϴ�.

���� ���, ChangeTextColors("10", "kg", Color.red, Color.blue) �Լ��� ȣ���ϸ�, "10"�� ����������, "kg"�� �Ķ������� ǥ�õ˴ϴ�.
     */
    private void ChangeTextColors(string number, string unit, Color numberColor, Color unitColor)
    {
        string numberColorHex = ColorUtility.ToHtmlStringRGBA(numberColor);
        string unitColorHex = ColorUtility.ToHtmlStringRGBA(unitColor);

        //textMeshPro.text = $"<color=#{numberColorHex}>{number}</color> <color=#{unitColorHex}>{unit}</color>";
    }

}
