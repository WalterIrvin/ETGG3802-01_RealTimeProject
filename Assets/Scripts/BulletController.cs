using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 mDestination;
    public int mDamage = 0;
    private Vector3 mDirection;
    private float mAliveTimer = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        mDirection = mDestination - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (mDirection * Time.deltaTime) * 6;
        mAliveTimer -= Time.deltaTime;
        if(mAliveTimer < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject tmp = other.gameObject;
        if (Equals(tmp.tag, "Enemy"))
        {
            tmp.GetComponent<EnemyMover>().health -= mDamage;
            Destroy(this.gameObject);
        }
        else if (Equals(tmp.tag, "Driller"))
        {
            Debug.Log("mDamge:" + mDamage);
            tmp.GetComponent<DrillerScript>().health -= mDamage;
            Destroy(this.gameObject);
        }
    }
}
