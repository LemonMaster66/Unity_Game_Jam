using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public AudioClip[] steps;

    private AudioManager audioManager;
    private Enemy enemy;

    void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        enemy = GetComponentInParent<Enemy>();
    }

    public void EnemyFootstepSfx()
    {
        audioManager.PlayRandomSound(steps, enemy.animator.GetFloat("Blend"), 1, 0.1f);
    }
}
