using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercer : Gun
{
    [Header("Unique Properties")]
    public float AltCharge = 0;
    public float AltCoolDown = 0;



    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if(playerMovement.WalkingCheck()) TargetAnimatorBlendTree = 1;
        else TargetAnimatorBlendTree = 0;
        animator.SetFloat("Blend", TargetAnimatorBlendTree, 0.1f, Time.deltaTime);

        //Alt Charge
        if(AltCoolDown > 0) AltCoolDown -= Time.deltaTime;
        if(AltCoolDown < 0 && AltCoolDown != 0)
        {
            AltCoolDown = 0;
            audioManager.PlaySound(gunManager.Piercer_Recharge, 0.3f, 1, 0, false);
        }

        if(HoldingAltShoot && AltCoolDown == 0) AltCharge += Time.deltaTime;
        else if(AltCharge != 0) AltCharge -= Time.deltaTime;
        
        if(AltCharge > 0.5f) AltCharge = 0.5f;
        if(AltCharge < 0) AltCharge = 0;

        if(HoldingAltShoot) animator.SetFloat("Charge", 1);
        else
        {
            animator.SetFloat("Charge", 0);
            audioManager.StopSound(gunManager.Piercer_Charge);
        }

        audioManager.SetValues(gunManager.Piercer_Charge, AltCharge*2, AltCharge*2, 0, true);
    }

    public override void ShootStart()
    {
        // Checks
        HoldingShoot = true;
        if (!CanShoot || AttackCooldown || MultiShotCooldown || AltCoolDown > 0 || AltCharge > 0) return;

        PierceCount = 0;
        Damage = _damage;
        Knockback = 18;

        animator.Play("Piercer_Shoot", 0, 0.1f);
        audioManager.PlayRandomSound(gunManager.Piercer_Shoot, 0.6f, 1f, 0.1f, false);
        audioManager.StopSound(gunManager.Piercer_Charge);

        CameraShake(0.75f);

        base.ShootStart();
    }

    public override void AltShootStart()
    {
        if (!CanShoot || AltCoolDown > 0 || AltCharge > 0) return;
        base.AltShootStart();

        animator.SetFloat("Charge", 1, 0.2f, Time.deltaTime);
        audioManager.PlaySound(gunManager.Piercer_Charge, 0f, 0f, 0, true);
    }

    public override void AltShootEnd()
    {
        HoldingAltShoot = false;
        // Checks
        if(!CanShoot || AttackCooldown || MultiShotCooldown) return;
        if(AltCharge != 0.5f) return;

        PierceCount = 4;
        AltCoolDown = 2.5f;
        Damage = _damage * 5;
        Knockback = 50;

        animator.Play("Piercer_Shoot", 0, 0.1f);
        audioManager.PlayRandomSound(gunManager.Piercer_PierceShoot, 0.9f, 1f, 0.1f, false);
        
        CameraShake(5);
        
        base.ShootStart();


        HoldingShoot = false;
        HoldingAltShoot = false;
    }
}
