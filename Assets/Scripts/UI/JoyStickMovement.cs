using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickMovement : MonoBehaviour
{
    public static JoyStickMovement Instance // singleton
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<JoyStickMovement>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("JoyStickMovement");
                    instance = instanceContainer.AddComponent<JoyStickMovement>();
                }
            }
            return instance;
        }
    }

    private static JoyStickMovement instance;

    [SerializeField] GameObject smallStick;
    [SerializeField] GameObject bgStick;
    public Vector3 joyVec;
    
    Vector3 stickFirstPosition;
    Vector3 joyStickFirstPosition;
    float stickRadius;
    
    // Start is called before the first frame update
    void Start()
    {
        stickRadius = bgStick.GetComponent<RectTransform>().sizeDelta.y / 2;
        joyStickFirstPosition = bgStick.transform.position;
    }

    public void PointDown()
    {
        bgStick.transform.position = Input.mousePosition;
        smallStick.transform.position = Input.mousePosition;
        stickFirstPosition = Input.mousePosition;

        //Set Trigger Walk
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 DragPosition = pointerEventData.position;
        joyVec = (DragPosition - stickFirstPosition).normalized;

        float stickDistance = Vector3.Distance(DragPosition, stickFirstPosition);
        
        if(stickDistance < stickRadius)
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickDistance;
        }
        else
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickRadius;
        }
    }

    public void Drop()
    {
        joyVec = Vector3.zero;
        bgStick.transform.position = joyStickFirstPosition;
        smallStick.transform.position = joyStickFirstPosition;
        //SetTrigger Idle
    }
}
