using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 mDestination;
    public int mDamage = 0;
    private Vector3 mDirection;
    private float mAliveTimer = 5.0f;

    public MODIFIER_EFFECT bulletEffect;
    public float effectTimer;

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
        if (Equals(tmp.tag, "Enemy") || Equals(tmp.tag, "Driller"))
        {
            if(bulletEffect != MODIFIER_EFFECT.MOD_NONE)
            {
                tmp.BroadcastMessage("SetStatus", bulletEffect);
                tmp.BroadcastMessage("SetStatusTimer", effectTimer);
            }

            tmp.BroadcastMessage("dmgHealth", mDamage);
            Destroy(this.gameObject);
        }
    }
}
