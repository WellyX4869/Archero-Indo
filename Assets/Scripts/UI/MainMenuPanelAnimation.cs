using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanelAnimation : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] Transform mainMenuPanel;

    [Header("Animation")]
    [SerializeField] Vector3 moveSpeed;
    [SerializeField] Vector2 boundsX;

    int direction = -1;

    // Update is called once per frame
    void Update()
    {
        // Check if moving out of bounds
        if(mainMenuPanel.localPosition.x <= boundsX.x) { direction = 1; }
        if(mainMenuPanel.localPosition.x >= boundsX.y) { direction = -1; }

        // Move Panel
        mainMenuPanel.localPosition += moveSpeed * direction * Time.deltaTime;
    }
}
