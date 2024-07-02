using System;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class PlayerStats : MonoBehaviour
{
    [Tab("Main")]
    [Header("Properties")]
    public float Health        = 100;
    public float Mana          = 100;
    public int   Collectables  = 0;


    [Header("States")]
    public bool Dead = false;


    [Tab("Settings")]
    public PlayerMovement playerMovement;
    public PlayerSFX      playerSFX;
    public Image _damageScreen;
    public Image _deathScreen;
    public GameObject textPopup;



    void Awake()
    {
        //Assign Scripts
        playerSFX      = FindAnyObjectByType<PlayerSFX>();
        playerMovement = GetComponent<PlayerMovement>();

        GameObject canvas = GameObject.Find("Canvas");
        if(canvas != null) _damageScreen = canvas.transform.GetChild(0).GetComponent<Image>();
        if(canvas != null) _deathScreen  = canvas.transform.GetChild(1).GetComponent<Image>();
    }

    void Update()
    {
        if(!Dead) 
        {
            Health = Math.Clamp(Health + Time.deltaTime*2, 0, 100);
            if(_damageScreen != null) _damageScreen.color = new Color(1,1,1, (Health / 80*-1)+1);
        }
        else if(_damageScreen != null) _damageScreen.color = new Color(1,1,1,0);
    }


    public void TakeDamage(float Damage = 100)
    {
        Health -= Damage;
        playerSFX.enemy.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(-2);
        playerSFX.PlaySound(playerSFX.Damage);
        if(Health < 0) Die();
    }
    public void Die()
    {
        Dead = true;
        _deathScreen.color = new Color(1,1,1,1);
        playerSFX.enemy.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(-5);
        playerSFX.StopSound(playerSFX.Damage);
        playerSFX.PlaySound(playerSFX.Death);
    }

    
    public void ObtainCollectable()
    {
        Collectables++;
        playerSFX.PlayRandomSound(playerSFX.ObtainCollectableSFX, 1, 1, 0.1f);
    }


    public void SpawnTextUI(string text)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject MoneyTextObj = Instantiate(textPopup, canvas.transform.position, Quaternion.identity, canvas.transform);
        MoneyTextObj.GetComponent<TextMeshProUGUI>().text = text;
        MoneyTextObj.transform.position = new Vector3(50, 25, 0);
    }
}
