using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTouch : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera LootCamera;


    public void Start()
    {
        LootCamera = Managers.Loot.lootCam;

        if (LootCamera == null)
            LootCamera = GameObject.Find("LootCamera").GetComponent<Camera>();

    }


    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            Ray ray = LootCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject == gameObject && hit.collider.gameObject.layer == LayerMask.NameToLayer("Loot"))
            {
                hit.collider.GetComponent<DropItem>().GetItem();
            }

            Debug.DrawRay(ray.origin, ray.direction * 10000f, Color.red, 2f);
        }
    }
}
