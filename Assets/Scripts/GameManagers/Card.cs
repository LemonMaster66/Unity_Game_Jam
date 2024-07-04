using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image CardImage;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Cost;

    public Ability ability;
    private AbilityManager abilityManager;


    public void Awake()
    {
        abilityManager = FindAnyObjectByType<AbilityManager>();
    }


    public void UpdateCard(Image newImage, string newName, string newDescription, int newCost, Ability newAbility)
    {
        if(newAbility != null)     ability   = newAbility;
        if(newImage != null)       CardImage = newImage;
        if(newName != null)        Name.text = newName;
        if(newDescription != null) Description.text = newDescription;
                                   Cost.text = "Cost: " + newCost + "hp";
    }

    public void ApplyAbility()
    {
        abilityManager.ApplyAbility(ability);
    }
}
