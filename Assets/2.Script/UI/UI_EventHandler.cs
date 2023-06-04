using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	//public Action OnClickHandler = null;
	public Action OnPressedHandler = null;
	public Action OnPointerDownHandler = null;
	public Action OnPointerUpHandler = null;

	public Action<PointerEventData> OnClickHandler = null;
	public Action<PointerEventData> OnDragHandler = null;
	//public Action OnActionClickHandler = null;



    bool _pressed = false;

	private void Update()
	{
		if (_pressed)
			OnPressedHandler?.Invoke();
	}



	public void OnPointerDown(PointerEventData eventData)
	{
		_pressed = true;
		OnPointerDownHandler?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_pressed = false;
		OnPointerUpHandler?.Invoke();
	}

    public void OnPointerClick(PointerEventData eventData)
    {
		//Debug.Log("OnBeginDrag");
		if (OnClickHandler != null)
			OnClickHandler.Invoke(eventData);

		//OnActionClickHandler?.Invoke();

	}

    public void OnDrag(PointerEventData eventData)
    {
		
		//Debug.Log("OnDrag");
		if (OnDragHandler != null)
			OnDragHandler.Invoke(eventData);
	}
}
