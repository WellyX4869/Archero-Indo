using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.position;
    }
}
