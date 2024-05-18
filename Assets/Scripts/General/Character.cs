using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("基础属性")] public float maxHealth;
    public float currentHealth;
    [Header("受伤无敌")] public float invulnerableDuration;
    public float invulnerableCounter;
    public bool isInvulnerable;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isInvulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                isInvulnerable = false;
            }
        }
    }

    public void TackDamage(Attack attacker)
    {
        if (isInvulnerable)
        {
            return;
        }

        if (currentHealth - attacker.damage <= 0)
        {
            currentHealth = 0;
            // Die();
        }
        else
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable(); //触发无敌
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 触发无敌
    /// </summary>
    private void TriggerInvulnerable()
    {
        if (!isInvulnerable)
        {
            isInvulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}