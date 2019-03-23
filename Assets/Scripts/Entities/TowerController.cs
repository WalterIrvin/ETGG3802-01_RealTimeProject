using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject TowerPrefab;
    public GameObject MoneyHandler;
    public GameObject Floor;



    // Update is called once per frame
    void Update()
    {

        // Performs a raycast at the beginning of the update method that all functions can use
        // This will hopefully limit the amount of raycasts that we need to do every frame.
        RaycastHit hit;
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);


        //Placing a tower in the world
        // Mouse 1
        // - except for when you hold alt (that moves the camera)
        if (Input.GetMouseButtonDown(0) && !Input.GetButton("Camera Control Modifier"))
        {
            PlaceTower(hit);
        }

        //Selling a tower
        // Mouse 2 (probably want to change it later but this will work for now)
        if (Input.GetMouseButtonDown(1) && !Input.GetButton("Camera Control Modifier"))
        {
            SellTower(hit);
        }

        //Centered camera rotation
        // Middle Mouse Wheel
        // Alt + Mouse 1
        if ((Input.GetMouseButton(2) && !Input.GetButton("Camera Control Modifier")) || (Input.GetButton("Camera Control Modifier") && Input.GetMouseButton(0)))
        {
            RotateCamera_YAxis(Input.GetAxis("Horizontal") * 10);
            RotateCamera_XAxis(Input.GetAxis("Vertical") * 10);
        }

        //Zooming in and out
        // Mouse wheel up and down
        if (Input.mouseScrollDelta.y != 0)
        {
            ZoomCamera(Input.mouseScrollDelta.y);
        }

        MainCamera.transform.LookAt(new Vector3(0, 1, 0));

    }



    // Places a tower in the game world and adjusts the player's monay accordingly
    private void PlaceTower(RaycastHit hit)
    {
        // If the raycast is valid
        if (hit.collider != null)
        {
            // If the raycast hits a wall
            if (hit.collider.gameObject.tag == "Wall" && hit.collider.gameObject.tag != "Tower")
            {
                //If there's money in  the bank
                MoneyScript M = MoneyHandler.GetComponent<MoneyScript>();
                if (M.Money >= 100)
                {
                    // Add a tower and subtract the cost
                    // To be able to place tower types later, we might want to have a type that we check for here
                    Vector3 pos = hit.collider.gameObject.transform.position;// + Vector3.up;
                    pos.y += 0.325f;
                    //Instantiate(TowerPrefab, pos, Quaternion.identity);
                    //M.Money -= 100;
                }
            }
        }
    }



    // Sells a tower and adjusts the player's money accordingly
    private void SellTower(RaycastHit hit)
    {
        // If the raycast is valid
        if (hit.collider != null)
        {
            // If the raycast hits a tower
            if (hit.collider.gameObject.tag == "Tower")
            {
                Destroy(hit.collider.gameObject);
                MoneyScript M = MoneyHandler.GetComponent<MoneyScript>();
                M.Money += 50;
            }
        }
    }

    

    // Rotates the camera around a point 1 unit above the center of the floor (Horizontal)
    private void RotateCamera_YAxis(float amt)
    {
        MainCamera.transform.RotateAround(Floor.transform.position + new Vector3(0, 1, 0), new Vector3(0, 1, 0), amt * Time.deltaTime);
    }



    // Rotates the camera around a point 1 unit above the center of the floor (Vertical)
    private void RotateCamera_XAxis(float amt)
    {
        MainCamera.transform.RotateAround(Floor.transform.position + new Vector3(0, 1, 0), MainCamera.transform.right, amt * Time.deltaTime);
        //In the future it would be a good idea to make it so that the viewing angles are restricted, but I don't think it's that important right now.
    }



    // Zooms the camera in and out
    private void ZoomCamera(float dir)
    {
        Vector3 cam_to_center = MainCamera.transform.position - new Vector3(0, 1, 0);
        // If distance from center point is not less than three units
        if((cam_to_center - cam_to_center.normalized).magnitude >= 3 || (dir * -1) > 0)
        {
            MainCamera.transform.Translate(cam_to_center.normalized * dir * -1, Space.World);
        }
    }
}
