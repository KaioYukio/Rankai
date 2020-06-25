using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Reference")]
    public Rigidbody rb;
    public Animator an;
    public CharacterAttack characterAttack;
    public TargetIndicator targetIndicator;
    public GetTargetsAround getTargetsAround;
    public Health playerHealth;
    public PlayerSound playerSound;
    public CameraControl cameraControl;
    public VFXControl vfxControl;
    public Transform target;
    public Transform bestTarget;
    public GameObject decalish;

    public float speed;
    public float speedLocked;
    private float actualSpeed;


    public float forceMuliplier;
    public float dodgeForce;
    public float forwardForce;
    public float timeToRecoverFromDodge;
    private float timerDodge;

    private Camera mainCamera;

    [Range(10f, 80f)]
    public float rotationSpeed;



    public bool isLockedOn;
    private Vector3 movementDirection;
    private Vector3 realDirection;

    private float stepTimer;
    public float stepTime;




    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        actualSpeed = speed;
        StopScreenShake();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.instance.IsPlayable())
        {
            if (PlayerStates.instance.estadoAtual != PlayerStates.instance.atacando && PlayerStates.instance.estadoAtual != PlayerStates.instance.bloqueando && PlayerStates.instance.estadoAtual != PlayerStates.instance.parry && PlayerStates.instance.estadoAtual != PlayerStates.instance.flinch && PlayerStates.instance.estadoAtual != PlayerStates.instance.desviando  && PlayerStates.instance.estadoAtual != PlayerStates.instance.morto)
            {
                Movement();
            }
            else
            {
                //StopForce();
            }

            if (isLockedOn)
            {
                RotationToTarget();
                cameraControl.LockYValue(); // Locka o Y do camera

            }
        }

    }

    private void Update()
    {
        if (GameManager.instance.IsPlayable())
        {
            Dodge();
            HandleInputs();
            CheckForTargets();
            CheckFlinchState();

        }
        else
        {
            rb.velocity = Vector3.zero;

            realDirection = Vector3.zero;
            an.SetFloat("speed", realDirection.magnitude);
            an.SetFloat("Horizontal", movementDirection.x);
            an.SetFloat("Vertical", movementDirection.z);


        }

    }

    

    void Movement()
    {
        movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        realDirection = mainCamera.transform.TransformDirection(movementDirection);
        if (realDirection.magnitude > 0.1f)
        {

            if (!isLockedOn)
            {
                Quaternion newRotation = Quaternion.LookRotation(realDirection);
                newRotation.x = 0;
                newRotation.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed * GameManager.instance.gameSpeed);

                rb.velocity = new Vector3(realDirection.normalized.x * actualSpeed * Time.deltaTime * GameManager.instance.gameSpeed, rb.velocity.y, realDirection.normalized.z * actualSpeed * Time.deltaTime * GameManager.instance.gameSpeed);

                Steps(stepTime);
            }
            else
            {
                if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                {
                    rb.velocity = new Vector3(realDirection.normalized.x * actualSpeed * Time.deltaTime * GameManager.instance.gameSpeed, rb.velocity.y, realDirection.normalized.z * actualSpeed * Time.deltaTime * GameManager.instance.gameSpeed) + transform.forward * forwardForce;
                }
                else
                {
                    rb.velocity = new Vector3(realDirection.normalized.x * actualSpeed * Time.deltaTime * GameManager.instance.gameSpeed, rb.velocity.y, realDirection.normalized.z * actualSpeed * Time.deltaTime * GameManager.instance.gameSpeed);
                }


            }



        }
        else
        {
            if (PlayerStates.instance.estadoAtual != PlayerStates.instance.desviando && PlayerStates.instance.estadoAtual != PlayerStates.instance.flinch)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
            else
            {

            }
        }
    }

    void HandleInputs()
    {
        if (Input.GetMouseButtonDown(2))
        {
            AssignBestTargetToTarget();

            ChangeLockState();

        }

        an.SetFloat("speed", realDirection.magnitude);
        an.SetFloat("Horizontal", movementDirection.x);
        an.SetFloat("Vertical", movementDirection.z);
        an.SetBool("IsTagetLocked", isLockedOn);
    }


    void Dodge()
    {
        timerDodge += Time.deltaTime;

        if (PlayerStates.instance.estadoAtual == PlayerStates.instance.livre)
        {
            Vector3 dir = mainCamera.transform.TransformDirection(movementDirection);
            if (Input.GetKeyDown(KeyCode.Space))
            {

                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.desviando);

                if (dir.magnitude >= 0.5f && !Input.GetKey(KeyCode.S))
                {

                    //rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
                    rb.AddForce((dir * dodgeForce * 2) + (transform.forward * forwardForce * 13), ForceMode.Impulse);
                    //dir.z = 0;
                    //dir.x *= 2;
                    //rb.AddForce((dir * dodgeForce * 2), ForceMode.Impulse);
                    Debug.Log("Desvio pro lado" + dir);
                }
                else
                {
                    //rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
                    rb.AddForce(-transform.forward * dodgeForce * 2, ForceMode.Impulse);

                }
                an.SetTrigger("Dodge");

                timerDodge = 0;
            }
        }

        if (timerDodge >= timeToRecoverFromDodge && PlayerStates.instance.estadoAtual == PlayerStates.instance.desviando)
        {
            PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.livre);
        }

    }

    void RotationToTarget()
    {
        if (target == null)
        {

            return;
        }

        Vector3 targetPos = target.position - transform.position;

        Quaternion newRotation = Quaternion.LookRotation(targetPos);
        newRotation.x = 0;
        newRotation.z = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }

    public void AssignBestTargetToTarget()
    {
        if (bestTarget != null)
        {
            target = bestTarget;
            targetIndicator.PlaceTargetIndicator(target.Find("Target Indicator Pos").transform);
            //target.GetComponent<Enemy>().AssignFresnelIntesity();

        }
    }



    void ChangeLockState()
    {
        if (target != null)
        {
            if (!isLockedOn)
            {
                isLockedOn = true;
                targetIndicator.PlaceTargetIndicator(target.Find("Target Indicator Pos").transform);
                actualSpeed = speedLocked;
            }
            else
            {
                target = null;
                bestTarget = null;
                isLockedOn = false;
                targetIndicator.RemoveParent();
                getTargetsAround.dist = Mathf.Infinity;
                actualSpeed = speed;
            }

        }
        else
        {
            bestTarget = null;
            isLockedOn = false;
            targetIndicator.RemoveParent();
            getTargetsAround.dist = Mathf.Infinity;
            actualSpeed = speed;
        }
    }

    void CheckForTargets()
    {
        if (bestTarget == null && isLockedOn)
        {
            ChangeLockState();
        }
    }

    void Steps(float intervalo)
    {
        stepTimer += Time.deltaTime * GameManager.instance.gameSpeed;

        if (stepTimer >= intervalo)
        {
            playerSound.PlayStep();
            stepTimer = 0;
        }
    }

    public void StopCharacter()
    {
        rb.velocity = Vector3.zero;
    }

    public void CheckFlinchState() // Provavelmente terá que mudar o modo como o state do player volta para o *moving*. Talvez usar eventos na animaçao de flinch
    {
        if (PlayerStates.instance.estadoAtual == PlayerStates.instance.flinch)
        {
            if (!an.GetCurrentAnimatorStateInfo(0).IsName("Flinch") && an.GetCurrentAnimatorStateInfo(0).length > 10)
            {
                PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.livre);

            }
            //Debug.Log(an.GetCurrentAnimatorStateInfo(0).IsName("Flinch"));
        }
    }

    public void ExitFlinch()
    {
        PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.livre);
        characterAttack.ResetAttackState();
        characterAttack.ResetAttackTriggers();
    }

    public void Flinch()
    {
        an.SetTrigger("Flinch");
        StopCharacter();
        //characterAttack.ResetAttackState();
        //characterAttack.ResetAttackTriggers();
        PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.flinch);
        //rb.AddForce(-transform.forward * 7, ForceMode.Impulse);
    }

    public void FlinchForce()
    {
        StopForce();
        rb.AddForce(-transform.forward * forceMuliplier, ForceMode.Impulse);
    }

    public void ParryStagger()
    {
        an.SetTrigger("Flinch");
        StopCharacter();
        PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.flinch);
        rb.AddForce(-transform.forward * 7, ForceMode.Impulse);
    }

    public void TakeDamage(Collider other, Enemy en, float dmg, KatanaColliderEnemy katanaColliderEnemy) // other serve para pegar o ponto onde a espada do inimigo acertou o jogador; enemy serve para causar o stagger no inimigo que atacou durante o parry do player
    {
        characterAttack.ColliderOff();
        if (playerHealth.health <= 0)
        {
            return;
        }

        if (characterAttack.CanExecuteParry()) //Checa se o player esta no tempo de executar o parry
        {
            if (en != null)
            {
                StopForce();
                en.Stagger(); //Executa o stagger no inimigo
                characterAttack.an.SetTrigger("RightParry");
                characterAttack.canExecuteParry = false;
                playerSound.PlayParry();
                vfxControl.PlayParrySpark();
                FOVChange();

            }
        }
        else
        {

            if (characterAttack.isBlocking)
            {
                if (katanaColliderEnemy.isLightAttack)
                {
                    //rb.AddForce(-transform.forward * 7, ForceMode.Impulse);

                    playerSound.PlayBlock();
                    vfxControl.PlaySpark();
                    cameraControl.StartScreenShake();
                    Invoke("StopScreenShake", 0.3f);
                }
                else
                {
                    Flinch();
                    playerSound.PlayHurt();
                    playerHealth.health -= dmg;
                    characterAttack.CancelBlock();
                    Vector3 point = other.ClosestPoint(transform.position);
                    decalish.transform.position = point;
                }

            }
            else
            {
                Flinch();
                playerSound.PlayHurt();
                if (katanaColliderEnemy.isLightAttack)
                {
                    playerHealth.health -= dmg;
                }
                else
                {
                    playerHealth.health -= dmg * 2;
                }

                Vector3 point = other.ClosestPoint(transform.position);
                decalish.transform.position = point;
            }

        }


    }

    void FOVChange()
    {
        cameraControl.ParryZoom();
    }

    
    void StopScreenShake()
    {
        cameraControl.StopScreenShake();
    }
    

    


    public void AddForce()
    {
        rb.AddForce(transform.forward * forceMuliplier, ForceMode.Impulse);
    }

    public void AddForceHeavyAttack()
    {
        rb.AddForce(transform.forward * forceMuliplier * 10, ForceMode.Impulse);
    }


    public void StopForce()
    {
        rb.velocity = Vector3.zero;
    }



}
