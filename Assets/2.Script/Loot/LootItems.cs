using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LootItems : MonoBehaviour
{

    protected string _itemId;
    protected GameObject _lootItem;
    protected int _layer;
    protected Rigidbody2D _rigid;
    protected int _ignoreLayer;
    protected int _reward;
    protected Canvas _canvas;
    protected Camera _lootCamera;
    protected bool _isRecycle;
    [SerializeField]protected RectTransform _rect;
    [SerializeField] Vector3 scale;
    protected bool _isRect;
    protected Camera _lootCam;
    protected Transform Target;
    protected Vector3 targetVector;

    protected virtual void Init()
    {
        if(_lootItem == null)
            _lootItem = this.gameObject;

        gameObject.BindEvent(OnClickEvent);
        //_reward = (int)Managers.Game.CalculateStageReward((int)Managers.Game.StageData.currentStage);

        //gameObject.GetComponent<RectTransform>() != null
        if (gameObject.GetComponent<RectTransform>() != null)
        {
            _isRect = true;
            _rect = transform.gameObject.GetComponent<RectTransform>();
            _rect.localScale = new Vector3(1, 1, 1);
            _rect.localPosition = new Vector3(0, 0, 0);

            transform.localScale = new Vector3(1, 1, 1);
            transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            _lootCam = Managers.Loot.lootCam;
            _isRect = false;

            float goSize = GetSize();

            transform.localScale = new Vector3(goSize, goSize, goSize);
            transform.localPosition = new Vector3(0, 0, 0);
        }


        scale = transform.localScale;
    }


    float GetSize()
    {
        switch(gameObject.name)
        {
            case "Key": return 200f;
            case "Chest": return 100f;
            case "EXP": return 50f;
            default:
                return 100f;
               
        }
    }

    public virtual void OnClickEvent(PointerEventData data)
    {
        if(_isRect)
        {
            _rect.localScale = new Vector3(1, 1, 1);
            _rect.localPosition = new Vector3(0, 0, 0);

        }
        else
        {
            transform.localScale = new Vector3(100, 100, 100);
            transform.localPosition = new Vector3(0, 0, 0);
        }

        Destroy();
    }

    protected void Destroy()
    {
        Managers.Resource.Destroy(_lootItem);
    }



}
