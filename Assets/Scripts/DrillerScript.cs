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
   /* private void SetDestination()
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }
    }*/
}
