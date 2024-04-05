using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    public int curHp;
    public int maxHp;
    public HealthBarUI healthBarUI;
    public TMP_Text healthPercentage;

    [Header("Attack")]
    public GameObject attackPrefab;
    public float timeBetweenAttacks;
    public bool alreadyAttacked;

    protected Character target;

    public event UnityAction onTakeDamage;
    public event UnityAction onHeal;

    public void TakeDamage(int damageToTake)
    {
        curHp -= damageToTake;

        onTakeDamage?.Invoke();
    }

    public void Heal(int healAmount)
    {
        curHp += healAmount;
        onHeal?.Invoke();

        if (curHp >= maxHp)
            curHp = maxHp;
    }
}
