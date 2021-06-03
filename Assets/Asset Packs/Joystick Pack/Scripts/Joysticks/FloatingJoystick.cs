using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    Vector3 firstPosition;
    
    protected override void Start()
    {
        firstPosition = background.gameObject.transform.position;
        base.Start();
        background.gameObject.SetActive(true);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.transform.position = firstPosition;
        base.OnPointerUp(eventData);
    }
}