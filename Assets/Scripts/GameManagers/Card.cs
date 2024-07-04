using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PalexUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public GameObject CardVisualPrefab;
    public CardVisual cardVisual;

    public bool Selected = false;

    private Transform VisualManager;
    private AbilityManager abilityManager;
    [HideInInspector] public Ability ability;

    private ShopManager shopManager;


    public void Initiate()
    {
        VisualManager = GameObject.Find("Card Visuals").transform;
        abilityManager = FindAnyObjectByType<AbilityManager>();
        shopManager = FindAnyObjectByType<ShopManager>();

        GameObject cardVisualObj = Instantiate(CardVisualPrefab, new Vector3(Screen.width / 2, Screen.height / 2, 0f), Quaternion.identity, VisualManager);
        cardVisual = cardVisualObj.GetComponent<CardVisual>();

        cardVisual.parentCard = this;
        cardVisual.parentCardTransform = transform;

        cardVisual.UpdateCard(null, ability.Name, ability.Description, ability.Cost);
    }

    public void ApplyAbility()
    {
        abilityManager.ApplyAbility(ability);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Selected = !Selected;

        DOTween.CompleteAll(true);
        cardVisual.ShakeTransform.transform.DOPunchRotation(Tools.RandomVector3(25), 0.2f, Random.Range(10, 20), 1);
        cardVisual.ShakeTransform.transform.DOPunchScale(new Vector3(0.1f,0.1f,0.1f), 0.2f, Random.Range(10, 20), 1);

        ApplyAbility();
        shopManager.EndShop();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DOTween.CompleteAll(true);
        cardVisual.ShakeTransform.transform.DOPunchRotation(Tools.RandomVector3(10), 0.2f, Random.Range(10, 20), 1);
        cardVisual.ShakeTransform.transform.DOPunchScale(new Vector3(0.05f,0.05f,0.05f), 0.2f, Random.Range(10, 20), 1);
    }
}
