using System;
using UnityEngine;
using UnityEngine.AI;
using VInspector;
using PalexUtilities;

public class Enemy : MonoBehaviour
{
    [Tab("Main")]
    public Transform   Target;
    public Transform   LastTarget;
    public Collider[]  Points;
    public LayerMask   OcclusionLayerMask;

    [Space(10)]

    [Header("Properties")]
    public float CurrentNoisePriority;
    public float SearchDuration;
    public float AttackCooldown;
    public float AngryCountdown;


    [Header("States")]
    [Variants("Wandering", "Searching", "Hearing", "Chasing", "Nigerundayo", "Enraged")]
    public string State;

    [Space(5)]
    
    public bool Active = true;
    public bool IgnorePlayer;


    [Tab("Audio")]
    public AudioSource WalkingSFX;
    public AudioClip Spooked;
    public AudioClip Enraged;
    public AudioClip SuccessfulAttack;


    [Tab("Settings")]
    public PlayerMovement   playerMovement;
    public PlayerStats      playerStats;
    public Camera           cam;
    public NavMeshAgent     agent;
    public Animator         animator;
    public AudioManager     audioManager;
    public NavMeshObstacle  navMeshObstacle;

    [Space(10)]

    public float animationSpeedTarget;
    public float animationSpeedBlend;

    


    void Awake()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerStats    = FindAnyObjectByType<PlayerStats>();
        cam = Camera.main;
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        audioManager = GetComponentInChildren<AudioManager>();
        FindAnyObjectByType<PlayerSFX>().enemy = this;
        navMeshObstacle = playerMovement.GetComponent<NavMeshObstacle>();

        WalkingSFX = GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        if(SearchDuration > 0) SearchDuration = Math.Clamp(SearchDuration - Time.deltaTime, 0, 100);
        if(AttackCooldown > 0) AttackCooldown = Math.Clamp(AttackCooldown - Time.deltaTime, 0, 100);

        if(AttackCooldown > 0)
        {
            navMeshObstacle.enabled = true;
            navMeshObstacle.radius = Math.Clamp(navMeshObstacle.radius + Time.deltaTime*20, 0, 30);
        }
        else
        {
            navMeshObstacle.enabled = false;
            navMeshObstacle.radius = Math.Clamp(navMeshObstacle.radius - Time.deltaTime*30, 0, 30);
        }

        if(AngryCountdown > 0) SetState("Enraged");
        
        if(State == "Enraged")
        {
            AngryCountdown = Math.Clamp(AngryCountdown - Time.deltaTime, 0, 100);
            if(AngryCountdown == 0)
            {
                AttackCooldown = 10;
                SetState("Nigerundayo");
                MoveUpdate();
            }
        }

        WalkingSFX.volume = agent.velocity.magnitude/10 - 0.2f;

        if(animator != null)
        {
            animator.SetFloat("Blend", agent.velocity.magnitude, 0.25f, Time.deltaTime);
            animator.speed = Math.Clamp(agent.velocity.magnitude/25, 0, 3);
        }
    }


    void FixedUpdate()
    {
        if(playerStats.Dead || !Active) return;

        agent.SetDestination(Target.position);

        //See Player
        if(!IgnorePlayer && AttackCooldown == 0 && State != "Nigerundayo")
        {
            if(!Tools.OcclusionCheck(Points, playerMovement.Camera, 1000000, OcclusionLayerMask))
            {
                SetState(State == "Enraged" ? "Enraged" : "Chasing");
                Target.position = playerMovement.transform.position;
            }
        }

        //Next Movement
        if(Tools.CalculatePathDistance(transform.position, agent.destination, agent) < 4)
        {
            NextMove();
            
            if(State == "Chasing" || State == "Enraged")
                if(AttackCooldown == 0 && Vector3.Distance(transform.position, playerMovement.transform.position) < 5) Attack(50);
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
        if(State == "Wandering") MoveUpdate();
        if(State == "Searching")
        {
            if(SearchDuration == 0) SetState("Wandering");
            MoveUpdate();
        }
        if(State == "Hearing")
        {
            SetState("Searching");
            CurrentNoisePriority = 0;
            MoveUpdate();
        }
        if(State == "Chasing")
        {
            if(!Tools.OcclusionCheck(Points, playerMovement.Camera, 1000000, OcclusionLayerMask)) Target.position = playerMovement.transform.position;
            else
            {
                SetState("Searching");
                MoveUpdate();
            }
        }
        if(State == "Nigerundayo")
        {
            navMeshObstacle.enabled = true;

            if(AttackCooldown == 0) SetState("Wandering");
            MoveUpdate();
        }
        if(State == "Enraged")
        {
            Target.position = playerMovement.transform.position;
            MoveUpdate();
        }
    }
    public void MoveUpdate() // Where to move based on State
    {
        if(State == "Wandering") Target.position = RandomNavmeshLocation(50, 18, 0.4f);
        if(State == "Searching") Target.position = RandomNavmeshLocation(30, 8, 0.4f);
        if(State == "Hearing") Target.position = RandomNavmeshLocation(20, 8, 0.4f);
        if(State == "Chasing")
        {
            if(!Tools.OcclusionCheck(Points, playerMovement.Camera, 1000000, OcclusionLayerMask)) Target.position = playerMovement.transform.position;
            else
            {
                SetState("Searching");
                MoveUpdate();
            }
        }
        if(State == "Nigerundayo") Target.position = RandomNavmeshLocation(300, 100, -1);
        if(State == "Enraged")
        {
            if(AttackCooldown == 0) Target.position = playerMovement.transform.position;
            else Target.position = RandomNavmeshLocation(100, 10, -1);
        }
    }
    public void SetState(string state)
    {
        State = state;
        if(state == "Wandering")   agent.speed = 35;
        if(state == "Searching")   agent.speed = 45;   SearchDuration = 5;
        if(state == "Hearing")     agent.speed = 40;
        if(state == "Chasing")     agent.speed = 50;
        if(state == "Nigerundayo") agent.speed = 100;
        if(state == "Enraged")     agent.speed = 60;
    }


    public void Attack(float Damage = 100)
    {
        AttackCooldown = State != "Enraged" ? 10f : 1;
        playerStats.TakeDamage(Damage);

        animator.CrossFade("Enemy_Attack", 0.1f);
        audioManager.PlaySound(SuccessfulAttack, 1, 1, 0.1f);

        if(State != "Enraged") SetState("Nigerundayo");
        MoveUpdate();
    }
    public void HearSound(Vector3 position, float Size = 1000, float priority = 1000)
    {
        if(State == "Chasing" || State == "Nigerundayo") return;

        if(priority >= CurrentNoisePriority)
        {
            if(Vector3.Distance(transform.position, position) > Size) return;

            SetState("Hearing");
            CurrentNoisePriority = priority;
            Target.position = position;
        }
    }
    [Button]
    public void Spook()
    {
        audioManager.PlaySound(Spooked, 0.6f, 1, 0.1f);

        if(AngryCountdown > 0)
        {
            AttackCooldown = 1;
            MoveUpdate();
            return;
        }

        if(UnityEngine.Random.Range(1,8) == 1) Enrage();
        else
        {
            AttackCooldown = 10;
            SetState("Nigerundayo");
            MoveUpdate();
        }
    }

    public void Enrage()
    {
        AngryCountdown = 12f;
        AttackCooldown = 1f;

        audioManager.PlaySound(Enraged, 1, 1, 0.1f);

        SetState("Enraged");
        MoveUpdate();
    }
}
