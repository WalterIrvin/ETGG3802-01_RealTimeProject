using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class world_builder : MonoBehaviour
{
    [SerializeField]
    NavMeshSurface[] navMeshSurfaces;

    public Transform Obstacle;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < navMeshSurfaces.Length; i++)
        {
            navMeshSurfaces[i].BuildNavMesh();
        }
    }

    void Update()
    {

        // If the mouse button is down, add a block if possible
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = (Camera)GameObject.Find("Main Camera").GetComponent(typeof(Camera));

            Ray from_camera = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit result;

            //If we hit the world, do stuff
            if (Physics.Raycast(from_camera, out result))
            {
                if (result.collider.tag == "World")
                {

                    // Snapping to grid
                    // this uses a cheap rounding method I found while screwing around.
                    // The number I get out of this is right, so the issue seems to be
                    // caused by incorrect input data (most likely user error...)
                    int create_x = (int)(result.point.x + .5);
                    int create_z = (int)(result.point.z + .5);
                    create_z -= 1;



                    // Creating the new wall and baking the navmesh around it
                    Transform newbie = Instantiate(Obstacle, new Vector3(create_x, 0, create_z), Quaternion.identity);



                    // Check to see if all enemies still have a path to the exit
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in enemies)
                    {
                        NavMeshAgent tmp = (NavMeshAgent)enemy.GetComponent(typeof(NavMeshAgent));

                        // Checking to see if the object has a NavMeshAgent (if not then there is a problem...)
                        if (tmp != null)
                        {
                            //Calculating the path the object needs to take since it may not have recalculated after the navmesh was rebaked
                            tmp.CalculatePath(tmp.destination, tmp.path);
                            //Checking to see if the enemy has a path to the exit
                            if (tmp.path.status == NavMeshPathStatus.PathPartial)
                            {
                                Debug.Log("Bad Obstacle Placement");

                                Destroy(newbie.gameObject);

                                return;

                            }



                        }



                    }
                }
            }


        }



        // Right Click to destroy walls (not actually a feature, just here for testing)
        if (Input.GetMouseButtonDown(1))
        {
            Camera cam = (Camera)GameObject.Find("Main Camera").GetComponent(typeof(Camera));

            Ray from_camera = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit result;

            if (Physics.Raycast(from_camera, out result))
            {
                if (result.collider.tag == "Wall")
                {
                    Destroy(result.collider.gameObject);
                }
            }
        }
    }
}
