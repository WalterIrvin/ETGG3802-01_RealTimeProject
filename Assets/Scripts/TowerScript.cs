using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    public float fireDelay;
    public int towerDamage;
    public int splashAmount;
    private System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
    private List<GameObject> hitList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.Elapsed.TotalMilliseconds >= (fireDelay * 1000.0f))
        {
            //removes splashAmount number of enemies from the hitlist, ie first 3 in == first 3 killed
            for(int i = 0; i < splashAmount; i++)
            {
                if (hitList.Count > 0)
                {
                    GameObject tmp = hitList[0];
                    tmp.GetComponent<tmp_EnemyMover>().health -= towerDamage;
                    Debug.Log("Health: " + tmp.GetComponent<tmp_EnemyMover>().health);
                    if(tmp.GetComponent<tmp_EnemyMover>().health <= 0)
                    {
                        Destroy(hitList[0]);
                        hitList.RemoveAt(0);
                    }
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
