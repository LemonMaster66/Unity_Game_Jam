using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PalexUtilities;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<Ability> abilities = new List<Ability>();
    public List<Card> cards = new List<Card>();

    public GameObject CardPrefab;

    private GunManager gunManager;
    private Gun ActiveGun;

    void Awake()
    {
        AddAbilites();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) ApplyAbility(abilities[1]);
        if(Input.GetKeyDown(KeyCode.F4))
        {
            Tools.ClearLogConsole();
            StartCoroutine(DisplayRandomAbilities());
        }
    }

    public void AddAbilites()
    {
        abilities.Clear();

        gunManager = FindAnyObjectByType<GunManager>();
        ActiveGun = gunManager.GetGun(gunManager.ActiveGun);

        abilities.Add(new Ability("Damage", "Increases damage by 10",
                                  gun => ActiveGun._damage += 10,
                                  10));
        abilities.Add(new Ability("AttackSpeed", "Decreases AttackSpeed by 0.05f",
                                  gun => ActiveGun.AttackSpeed -= 0.05f,
                                  10));
        abilities.Add(new Ability("Multishot", "Increases Multishot by 1",
                                  gun => ActiveGun.MultiShot += 1,
                                  15));
        abilities.Add(new Ability("Ricochet", "Increases Ricochet Bounces by 1",
                                  gun => ActiveGun.RicochetCount += 1,
                                  5));
        abilities.Add(new Ability("Piercing", "Increases Percing by 1",
                                  gun => ActiveGun.PierceCount += 1,
                                  8));
    }
    public void ApplyAbility(Ability ability)
    {
        ability.Effect(ActiveGun);
    }


    public IEnumerator DisplayRandomAbilities()
    {
        Transform CardDomain = GameObject.Find("Cards").transform;
        foreach(Card card in cards) Destroy(card.gameObject);

        cards.Clear();

        List<Ability> randomAbilities = GetRandomAbilities(3);
        foreach (var ability in randomAbilities)
        {
            yield return new WaitForSeconds(0.12f);

            Debug.Log(ability.Name + ":  " + ability.Description + "  -  Costs: " + ability.Cost + "hp");

            GameObject cardObj = Instantiate(CardPrefab, Vector3.zero, Quaternion.identity, CardDomain);
            Card newCard = cardObj.GetComponent<Card>();

            newCard.UpdateCard(null, ability.Name, ability.Description, ability.Cost, ability);
            cards.Add(newCard);
        }
    }

    List<Ability> GetRandomAbilities(int count)
    {
        return abilities.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
    }
}
