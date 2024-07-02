using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using VInspector;

public class TypeWriterFX : MonoBehaviour
{
    [Tab("Main")]
    public bool Active;
    public bool Typing;

    [Min(0)] public float Cooldown;

    [Space(6)]

    public List<string> Dialogue;
    public int Line;


    [Tab("UI Audio")]
    public AudioClip[] TextBoxAppear;
    public AudioClip[] TextBoxDisappear;
    public AudioClip   TextBoxNext;


    [Tab("Settings")]
    public NPC npc;
    [Space(5)]
    public GameObject DialogueBoxPrefab;
    public GameObject DialogueBoxObj;
    public TextMeshProUGUI tmp;
    [Space(5)]
    public PlayerMovement playerMovement;
    public AudioManager audioManager;


    void Awake()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        audioManager = GetComponent<AudioManager>();
    }

    void Update()
    {
        Cooldown = Math.Clamp(Cooldown - Time.deltaTime, 0, 1);
    }


    public void Type()
    {
        playerMovement.Pause(true);

        DialogueBoxObj = Instantiate(DialogueBoxPrefab, transform);
        tmp = DialogueBoxObj.GetComponentsInChildren<TextMeshProUGUI>()[0];
        TextMeshProUGUI tmpName = DialogueBoxObj.GetComponentsInChildren<TextMeshProUGUI>()[1];

        tmpName.text = npc.Name;

        if(!Active)
        {
            audioManager.PlayRandomSound(TextBoxAppear, 1, 1, 0.1f);
            DialogueBoxObj.GetComponent<Animator>().CrossFade("Dialogue Box Appear", 0.05f);
        }

        StartCoroutine(TypeCharacters());
    }

    public IEnumerator TypeCharacters()
    {
        tmp.text = "";

        if(!Active) Cooldown = 0.3f;
        else        Cooldown = 0.1f;
        yield return new WaitForSeconds(Cooldown);

        Active = true;
        Typing = true;

        char[] Characters = Dialogue[Line].ToCharArray();

        foreach(char Character in Characters)
        {
            tmp.text += Character;
            if(SpokenCharacter(Character)) 
            {
                npc.Punch();
                if(npc.audioManager != null) npc.audioManager.PlayRandomSound(npc.SpeakingNoises, 1, 1f, 0.1f);
            }

            yield return new WaitForSeconds(CharacterDelay(Character));
        }

        Typing = false;
    }


    public void Next()
    {
        Line++;

        if(Line >= Dialogue.Count) Finish();
        else
        {
            Destroy(DialogueBoxObj);
            Type();

            audioManager.PlaySound(TextBoxNext, 1, 1, 0.1f);
        }
    }

    public void Skip()
    {
        Typing = false;
        tmp.text = Dialogue[Line];
        StopAllCoroutines();
    }

    public void Finish()
    {
        Line = 0;
        Active = false;
        Cooldown = 0.5f;
        playerMovement.Pause(false);

        DialogueBoxObj.GetComponent<Animator>().CrossFade("Dialogue Box Disappear", 0.05f);
        audioManager.PlayRandomSound(TextBoxDisappear, 1, 1, 0.1f);

        npc.FinishTalking();

        Destroy(DialogueBoxObj, 0.5f);
    }



    public bool SpokenCharacter(char Character)
    {
        switch (Character.ToString())
        {
            case " ": return false;
            case ".": return false;
            case ",": return false;
            default:  return true;
        }
    }

    public float CharacterDelay(char Character)
    {
        switch (Character.ToString())
        {
            case " ": return 0.065f;
            case ".": return 0.25f;
            case ",": return 0.15f;
            default:  return 0.035f;
        }
    }
}
