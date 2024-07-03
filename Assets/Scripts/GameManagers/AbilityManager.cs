using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PalexUtilities;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<Ability> abilities = new List<Ability>();

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
            DisplayRandomAbilities();
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
                                  10));
    }
    void ApplyAbility(Ability ability)
    {
        ability.Effect(ActiveGun);
    }


    void DisplayRandomAbilities()
    {
        List<Ability> randomAbilities = GetRandomAbilities(3);
        foreach (var ability in randomAbilities)
        {
            Debug.Log(ability.Name + ": " + ability.Description + "  -  Costs: " + ability.Cost + "hp");
        }
    }

    List<Ability> GetRandomAbilities(int count)
    {
        return abilities.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
    }
}
