using System;
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
    public float maxHealth;
    private Image healthBar;
    private GameObject MoneyHandle;
    //private AudioSource source;

    private MODIFIER_EFFECT statusEffect;
    private float statusTimer;
    private bool changeTimer;

    //private List<EntityModifierRelay> statusModifiers;
    //private Dictionary<MODIFIER_EFFECT, EntityModifierRelay> statusModifiers = new Dictionary<MODIFIER_EFFECT, EntityModifierRelay>();


    [SerializeField]
    public Transform _destination;

    NavMeshAgent _navMeshAgent;

    public ParticleSystem DestructionEffect;

    void Start()
    {
        //source = GetComponent<AudioSource>();

        statusEffect = MODIFIER_EFFECT.MOD_NONE;
        statusTimer = 0;
        changeTimer = false;

        //Enemy healthbar setup
        maxHealth = health;
        healthBar = transform.Find("EnemyCanvas").Find("healthBG").Find("health").GetComponent<Image>();
        healthBar.fillAmount = (float)health / (float)maxHealth;

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

    void LateUpdate()
    {
        _navMeshAgent.speed = agentMaxSpeed;
        ProcessStatus();

        if (this.health <= 0)
        {
            //source.Play(0);
            ParticleSystem explosionEffect = Instantiate(DestructionEffect) as ParticleSystem;
            explosionEffect.transform.position = transform.position;
            Destroy(gameObject);
            MoneyHandle.BroadcastMessage("ChangeMoney", value); // causes null reference exception
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

    private void SetStatus(MODIFIER_EFFECT newEffect)
    {
        if(statusEffect == MODIFIER_EFFECT.MOD_SLOW_HEAVY && newEffect == MODIFIER_EFFECT.MOD_SLOW)
        {
            changeTimer = false;
        }
        else
        {
            statusEffect = newEffect;
            changeTimer = true;
        }
    }

    private void SetStatusTimer(float newTimer)
    {
        if(changeTimer)
            statusTimer = newTimer;
    }

    private void dmgHealth(int damage)//, MODIFIER_EFFECT effect, float effectTime)
    {
        //Enemies damage function, takes away what amount of health is determined by the abuser
        health -= damage;
        
        //Resizing our healthbar
        if(healthBar != null)
            healthBar.fillAmount = (float)health / (float)maxHealth;
    }

    private void ProcessStatus()
    {
        if(statusEffect == MODIFIER_EFFECT.MOD_NONE)
            return;

        statusTimer -= Time.deltaTime;
        if(statusTimer <= 0)
        {
            statusEffect = MODIFIER_EFFECT.MOD_NONE;
            statusTimer = 0;
            return;
        }

        switch(statusEffect)
        {
            case MODIFIER_EFFECT.MOD_SLOW:
                _navMeshAgent.speed = agentMaxSpeed * 0.5f;
                break;

            case MODIFIER_EFFECT.MOD_SLOW_HEAVY:
                _navMeshAgent.speed = agentMaxSpeed * 0.2f;
                break;

            default:
                break;
        }
    }

    /*
    private void dmgSlow(int damage)
    {
        _navMeshAgent.speed = agentMaxSpeed * slowMultiplier;
           //Enemies damage function, takes away what amount of health is determined by the abuser
           health -= damage;
        //Resizing our healthbar
        if (healthBar != null)
            healthBar.fillAmount = (float)health / (float)maxHealth;
    }
    */

    /*
    public void AddModifier(EntityModifier newModifier)
    {
        EntityModifierRelay newMod;

        if(statusModifiers.ContainsKey(newModifier.modEffect) && newModifier.overrideValue <= statusModifiers[newModifier.modEffect].entMod.overrideValue)
        {
            if(newModifier.overrideValue == statusModifiers[newModifier.modEffect].entMod.overrideValue)
            {
                // Extend duration //
                newMod = statusModifiers[newModifier.modEffect];
                newMod.currentTimeLeft += newModifier.startTime;
                if(newMod.currentTimeLeft > newMod.entMod.maxTimeLeft)
                    newMod.currentTimeLeft = newMod.entMod.maxTimeLeft;
            }
        }
        else
        {
            // Add/Replace modifier //
            newMod.entMod = newModifier;
            newMod.currentTimeLeft = newModifier.startTime;

            statusModifiers[newModifier.modEffect] = newMod;
        }
    }

    private void ProcessModifiers()
    {
        if(statusModifiers.Count == 0)
            return;

        EntityModifierRelay EMR;

        // Update //
        foreach(KeyValuePair<MODIFIER_EFFECT, EntityModifierRelay> E in statusModifiers)
        {
            EMR = E.Value;
            switch (EMR.entMod.modType)
            {
                case MODIFIER_TYPE.MOD_T_CONST:
                    break;

                case MODIFIER_TYPE.MOD_T_TIMED:
                    EMR.currentTimeLeft -= Time.deltaTime;
                    break;

                case MODIFIER_TYPE.MOD_T_DECAY:
                    EMR.currentTimeLeft -= Time.deltaTime;
                    break;
            }
            statusModifiers[E.Key] = EMR;
        }

        // Remove //
        foreach (KeyValuePair<MODIFIER_EFFECT, EntityModifierRelay> E in statusModifiers)
        {
            if(statusModifiers[E.Key].currentTimeLeft <= 0)
            {
                foreach(EntityModifier EM in E.Value.entMod.timeEndEffects)
                {
                    AddModifier(EM);
                }
                statusModifiers.Remove(E.Key);
            }
        }

        _navMeshAgent.speed        = agentMaxSpeed;
        _navMeshAgent.acceleration = agentAcceleration;

        // Add effects //
        foreach (KeyValuePair<MODIFIER_EFFECT, EntityModifierRelay> E in statusModifiers)
        {
            EMR = E.Value;
            switch (EMR.entMod.modEffect)
            {
                case MODIFIER_EFFECT.MOD_E_SLOW:
                    if(!statusModifiers.ContainsKey(MODIFIER_EFFECT.MOD_E_FREEZE))
                    {
                        float someFloat = agentMaxSpeed * EMR.entMod.modValue;

                        if(EMR.entMod.modType == MODIFIER_TYPE.MOD_T_DECAY)
                        {
                            someFloat *= (EMR.currentTimeLeft / EMR.entMod.maxTimeLeft);

                            _navMeshAgent.speed        = agentMaxSpeed     * someFloat;
                            _navMeshAgent.acceleration = agentAcceleration * someFloat;
                        }
                        else
                        {
                            _navMeshAgent.speed        = agentMaxSpeed     * someFloat;
                            _navMeshAgent.acceleration = agentAcceleration * someFloat;
                        }
                    }

                    break;

                case MODIFIER_EFFECT.MOD_E_FREEZE:
                    if(!statusModifiers.ContainsKey(MODIFIER_EFFECT.MOD_E_ANTIFREEZE))
                    {
                        _navMeshAgent.speed = 0;
                        _navMeshAgent.acceleration = 0;
                    }
                    break;

                case MODIFIER_EFFECT.MOD_E_ANTIFREEZE:
                    // Doesn't do anything just prevents freezing //
                    break;
            }
        }
       
    }
    */
}
