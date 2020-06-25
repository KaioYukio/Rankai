using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{

    [Header("Reference")]
    public Animator an;

    public CharacterMovement characterMovement;

    public Breath breath;

    public PlayerSound playerSound;

    public CameraControl cameraControl;

    public VFXControl vfxControl;

    public Collider katanaCol;

    public ParticleSystem succesefulBlink;

    public GameObject gapFeedBack;

    public GameObject parryGraphics;

    public bool canAttack;
    public float minWaitTime;
    private float waitTimer;

    public float pressedTime;
    private float pressedTimer;
    private bool releasedButton;
    public bool perfomedHeavyAttack;

    [Header("Block/Parry")]
    public bool isBlocking;
    public bool canExecuteParry;
    public bool isInGapParry;
    public float maxTimeParry;
    private float timerParry;
    public bool parrying;

    public bool isInCombat;
    public bool isInGap;
    public int atkIndex;

    public bool canStartCombo;
    public bool canBlock;
    private float blockTimer;
    public float blockTime;
    private bool executedBlock;


    // Start is called before the first frame update
    void Start()
    {
        //breath = GetComponent<ParticleSystem>();

        isInGapParry = true;
        canBlock = true;
        releasedButton = true;
        canAttack = true;
        atkIndex = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsPlayable())
        {
            //gapFeedBack.SetActive(isInGap);
            HandeInput();

            if (canExecuteParry)
            {
                //breath.GlowParry();
            }
            waitTimer += Time.deltaTime * GameManager.instance.gameSpeed;
        }

    }

    void HandeInput()
    {


        //if (PlayerStates.instance.estadoAtual == PlayerStates.instance.livre || PlayerStates.instance.estadoAtual == PlayerStates.instance.bloqueando)
        //{
            
        //}

        if (isBlocking)
        {
            characterMovement.StopForce();
        }

        if (Input.GetMouseButtonDown(1))
        {
            blockTimer = 0;
            an.ResetTrigger("RightParry");
        }
        else if (Input.GetMouseButton(1) && canBlock)
        {
            if (!executedBlock)
            {
                blockTimer += Time.deltaTime;

            }

            if (blockTimer >= blockTime)
            {
                breath.ResetGlow();
                an.SetBool("IsBlocking", true);
                isInCombat = false;
                an.SetBool("IsInCombat", false);
                blockTimer = 0;
                executedBlock = true;
                isBlocking = true;

                ResetAttackTriggers();

                PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.bloqueando);
            }

        }
        else if (Input.GetMouseButtonUp(1))
        {


            if (blockTimer == 0)
            {
                ResetParryValues();
                CancelBlock();
            }
            else
            {
                if (!executedBlock && !isBlocking)
                {
                    an.SetTrigger("Parry");
                    PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.parry);
                    breath.GlowParry();
                    characterMovement.StopForce();
                    ResetAttackTriggers();
                    //ResetAttackTriggers();
                    canAttack = false;
                    blockTimer = 0;

                }
                else
                {
                    ResetParryValues();
                }
            }

          
        }



        if (Input.GetMouseButtonDown(0))
        {
            pressedTimer = 0;
            an.ResetTrigger("Parry");
            canExecuteParry = false;
            releasedButton = true;
        }
        else if (Input.GetMouseButton(0) && releasedButton)
        {
            pressedTimer += Time.deltaTime;

            characterMovement.StopForce();

            if (pressedTimer >= pressedTime)
            {
                vfxControl.PlayHeavyAttack();
                //breath.GlowHeavyAttack();
                cameraControl.HeavyAttackZoom();
                pressedTimer = 0;
                waitTimer = 0;
                releasedButton = false;
                perfomedHeavyAttack = true;
                PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.atacando);
                ResetAttackTriggers();
                an.ResetTrigger("Parry");
                an.SetTrigger("HeavyAttack");
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {

            an.ResetTrigger("HeavyAttack");

            if (waitTimer >= minWaitTime && !perfomedHeavyAttack && canAttack)
            {
                if (atkIndex == 0)
                {
                    an.SetBool("IsBlocking", false);
                    an.SetTrigger("Attack0");
                    characterMovement.StopCharacter();
                    atkIndex++;
                }
                else if (atkIndex == 1)
                {
                    if (isInGap)
                    {
                        an.SetTrigger("Attack1");
                        atkIndex++;
                        //succesefulBlink.Play();
                        //breath.AddBreathPoints();

                        isInGap = false;
                        //if (breath.IsInBlinkGap())
                        //{
                        //    //an.SetTrigger("Attack1");
                        //    //atkIndex++;
                        //    //succesefulBlink.Play();
                        //    //breath.AddBreathPoints();

                        //    //isInGap = false;
                        //}
                        //else
                        //{
                        //    an.SetTrigger("Attack1");
                        //    atkIndex++;

                        //    isInGap = false;
                        //}
                        breath.ResetBlinkGap();
                    }
                }
                else if (atkIndex == 2)
                {
                    if (isInGap)
                    {
                        an.SetTrigger("Attack2");
                        atkIndex++;
                        //succesefulBlink.Play();
                        //breath.AddBreathPoints();

                        isInGap = false;
                        //if (breath.IsInBlinkGap())
                        //{
                        //    an.SetTrigger("Attack2");
                        //    atkIndex++;
                        //    succesefulBlink.Play();
                        //    breath.AddBreathPoints();

                        //    isInGap = false;
                        //}
                        //else
                        //{
                        //    an.SetTrigger("Attack2");
                        //    atkIndex++;

                        //    isInGap = false;
                        //}
                        breath.ResetBlinkGap();
                    }
                }

                vfxControl.PlayLightAttack();
                waitTimer = 0;
                PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.atacando);
                canBlock = false;
                an.SetBool("IsInCombat", true);
            }

            perfomedHeavyAttack = false;
        }




    }

    void Parry()
    {
        if (isInGapParry && parrying)
        {
            timerParry += Time.deltaTime * GameManager.instance.gameSpeed;

            if (timerParry >= maxTimeParry)
            {
                isInGapParry = false;
                parryGraphics.SetActive(false);
                canExecuteParry = false;
                timerParry = 0;
                parrying = false;

            }
            else
            {
                parryGraphics.SetActive(true);
                canExecuteParry = true;
            }
        }

    }

    public void CanParry()
    {
        canExecuteParry = true;
        characterMovement.StopForce();
        
    }

    public void CantParry()
    {
        canExecuteParry = false;
        breath.ResetGlow();
        PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.livre);
    }

    public void CanAttack()
    {
        canAttack = true;
        waitTimer = minWaitTime + 1;
    }

    void ResetParryValues()
    {
        parryGraphics.SetActive(false);
        canExecuteParry = false;
        isInGapParry = true;
        timerParry = 0;
        parrying = false;
    }

    public void CancelBlock()
    {
        PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.livre);
        breath.ResetGlow();
        an.SetBool("IsBlocking", false);
        isBlocking = false;
        executedBlock = false;
        atkIndex = 0;


    }

    public bool CanExecuteParry()
    {
        if (canExecuteParry)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsInCombat()
    {
        if (PlayerStates.instance.state == PlayerStates.playerStates.attacking)
        {
            return true;
        }
        else
        {
            an.SetBool("Attack", false);
            return false;
        }
    }

    public void CanBlock()
    {
        canBlock = true;
    }

    public void DoGap()
    {
        isInGap = true;

    }

    public void PlaySound()
    {
        playerSound.PlayHitKatana();
    }

    public void DontGap()
    {
        isInGap = false;
        breath.ResetBlinkGap();

    }

    public void DoParry()
    {
        isBlocking = true;
        parrying = true;
    }


    public void ColliderOn()
    {
        katanaCol.enabled = true;
        ResetAttackTriggers();
    }

    public void ColliderOff()
    {
        katanaCol.enabled = false;
        ResetAttackTriggers();
    }

    public void ReleaseHeavyCam()
    {
        cameraControl.ReleaseHeavyCam();
    }


    public void ResetAttackState()
    {
        PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.livre);
        //atkIndex = 1;
        atkIndex = 0;
        canStartCombo = true;
        isInCombat = false; //ULTIMA MUDANÇA FEITA NOS ANIMATIONS EVENTS *******************************
        an.SetBool("Attack", false);
        an.SetBool("IsInCombat", false);
        an.SetBool("IsBlocking", false);
        an.ResetTrigger("Parry");
        ResetAttackTriggers();
        breath.ResetGlow();
        canExecuteParry = false;
        canAttack = true;
        isBlocking = false;
        canBlock = true;
    }

    public void ResetAttackTriggers()
    {
        an.ResetTrigger("Attack0");
        an.ResetTrigger("Attack1");
        an.ResetTrigger("Attack2");
    }
}
