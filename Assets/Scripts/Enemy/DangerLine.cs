using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : MonoBehaviour
{
    TrailRenderer trailRenderer;
    public Vector3 EndPosition = Vector3.zero;
    public float trailWidth = 10f;

    // Start is called before the first frame update
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.startWidth = trailWidth;
        trailRenderer.endWidth = trailWidth;
        trailRenderer.startColor = new Color(1, 0, 0, 0.7f);
        trailRenderer.endColor = new Color(1, 0, 0, 0.7f);
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, EndPosition, Time.deltaTime * 3.5f);
    }
}
