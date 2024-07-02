using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VInspector;

public class Visualisers : MonoBehaviour
{
    [Variants("null")]
    public string Type;

    // Update is called once per frame
    void Awake()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        // if(Type == "Money")            textMeshPro.text = Type + ": " + playerStats.Money + "";
        // if(Type == "Camera Film")      textMeshPro.text = Type + ": " + (cameraManager.FilmLength + (10 * playerStats.extraFilm)) + "";
        // if(Type == "Render Speed")     textMeshPro.text = Type + ": " + cameraManager.CaptureCooldown + "";
        // if(Type == "Speed")            textMeshPro.text = Type + ": " + playerMovement.Speed + "";
        // if(Type == "Smoke Bombs")      textMeshPro.text = Type + ": " + playerStats.SmokeBomb + "";
    }
}
