using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class DrillerScript : MonoBehaviour
{
    public int value = 100;
    public float agentAcceleration = 1;
    public float agentMaxSpeed = 1;
    public float health = 100;
    private float maxHealth;
    public bool isFragile = true; // set to false if you want the driller to not die when it breaks a destructible.
    private GameObject MoneyHandle;

    [SerializeField]
    List<Transform> _destinations = new List<Transform>();

    NavMeshAgent _navMeshAgent;

    void Start()
    {
        maxHealth = health;

        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("The navmesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            InitializeDestination();
        }
        _navMeshAgent.speed = agentMaxSpeed;
        _navMeshAgent.acceleration = agentAcceleration;
        MoneyHandle = GameObject.FindWithTag("Money");
    }

    void Update()
    {
        if (this.health <= 0)
        {
            Destroy(gameObject);
            MoneyHandle.BroadcastMessage("ChangeMoney", value);
        }

        UpdateDestination();
    }

    private void InitializeDestination()
    {
        foreach (GameObject tmpObj in GameObject.FindGameObjectsWithTag("Destructible"))
            _destinations.Add(tmpObj.transform);

        foreach (GameObject tmpObj in GameObject.FindGameObjectsWithTag("Base"))
            _destinations.Add(tmpObj.transform);

        //_destinations = _destinations.OrderBy(x => Vector3.Distance(gameObject.transform.position, x.position)).ToList();

        _destinations.Sort(delegate (Transform a, Transform b)
        {
            return Vector3.Distance(this.transform.position, b.position)
            .CompareTo(
              Vector3.Distance(this.transform.position, a.position));
        });

        _destinations.Reverse();

        UpdateDestination();
    }

    private void UpdateDestination()
    {
        if (!_destinations.Any())
        {
            Debug.LogError("No Destination for " + gameObject.name);
            return;
        }

        while (_destinations[0] == null)
            _destinations.RemoveAt(0);

        _navMeshAgent.SetDestination(_destinations[0].position);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Destructible")
        {
            Destroy(col.gameObject);

            if (this.isFragile)
            {
                Destroy(gameObject);
                return;
            }
        }

        UpdateDestination();
    }
}
