using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float delayUntilDestroyed = 3f;
    public bool isBouncy = false;
    public float damage = 100f;

    Rigidbody rb;
    int bounceTimes = 3;
    private float projectileSpeed = 2000f;
    Vector3 newDir;

    // Start is called before the first frame update
    void Start()
    {
        if (isBouncy)
        {
            projectileSpeed = transform.parent.GetComponentInChildren<EnemyController>().projectileSpeed;
            rb = GetComponent<Rigidbody>();
            newDir = transform.forward;
            rb.velocity = newDir;
        }
        else
        {
            Destroy(gameObject, delayUntilDestroyed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isBouncy && collision.gameObject.tag != "Projectile")
        {
            Destroy(gameObject);
        }
        else
        {
            if(collision.gameObject.CompareTag("Wall") && bounceTimes > 0)
            {
                bounceTimes--;
                Vector3 reflectDir = Vector3.Reflect(transform.forward, collision.contacts[0].normal).normalized;
                float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, rot, 0);
                rb.AddForce(transform.eulerAngles * projectileSpeed);
            }
            
            if(collision.gameObject.tag != "Wall" || bounceTimes <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawRay(transform.position, rb.velocity * 10f);
    //}
}
