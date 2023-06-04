using DG.Tweening;
using System;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 이동 속도
    public float maxHeight = 3.0f; // 최대 높이
    public float duration = 1.0f; // 이동 시간

    [SerializeField] Transform startTr;
    [SerializeField] Transform endTr;

    private Vector3 startPos; // 시작 위치
    private Vector3 endPos; // 종료 위치
    private float startTime; // 이동 시작 시간

    private bool isMoving = false; // 이동 중인지 여부

    public GameObject Card;


    private void Start()
    {
        StartMovement();
    }

    public void InitData(Transform start, Transform end ,GameObject Card = null, float speed = 3, float maxHeight = 4, float duration = 1)
    {
        this.moveSpeed = speed;
        this.maxHeight = maxHeight;
        this.duration = duration;
        this.Card = Card;

        startTr = start;
        endTr = end;
    }

    Action _action;
    public void InitData(Transform start, Transform end, Action a,GameObject Card = null, float speed = 3, float maxHeight = 4, float duration = 1)
    {
        this.moveSpeed = speed;
        this.maxHeight = maxHeight;
        this.duration = duration;
        this.Card = Card;

        
        

        startTr = start;
        endTr = end;

        Card.transform.localPosition = startTr.localPosition;
        Card.transform.localScale = new Vector3(1, 1, 1);

        _action = null;
        _action = a;
    }

    // 아이템 이동 시작
    public void StartMovement()
    {
        if (!isMoving)
        {
            float random = UnityEngine.Random.Range(-2f, 2f);

            startPos = startTr.position;

            if (endTr == null)
                endPos = startPos + Vector3.left * random + Vector3.up * maxHeight;
            else
                endPos = endTr.position;

            startTime = Time.time;
            isMoving = true;
        }
    }

    // 프레임마다 아이템 이동
    private void Update()
    {
        if (isMoving)
        {
            float elapsed = Time.time - startTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // 곡선 이동
            Vector3 curvePos = Vector3.Lerp(startPos, endPos, t) + Vector3.up * Mathf.Sin(t * Mathf.PI) * maxHeight;

            transform.position = curvePos;

            // 이동 완료 시 초기화
            if (t == 1.0f)
            {
                isMoving = false;
                startPos = endPos;
                endPos = startPos - Vector3.up * maxHeight;
                startTime = Time.time;
                if(_action != null)
                {

                    endTr.DOPunchScale(new Vector3(0.2f, 0.2f), 0.1f).OnComplete(() =>
                      {
                          
                          endTr.transform.localScale = new Vector3(1, 1, 1);
                      });
                    _action?.Invoke();

                }
                

            }
        }
    }


}
