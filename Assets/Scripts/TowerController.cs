using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject TowerPrefab;
    public GameObject MoneyHandler;



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
                //Instantiate(TowerPrefab, hit.point, Quaternion.identity);
                if (hit.collider.gameObject.tag == "Wall")
                {
                    Debug.Log("3");
                    MoneyScript M = MoneyHandler.GetComponent<MoneyScript>();
                    if (M.Money >= 100)
                    {
                        Instantiate(TowerPrefab, hit.collider.gameObject.transform.position + Vector3.up, Quaternion.identity);
                        M.Money -= 100;
                    }
                }
            }
        }
    }
}
