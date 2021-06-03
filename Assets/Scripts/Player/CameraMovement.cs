using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject Player;

    [SerializeField] float offsetY = 45f;
    [SerializeField] float offsetZ = -40f;

    Vector3 cameraPosition;

    // Update is called once per frame
    void LateUpdate()
    {
        //cameraPosition.x = Player.transform.position.x;
        cameraPosition.y = Player.transform.position.y + offsetY;
        cameraPosition.z = Player.transform.position.z + offsetZ;

        transform.position = cameraPosition;
    }
}
