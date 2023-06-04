using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class QuestScriptable : ScriptableObject
{
    public string questId;
    public Sprite questIcon;
    public Define.QuestType questType;
    public string questTile;
    public Sprite rewardIcon;
    public string rewardId;
    public int point;
    public int multiple;
}
