using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基础属性")] public float maxHealth;
    public float currentHealth;
    [Header("受伤无敌")] public float invulnerableDuration;
    public float invulnerableCounter;
    public bool isInvulnerable;
    public UnityEvent<Transform> onTakeDamage;
    public UnityEvent OnDie;
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
            OnDie?.Invoke();
        }
        else
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable(); //触发无敌
            // 执行受伤
            onTakeDamage?.Invoke(attacker.transform);
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