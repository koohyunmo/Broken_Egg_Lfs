using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class CUtil
{

    /// <summary>
    /// 오브젝트의 GetComponent를 까먹었을 경우 자동으로 컴포넌트 연결
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
    /// 자식 오브젝트를 순회하면서 해당 제너릭 타입을 찾음,게임 오브젝트 전용 매핑
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
    /// 자식 오브젝트를 순회하면서 해당 제너릭 타입을 찾음,컴포넌트 전용 매핑
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        // recursive = 재귀로 찾을거냐? 여부
        // 직속 자식만 찾을지 recursive = false
        // 모든 자식(자식의 자식의 자식 ...)을 찾을지 recursive = true

        if (go == null)
            return null;

        if (recursive == false)
        {
            // 직속 자식만 찾기

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

        if (number >= BigInteger.Pow(10, 36)) // 100,000,000,000,000,000,000,000,000,000,000,000,000 이상
        {
            return (number / BigInteger.Pow(10, 36)).ToString("F1") + "D";
        }
        else if (number >= BigInteger.Pow(10, 33)) // 1,000,000,000,000,000,000,000,000,000,000,000,000 이상
        {
            return (number / BigInteger.Pow(10, 33)).ToString("F1") + "C";
        }
        else if (number >= BigInteger.Pow(10, 30)) // 10^30 이상
        {
            return (number / BigInteger.Pow(10, 30)).ToString("F1") + "BB";
        }
        else if (number >= BigInteger.Pow(10, 27)) // 10^27 이상
        {
            return (number / BigInteger.Pow(10, 27)).ToString("F1") + "B";
        }
        else if (number >= BigInteger.Pow(10, 24)) // 10^24 이상
        {
            return (number / BigInteger.Pow(10, 24)).ToString("F1") + "AA";
        }
        else if (number >= BigInteger.Pow(10, 21)) // 10^21 이상
        {
            return (number / BigInteger.Pow(10, 21)).ToString("F1") + "A";
        }
        else if (number >= BigInteger.Pow(10, 18)) // 10^18 이상
        {
            return (number / BigInteger.Pow(10, 18)).ToString("F1") + "S";
        }
        else if (number >= BigInteger.Pow(10, 15)) // 10^15 이상
        {
            return (number / BigInteger.Pow(10, 15)).ToString("F1") + "P";
        }
        else if (number >= BigInteger.Pow(10, 12)) // 10^12 이상
        {
            return (number / BigInteger.Pow(10, 12)).ToString("F1") + "T";
        }
        else if (number >= BigInteger.Pow(10, 9)) // 10^9 이상
        {
            return (number / BigInteger.Pow(10, 9)).ToString("F1") + "B";
        }
        else if (number >= BigInteger.Pow(10, 6)) // 10^6 이상
        {
            return (number / BigInteger.Pow(10, 6)).ToString("F1") + "M";
        }
        else if (number >= BigInteger.Pow(10, 3)) // 10^3 이상
        {
            return (number / BigInteger.Pow(10, 3)).ToString("F1") + "K";
        }
        else // 1,000 미만
        {
            return number.ToString();
        }
    }

    public static string LongFormatNumber(long number)
    {
        if (number >= Mathf.Pow(10, 12)) // 1,000,000,000,000 이상
        {
            return (number / Mathf.Pow(10, 12)).ToString("F1") + "T";
        }
        else if (number >= Mathf.Pow(10, 9)) // 1,000,000,000 이상
        {
            return (number / Mathf.Pow(10, 9)).ToString("F1") + "B";
        }
        else if (number >= Mathf.Pow(10, 6)) // 1,000,000 이상
        {
            return (number / Mathf.Pow(10, 6)).ToString("F1") + "M";
        }
        else if (number >= Mathf.Pow(10, 3)) // 1,000 이상
        {
            return (number / Mathf.Pow(10, 3)).ToString("F1") + "K";
        }
        else // 1,000 미만
        {
            return number.ToString();
        }
    }


    public static (string, string) GetNumberAndText(BigInteger number)
    {
        string code = "";
        string numStr = "";

        if (number >= BigInteger.Pow(10, 36)) // 100,000,000,000,000,000,000,000,000,000,000,000,000 이상
        {
            numStr = (number / BigInteger.Pow(10, 36)).ToString("0");
            code = "D";
        }
        else if (number >= BigInteger.Pow(10, 33)) // 1,000,000,000,000,000,000,000,000,000,000,000,000 이상
        {
            numStr = (number / BigInteger.Pow(10, 33)).ToString("0");
            code = "C";
        }
        else if (number >= BigInteger.Pow(10, 30)) // 10^30 이상
        {
            numStr = (number / BigInteger.Pow(10, 30)).ToString("0");
            code = "BB";
        }
        else if (number >= BigInteger.Pow(10, 27)) // 10^27 이상
        {
            numStr = (number / BigInteger.Pow(10, 27)).ToString("0");
            code = "B";
        }
        else if (number >= BigInteger.Pow(10, 24)) // 10^24 이상
        {
            numStr = (number / BigInteger.Pow(10, 24)).ToString("0");
            code = "AA";
        }
        else if (number >= BigInteger.Pow(10, 21)) // 10^21 이상
        {
            numStr = (number / BigInteger.Pow(10, 21)).ToString("0");
            code = "A";
        }
        else if (number >= BigInteger.Pow(10, 18)) // 10^18 이상
        {
            numStr = (number / BigInteger.Pow(10, 18)).ToString("0");
            code = "S";
        }
        else if (number >= BigInteger.Pow(10, 15)) // 10^15 이상
        {
            numStr = (number / BigInteger.Pow(10, 15)).ToString("0");
            code = "P";
        }
        else if (number >= BigInteger.Pow(10, 12)) // 10^12 이상
        {
            numStr = (number / BigInteger.Pow(10, 12)).ToString("0");
            code = "T";
        }
        else if (number >= BigInteger.Pow(10, 9)) // 10^9 이상
        {
            numStr = (number / BigInteger.Pow(10, 9)).ToString("0");
            code = "B";
        }
        else if (number >= BigInteger.Pow(10, 6)) // 10^6 이상
        {
            numStr = (number / BigInteger.Pow(10, 6)).ToString("0");
            code = "M";
        }
        else if (number >= BigInteger.Pow(10, 3)) // 10^3 이상
        {
            numStr = (number / BigInteger.Pow(10, 3)).ToString("0");
            code = "K";
        }
        else // 1,000 미만
        {
            numStr = number.ToString();
            code = " ";
        }

        return (numStr, code);
    }

    public static string FormatNumber(long number)
    {
        if (number >= 1000000000000) // 1조 이상
        {
            return (number / 1000000000000f).ToString("0.###") + "T";
        }
        else if (number >= 1000000000) // 10억 이상
        {
            return (number / 1000000000f).ToString("0.###") + "B";
        }
        else if (number >= 1000000) // 1백만 이상
        {
            return (number / 1000000f).ToString("0.###") + "M";
        }
        else if (number >= 1000) // 1천 이상
        {
            return (number / 1000f).ToString("0.###") + "k";
        }
        else // 1천 미만
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
이 코드는 TextMeshPro를 사용하여 텍스트의 색상을 변경하는 함수입니다.

함수는 문자열(number, unit)과 각 문자열의 색상(numberColor, unitColor)을 매개변수로 받습니다.

함수 내에서는 먼저 numberColor와 unitColor를 16진수 색상 코드로 변환하고, 이를 이용하여 텍스트에 적용할 색상 코드 문자열(numberColorHex, unitColorHex)을 생성합니다.

그리고나서, TextMeshPro 객체의 text 속성에 색상이 적용된 문자열을 설정하여 텍스트의 색상을 변경합니다. 문자열 내에서 <color> 태그를 사용하여 numberColorHex와 unitColorHex 값을 적용합니다.

예를 들어, ChangeTextColors("10", "kg", Color.red, Color.blue) 함수를 호출하면, "10"은 빨간색으로, "kg"는 파란색으로 표시됩니다.
     */
    private void ChangeTextColors(string number, string unit, Color numberColor, Color unitColor)
    {
        string numberColorHex = ColorUtility.ToHtmlStringRGBA(numberColor);
        string unitColorHex = ColorUtility.ToHtmlStringRGBA(unitColor);

        //textMeshPro.text = $"<color=#{numberColorHex}>{number}</color> <color=#{unitColorHex}>{unit}</color>";
    }

}
