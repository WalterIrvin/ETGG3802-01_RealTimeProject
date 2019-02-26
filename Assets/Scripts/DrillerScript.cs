using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DrillerScript : MonoBehaviour
{
    public float health = 100;

    void Start()
    {

    }

    void Update()
    {
        if (this.health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        transform.position += transform.forward * Time.deltaTime;
    }

}
