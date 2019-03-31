using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class DrillerScript : MonoBehaviour
{
    public int value = 100;
    public float agentAcceleration = 1;
    public float agentMaxSpeed = 1;
    public float health = 100;
    private float maxHealth;
    private Image healthBar;
    public bool isFragile = true; // set to false if you want the driller to not die when it breaks a destructible.
    private GameObject MoneyHandle;
    //private AudioSource source;

    [SerializeField]
    List<Transform> _destinations = new List<Transform>();

    NavMeshAgent _navMeshAgent;

    public ParticleSystem DestructionEffect;


    void Start()
    {
        //source = GetComponent<AudioSource>();

        maxHealth = health;
        healthBar = transform.Find("EnemyCanvas").Find("healthBG").Find("health").GetComponent<Image>();

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
        UpdateDestination();
    }

    void LateUpdate()
    {
        if (this.health <= 0)
        {
            //source.Play(0);
            ParticleSystem explosionEffect = Instantiate(DestructionEffect) as ParticleSystem;
            explosionEffect.transform.position = transform.position;
            Destroy(gameObject);
            MoneyHandle.BroadcastMessage("ChangeMoney", value); // causes null reference exception
        }
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
                ParticleSystem explosionEffect = Instantiate(DestructionEffect) as ParticleSystem;
                explosionEffect.transform.position = transform.position;
                Destroy(gameObject);
                return;
            }
            UpdateDestination();
        }

    }

    public void dmgHealth(int damage)
    {
        //Enemies damage function, takes away what amount of health is determined by the abuser
        health -= damage;
        //Resizing our healthbar
        healthBar.fillAmount = (float)health / (float)maxHealth;
    }
}
