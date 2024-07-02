using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using VInspector;

public class NPC : Interactable
{
    public string Name;
    public List<string> Dialogue;
    public AudioClip[] SpeakingNoises;

    public bool RelocateOnFinish;
    [ShowIf("RelocateOnFinish")] public GameObject DisappearParticle;
    [ShowIf("RelocateOnFinish")] public GameObject NewNPC;
    [EndIf]

    public bool PlaySoundOnAwake;
    [ShowIf("PlaySoundOnAwake")] public AudioClip[] SpawnSFX;

    [HideInInspector] public AudioManager audioManager;


    public void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        if(PlaySoundOnAwake) audioManager.PlayRandomSound(SpawnSFX, 1, 1, 0.1f);
    }


    public override void InteractStart()
    {
        GameObject canvas = GameObject.Find("Canvas");
        TypeWriterFX typeWriter = canvas.GetComponent<TypeWriterFX>();

        if(typeWriter != null)
        {
            typeWriter.npc = this;
            typeWriter.Dialogue.Clear();
            foreach(string Line in Dialogue) typeWriter.Dialogue.Add(Line);

            if(typeWriter.Cooldown > 0) return;

            if(!typeWriter.Active) typeWriter.Type();
            else if(!typeWriter.Typing) typeWriter.Next();
            else typeWriter.Skip();
        }
    }

    public virtual void Punch()
    {
        DOTween.CompleteAll(true);
        transform.DOPunchPosition(new Vector3(0,2,0), 0.15f, Random.Range(15, 30), 1);
        transform.DOPunchRotation(new Vector3(0,0,Random.Range(-100, 100)), 1, 0, 0);
        transform.DOPunchScale(RandomVector3(0.2f, 0.2f), 0.1f, 0, 0);

        //transform.parent.
    }

    public Vector3 RandomVector3(float value, float RandomOffset)
    {
        return new Vector3(value += Random.Range(-RandomOffset, RandomOffset),
                           value += Random.Range(-RandomOffset, RandomOffset),
                           value += Random.Range(-RandomOffset, RandomOffset));
    }

    public void FinishTalking()
    {
        if(RelocateOnFinish)
        {
            DOTween.CompleteAll();
            Instantiate(DisappearParticle, transform.position, Quaternion.identity, null);
            if(NewNPC != null) NewNPC.SetActive(true);
            Destroy(gameObject);
        }
    }
}
