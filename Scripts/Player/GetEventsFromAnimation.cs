using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetEventsFromAnimation : MonoBehaviour
{
    public Animator an;
    public Health health;
    public CharacterAttack characterAttack;
    public CharacterMovement characterMovement;
    public Breath breath;

    public Animator voceMorreu;

    // Start is called before the first frame update
    void Start()
    {
        an = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        an.SetFloat("GameSpeed", GameManager.instance.gameSpeed);
    }

    public void CanAttack()
    {
        characterAttack.canAttack = true;
    }

    public void CanParry()
    {
        characterAttack.CanParry();
    }

    public void CantParry()
    {
        characterAttack.CantParry();
    }

    public void StopForce()
    {
        characterMovement.StopForce();
    }

    public void CanBlock()
    {
        characterAttack.CanBlock();
    }

    public void ResetAttackState()
    {
        characterAttack.ResetAttackState();
    }

    public void ResetAttackTriggers()
    {
        characterAttack.ResetAttackTriggers();
    }

    public void FlinchForce()
    {
        characterMovement.FlinchForce();
    }

    public void AddForce()
    {
        characterMovement.AddForce();
    }

    public void AddForceHeavyAttack()
    {
        characterMovement.AddForceHeavyAttack();
    }

    public void StartBreathing(float duration_)
    {
        //breath.StartBreathing(duration_);
    }

    public void ResetGlow()
    {
        breath.ResetGlow();
    }

    public void PlaySound()
    {
        characterAttack.PlaySound();
    }

    public void DoGap()
    {
        characterAttack.DoGap();
    }

    public void DontGap()
    {
        characterAttack.DontGap();
    }

    public void VoceMorreu()
    {
        voceMorreu.SetTrigger("GO");
    }

    public void ExitFlinch()
    {
        characterMovement.ExitFlinch();
    }
    public void DoParry()
    {
        characterAttack.DoParry();
    }

    public void RestartGame()
    {
        health.RestartGame();
    }

    void ColliderOn()
    {
        characterAttack.ColliderOn();
    }

    void ColliderOff()
    {
        characterAttack.ColliderOff();
    }
    public void ReleaseHeavyCam()
    {
        characterAttack.ReleaseHeavyCam();
    }
}
