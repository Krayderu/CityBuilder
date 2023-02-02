using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject prefab;
    public int gridRows = 10;
    public int gridColumns = 10;
    public float gridSpacing = 1.0f;
    public LayerMask buildableLayer;

    void Start()
    {
        for (int i = 0; i < gridRows; i++)
        {
            for (int j = 0; j < gridColumns; j++)
            {
                Vector3 position = new Vector3(i * gridSpacing, 0, j * gridSpacing);
                Instantiate(prefab, position, Quaternion.identity, transform);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildableLayer))
            {
                Vector3 gridPosition = SnapToGrid(hit.point);
                Instantiate(prefab, gridPosition, Quaternion.identity, transform);
            }
        }
    }

    Vector3 SnapToGrid(Vector3 hitPoint)
    {
        float snapX = Mathf.Round(hitPoint.x / gridSpacing) * gridSpacing;
        float snapY = hitPoint.y;
        float snapZ = Mathf.Round(hitPoint.z / gridSpacing) * gridSpacing;
        return new Vector3(snapX, snapY, snapZ);
    }
}
