using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject prefab;
    public Material buildableMaterial;
    public Material unbuildableMaterial;
    public float gridSpacing = 1.0f;
    public LayerMask buildableLayer;
    private GameObject indicator;

    void Start()
    {
        indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
        indicator.GetComponent<MeshRenderer>().material = unbuildableMaterial;
        indicator.SetActive(false);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildableLayer))
        {
            Vector3 gridPosition = SnapToGrid(hit.point);
            indicator.transform.position = gridPosition;
            indicator.GetComponent<MeshRenderer>().material = buildableMaterial;
            indicator.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                GameObject instance = Instantiate(prefab, gridPosition, Quaternion.identity);
                instance.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            }
        }
        else
        {
            indicator.SetActive(false);
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

