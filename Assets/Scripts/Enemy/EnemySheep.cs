using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySheep : MonoBehaviour
{
    GameObject Player;

    public LayerMask layerMask;

    public GameObject DangerMarker;
    public GameObject EnemyBolt;

    public Transform BoltGenPosition;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(WaitPlayer());
    }
    IEnumerator WaitPlayer()
    {
        yield return null;

        yield return new WaitForSeconds(4f);
        transform.LookAt(Player.transform.position);
        DangerMarkerShoot();

        yield return new WaitForSeconds(2f);
        Shoot();

    }

    void DangerMarkerShoot()
    {
        Vector3 NewPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Physics.Raycast(NewPosition, transform.forward, out RaycastHit hit, 30f, layerMask);

        if (hit.transform.CompareTag("Wall"))
        {
            GameObject DangerMarkerClone = Instantiate(DangerMarker, NewPosition, transform.rotation);
            DangerMarkerClone.GetComponent<DangerLine>().EndPosition = hit.point;
        }
    }

    void Shoot()
    {
        Vector3 CurrentRotation = transform.eulerAngles + new Vector3(-90, 0, 0);
        Instantiate(EnemyBolt, BoltGenPosition.position, Quaternion.Euler(CurrentRotation));
    }
}