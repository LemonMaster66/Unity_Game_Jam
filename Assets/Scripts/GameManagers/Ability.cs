using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ability
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Action<Gun> Effect { get; private set; }
    public int Cost { get; private set; }

    public Ability(string name, string description, Action<Gun> effect, int cost)
    {
        Name = name;
        Description = description;
        Effect = effect;
        Cost = cost;
    }
}
