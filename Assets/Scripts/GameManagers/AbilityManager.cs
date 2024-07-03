using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<Ability> abilities = new List<Ability>();

    private GunManager gunManager;
    private Gun ActiveGun;

    void Awake()
    {
        //AddAbilites();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            AddAbilites();
            ApplyAbility(abilities[1]);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddAbilites();
            ApplyAbility(abilities[0]);
        }
    }

    public void AddAbilites()
    {
        abilities.Clear();

        gunManager = FindAnyObjectByType<GunManager>();
        ActiveGun = gunManager.GetGun(gunManager.ActiveGun);

        abilities.Add(new Ability("Damage Boost", "Increases damage by 10",
                                  gun => ActiveGun._damage += 10,
                                  10));
        abilities.Add(new Ability("AttackSpeed Boost", "Increases AttackSpeed by 20",
                                  gun => ActiveGun.AttackSpeed -= 0.05f,
                                  10));
    }


    void ApplyAbility(Ability ability)
    {
        ability.Effect(ActiveGun);
        Debug.Log(ability.Description);
    }
}
