using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private AbilityManager abilityManager;
    private Mouse mouse;


    void Awake()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerStats = FindAnyObjectByType<PlayerStats>();
        abilityManager = FindAnyObjectByType<AbilityManager>();
        mouse = FindAnyObjectByType<Mouse>();
    }


    public void StartShop()
    {
        playerMovement.Pause(true);
        mouse.ShowMouse();
    }

    public void EndShop()
    {
        playerMovement.Pause(false);
        mouse.HideMouse();
    }

    public void Reroll()
    {
        
    }
}
