﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class tmp_EnemyMover : MonoBehaviour
{
    public float health = 100;
    private float maxHealth = 100;
    private Image healthBar;

    [SerializeField]
    public Transform _destination;

    NavMeshAgent _navMeshAgent;

    void Start()
    {
        //Enemy healthbar setup
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
    }

    void Update()
    {
        if (this.health <= 0)
        {
            Destroy(gameObject);
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
