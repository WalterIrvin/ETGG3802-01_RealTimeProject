using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMover : MonoBehaviour
{
    public int value = 100;
    public float agentAcceleration = 1;
    public float agentMaxSpeed = 1;
    public float health = 100;
    private float maxHealth;
    private Image healthBar;
    private GameObject MoneyHandle;

    [SerializeField]
    public Transform _destination;

    NavMeshAgent _navMeshAgent;

    public ParticleSystem DestructionEffect;

    void Start()
    {
        //Enemy healthbar setup
        maxHealth = health;
        healthBar = transform.Find("EnemyCanvas").Find("healthBG").Find("health").GetComponent<Image>();

        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("The navmesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            SetDestination();
        }
        _navMeshAgent.speed = agentMaxSpeed;
        _navMeshAgent.acceleration = agentAcceleration;
        MoneyHandle = GameObject.FindWithTag("Money");
    }

    void Update()
    {
        if (this.health <= 0)
        {
            ParticleSystem explosionEffect = Instantiate(DestructionEffect) as ParticleSystem;
            explosionEffect.transform.position = transform.position;
            Destroy(gameObject);
            MoneyHandle.BroadcastMessage("ChangeMoney", value);
        }
    }

    private void SetDestination()
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }
    }

    private void dmgHealth(int damage)
    {
        //Enemies damage function, takes away what amount of health is determined by the abuser
        health -= damage;
        //Resizing our healthbar
        healthBar.fillAmount = (float)health / (float)maxHealth;
    }
}
