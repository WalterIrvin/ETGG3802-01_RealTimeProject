using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    public int fireDelay;
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
        //Debug.Log("Updating TowerScript");
        if(timer.Elapsed.Seconds > fireDelay)
        {
            if(hitList.Count > 0)
            {
                Destroy(hitList[0]);
                hitList.RemoveAt(0);
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
