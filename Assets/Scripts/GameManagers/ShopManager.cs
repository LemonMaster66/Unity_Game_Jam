using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public bool Shopping;

    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private AbilityManager abilityManager;
    private GunManager gunManager;
    private Mouse mouse;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!Shopping) return;
            EndShop();
        }
    }


    void Awake()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerStats = FindAnyObjectByType<PlayerStats>();
        abilityManager = FindAnyObjectByType<AbilityManager>();
        gunManager = FindAnyObjectByType<GunManager>();
        mouse = FindAnyObjectByType<Mouse>();

        StartShop();
    }


    public void StartShop()
    {
        Shopping = true;

        playerMovement.Pause(true);
        mouse.ShowMouse();
        gunManager.GetGun(gunManager.ActiveGun).HoldingShoot = false;
        gunManager.GetGun(gunManager.ActiveGun).HoldingAltShoot = false;

        StartCoroutine(abilityManager.DisplayRandomAbilities(3));
    }

    public void EndShop()
    {
        if(!Shopping) return;
        Shopping = false;

        playerMovement.Pause(false);
        mouse.HideMouse();

        StartCoroutine(abilityManager.DestroyAllCards());

        FindAnyObjectByType<WaveManager>().SpawnWave();
    }

    public void Reroll()
    {
        abilityManager.DisplayRandomAbilities(3);
    }
}
