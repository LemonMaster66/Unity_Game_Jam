using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public GameObject CardVisualPrefab;
    public CardVisual cardVisual;

    public Ability ability;
    private Transform VisualManager;
    private AbilityManager abilityManager;


    public void Initiate()
    {
        VisualManager = GameObject.Find("Card Visuals").transform;
        abilityManager = FindAnyObjectByType<AbilityManager>();

        GameObject cardVisualObj = Instantiate(CardVisualPrefab, new Vector3(Screen.width / 2, Screen.height / 2, 0f), Quaternion.identity, VisualManager);
        cardVisual = cardVisualObj.GetComponent<CardVisual>();

        cardVisual.parentCard = this;
        cardVisual.cardTransform = transform;

        cardVisual.UpdateCard(null, ability.Name, ability.Description, ability.Cost);
    }

    public void ApplyAbility()
    {
        abilityManager.ApplyAbility(ability);
    }
}
