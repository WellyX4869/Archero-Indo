using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBolt : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.velocity = transform.up * -10f;
        rb.velocity = Vector3.up * -10f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            Destroy(gameObject, 0.1f);
        }
    }
}