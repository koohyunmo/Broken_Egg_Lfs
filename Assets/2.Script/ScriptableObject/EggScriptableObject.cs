using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "New Egg", menuName = "ScriptableObject/Egg")]
public class EggScriptableObject : ScriptableObject
{
    public string eggID;
    public string eggName;
    public Sprite eggImage;
    public int level;
    public long MaxHp;
}


#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(EggScriptableObject))]
public class EffPreviewer : Editor
{
    EggScriptableObject item;
    Texture2D icon;

    public override void OnInspectorGUI()
    {
        item = (EggScriptableObject)target;

        GUILayout.BeginHorizontal();
        icon = AssetPreview.GetAssetPreview(item.eggImage);
        GUILayout.Label(icon);

        GUILayout.EndHorizontal();
        DrawDefaultInspector();

    }
}
#endif
