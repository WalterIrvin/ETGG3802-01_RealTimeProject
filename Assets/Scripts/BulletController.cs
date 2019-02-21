using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 mDestination;
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
        transform.position += mDirection * Time.deltaTime;
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
            tmp.GetComponent<tmp_EnemyMover>().health -= 100;
            Destroy(this.gameObject);
        }
    }
}
