using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float delayUntilDestroyed = 3f;
    public float damage = 100f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, delayUntilDestroyed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Projectile")
            Destroy(gameObject);
    }
}
