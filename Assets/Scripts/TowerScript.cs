using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    public float fireDelay;
    
    public GameObject mProjectile;
    public int towerDamage;
    private System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
    private List<GameObject> hitList = new List<GameObject>();
    private GameObject mTarget;
    // Start is called before the first frame update
    void Start()
    {
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hitList.Count; i++)
        {
            if (hitList[i] != null)
            {

                GameObject tmp = hitList[i];
                if (tmp.GetComponent<tmp_EnemyMover>().health <= 0)
                {
                    Destroy(tmp);
                    hitList.RemoveAt(i);
                }
            }
            else
            {
                hitList.RemoveAt(i);
            }
        }

        ///Fire control
        if (timer.Elapsed.TotalMilliseconds >= (fireDelay * 1000.0f))
        {
            //removes splashAmount number of enemies from the hitlist, ie first 3 in == first 3 killed
            
            if (hitList.Count > 0)
            {
                float cur_distance;
                float old_distance;
                for(int i = 0; i < hitList.Count; i++)
                {
                    if(hitList[i] != null)
                    {
                        GameObject tmp_target = hitList[i];
                        if (mTarget == null)
                        {
                            mTarget = tmp_target;
                        }
                        else
                        {
                            cur_distance = (tmp_target.transform.position - this.gameObject.transform.position).magnitude;
                            old_distance = (mTarget.transform.position - this.gameObject.transform.position).magnitude;
                            if (cur_distance < old_distance)
                            {
                                mTarget = tmp_target;
                            }
                        }
                    } 
                }
                
                GameObject tmp = mTarget;
                if (tmp != null)
                {
                    Vector3 destination = tmp.GetComponent<tmp_EnemyMover>().transform.position;
                    GameObject the_bullet = Instantiate(mProjectile, transform.position, Quaternion.identity);
                    the_bullet.GetComponent<BulletController>().mDestination = destination;
                }

            }
            

            timer.Reset();
            timer.Start();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject tmp = other.gameObject;
        if(Equals(tmp.tag, "Enemy"))
        {
            hitList.Add(tmp);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject tmp = other.gameObject;
        if(Equals(tmp.tag,"Enemy"))
        {
            hitList.Remove(tmp);
        }
    }
}
