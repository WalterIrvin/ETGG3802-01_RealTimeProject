using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject TowerPrefab;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse clicked");
            RaycastHit hit;
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("2");
                Instantiate(TowerPrefab, hit.point, Quaternion.identity);
                if (hit.transform.name == "Wall")
                {
                    Debug.Log("3");
                    Instantiate(TowerPrefab, hit.point, Quaternion.identity);
                }
            }
        }
    }
}
