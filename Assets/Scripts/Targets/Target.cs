using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

[Serializable]
public class Target : MonoBehaviour
{
    [Tab("Main")]
    [Header("Types")]
    public bool Invincible = false;
    public bool ShowDamageIndicator = false;
    [ShowIf("ShowDamageIndicator")] public GameObject damageIndicatorObj;
    [EndIf]

    [Header("Properties")]
    public float Health = 100;

    [Header("States")]
    public bool Dead = false;

    private GameObject dmgIndicator;
    private DamageIndicator dmgIndicatorValues;
    [HideInInspector] public float TotalDamage;
    [HideInInspector] public float MaxHealth;
    [HideInInspector] public float DamageStorageTime;



    public virtual void Awake()
    {
        MaxHealth = Health;
    }
    public virtual void Update()
    {
        if(DamageStorageTime > 0) DamageStorageTime -= Time.deltaTime;
        if(DamageStorageTime < 0)
        {
            DamageStorageTime = 0;
            TotalDamage = 0;
        }
    }


    public virtual void TakeDamage(float Damage, Vector3 HitPoint)
    {
        if(Dead && DamageStorageTime == 0) return;

        TotalDamage += Damage;
        if(!Invincible) Health -= Damage;
        

        if(ShowDamageIndicator)
        {
            if(DamageStorageTime == 0)
            {
                dmgIndicator = Instantiate(damageIndicatorObj, HitPoint, Quaternion.identity);
                dmgIndicatorValues = dmgIndicator.GetComponent<DamageIndicator>();

                dmgIndicatorValues.DamageUpdate(TotalDamage);
            }
            else if(DamageStorageTime != 0)
            {
                dmgIndicatorValues.DamageUpdate(TotalDamage);
            }
        }
        
        if(!Dead) DamageStorageTime = 0.1f;
        if(Health <= 0) Die(TotalDamage);

        Health = (float)Math.Round(Health, 2);
    }

    public virtual void Die(float Damage)
    {
        Dead = true;

        gameObject.SetActive(false);
        Debug.Log(gameObject.name + " is Dead");
    }
}
