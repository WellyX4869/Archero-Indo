using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<CameraMovement>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("CameraMovement");
                    instance = instanceContainer.AddComponent<CameraMovement>();
                }
            }
            return instance;
        }
    }
    private static CameraMovement instance;

    [SerializeField] GameObject Player;
    [SerializeField] Image FadeInOutImg;

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

    public void CameraNextRoom()
    {
        // Fade in/out
        StartCoroutine(FadeInOut());
        cameraPosition.x = Player.transform.position.x;
    }

    IEnumerator FadeInOut()
    {
        float alpha = 1;
        FadeInOutImg.color = new Vector4(1, 1, 1, alpha);
        yield return new WaitForSeconds(0.3f);

        while (alpha >= 0)
        {
            FadeInOutImg.color = new Vector4(1, 1, 1, alpha);
            alpha -= 0.02f;
            yield return null;
        }
    }
}
