using System;
using UnityEngine;
using UnityEngine.AI;
using VInspector;
using PalexUtilities;

public class Enemy : Target
{
    [Tab("Main")]
    public bool Active = true;
    public bool IgnorePlayer;

    [Variants("Searching", "Chasing")]
    public string State;


    [Header("Pathing")]
    public Transform   Target;
    public Transform   LastTarget;
    public Collider[]  Points;
    public LayerMask   OcclusionLayerMask;

    [Space(5)]
    
    


    [Tab("Audio")]
    public AudioClip[] StepSFX;
    public AudioClip[] AttackSFX;


    [Tab("Settings")]
    public PlayerMovement   playerMovement;
    public PlayerStats      playerStats;
    public Camera           cam;
    public NavMeshAgent     agent;
    public Animator         animator;
    public AudioManager     audioManager;
    [Space(8)]
    public float SearchDuration;
    public float AttackCooldown;

    [Space(10)]

    public float animationSpeedTarget;
    public float animationSpeedBlend;

    private string STATE_SEARCHING = "Searching";
    private string STATE_CHASING   = "Chasing";

    

    public override void Awake()
    {
        base.Awake();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerStats    = FindAnyObjectByType<PlayerStats>();
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        audioManager = GetComponentInChildren<AudioManager>();
    }

    public override void Update()
    {
        base.Update();

        if(SearchDuration > 0) SearchDuration = Math.Clamp(SearchDuration - Time.deltaTime, 0, 100);
        if(AttackCooldown > 0) AttackCooldown = Math.Clamp(AttackCooldown - Time.deltaTime, 0, 100);

        if(animator != null)
        {
            animator.SetFloat("Blend", agent.velocity.magnitude/agent.speed, 0.05f, Time.deltaTime);
            animator.speed = Math.Clamp(AttackCooldown > 0 ? 1 : agent.velocity.magnitude/25, 0, 3);
        }
    }


    void FixedUpdate()
    {
        if(playerStats.Dead || !Active) return;

        agent.SetDestination(Target.position);

        //See Player
        if(!IgnorePlayer && AttackCooldown == 0)
        {
            if(!Tools.OcclusionCheck(Points, playerMovement.Camera, 1000000, OcclusionLayerMask))
            {
                SetState(STATE_CHASING);
                Target.position = playerMovement.transform.position;
            }
        }

        //Next Movement
        if(Tools.CalculatePathDistance(transform.position, agent.destination, agent) < 4)
        {
            NextMove();
            
            if(State == STATE_CHASING)
                if(AttackCooldown == 0 && Vector3.Distance(transform.position, playerMovement.transform.position) < 6) Attack(10);
        }
    }


    public Vector3 RandomNavmeshLocation(float Range = 20, float MinDistance = 0, float MaxDirectionDotDifference = -1)
    {
        int Iterations = 0;

        while(Iterations < 20)
        {
            Iterations++;
            Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * Range;
            Vector3 dir = new Vector3(randomDirection.x, 0, randomDirection.y);

            dir += transform.position;
            if (NavMesh.SamplePosition(dir, out NavMeshHit hit, Range, 1))
            {
                Vector3 LastDir = Vector3.Normalize(Target.position - transform.position);
                Vector3 NextDir = Vector3.Normalize(hit.position    - transform.position);

                if(Tools.CalculatePathDistance(Target.position, hit.position, agent) < MinDistance) continue;
                if(Tools.CalculatePathDistance(Target.position, hit.position, agent) > Range+3)     continue;
                
                if(Vector3.Dot(LastDir, NextDir) < MaxDirectionDotDifference && Iterations < 20) continue;

                return hit.position;
            }
            return RandomNavmeshLocation(Range, MinDistance, MaxDirectionDotDifference);
        }
        // Failed all Checks
        return RandomNavmeshLocation(Range+6, MinDistance-5, MaxDirectionDotDifference + 0.25f);
    }

    
    public void NextMove() // What to do when you reach the Target
    {
        if(AttackCooldown > 0) return;

        if(State == STATE_SEARCHING) MoveUpdate();
        if(State == STATE_CHASING)
        {
            if(!Tools.OcclusionCheck(Points, playerMovement.Camera, 1000000, OcclusionLayerMask)) Target.position = playerMovement.transform.position;
            else
            {
                SetState("Searching");
                MoveUpdate();
            }
        }
    }
    public void MoveUpdate() // Where to move based on State
    {
        if(State == "Searching") Target.position = RandomNavmeshLocation(30, 8, 0.4f);
        if(State == "Chasing")
        {
            if(!Tools.OcclusionCheck(Points, playerMovement.Camera, 1000000, OcclusionLayerMask)) Target.position = playerMovement.transform.position;
            else
            {
                SetState("Searching");
                MoveUpdate();
            }
        }
    }
    public void SetState(string state)
    {
        State = state;
        // if(state == "Searching")
        // if(state == "Chasing")
    }


    public virtual void Attack(float Damage = 100)
    {
        AttackCooldown = 1.25f;
        playerStats.TakeDamage(Damage);

        animator.CrossFade("Enemy_Attack", 0.1f);
        audioManager.PlayRandomSound(AttackSFX, 1, 1, 0.1f);
    }
}
