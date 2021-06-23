using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class SnapEditor : MonoBehaviour
{
    [SerializeField] [Range(1f, 20f)] float gridSize = 10f;

    TextMesh textMesh;
    Vector3 position;

    private void Start()
    {
        position.x = Mathf.RoundToInt(transform.position.x / gridSize);
        position.y = transform.position.y;
        position.z = Mathf.RoundToInt(transform.position.z / gridSize);
        SnapPosition();
    }

    public float GetGridSize()
    {
        return gridSize;
    }

    private void Update()
    {
        position.x = Mathf.RoundToInt(transform.position.x / gridSize);
        position.y = transform.position.y;
        position.z = Mathf.RoundToInt(transform.position.z / gridSize);
        SnapPosition();
    }

    public void SnapPosition()
    {
        Vector3 snapPos;

        snapPos.x = position.x * gridSize;
        snapPos.z = position.z * gridSize;
        transform.position = new Vector3(snapPos.x, position.y, snapPos.z);

        string labelText = position.x + "," + position.z;
        gameObject.name = labelText;
    }

    public void ChangePosition(Vector3 pos)
    {
        position = pos;
        SnapPosition();
    }
}

