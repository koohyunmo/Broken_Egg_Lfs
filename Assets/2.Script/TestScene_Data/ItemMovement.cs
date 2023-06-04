using DG.Tweening;
using System;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f; // �̵� �ӵ�
    public float maxHeight = 3.0f; // �ִ� ����
    public float duration = 1.0f; // �̵� �ð�

    [SerializeField] Transform startTr;
    [SerializeField] Transform endTr;

    private Vector3 startPos; // ���� ��ġ
    private Vector3 endPos; // ���� ��ġ
    private float startTime; // �̵� ���� �ð�

    private bool isMoving = false; // �̵� ������ ����

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

    // ������ �̵� ����
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

    // �����Ӹ��� ������ �̵�
    private void Update()
    {
        if (isMoving)
        {
            float elapsed = Time.time - startTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // � �̵�
            Vector3 curvePos = Vector3.Lerp(startPos, endPos, t) + Vector3.up * Mathf.Sin(t * Mathf.PI) * maxHeight;

            transform.position = curvePos;

            // �̵� �Ϸ� �� �ʱ�ȭ
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
