using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Reference")]
    public Animator an;

    public Rigidbody rb;
    public new Collider collider;

    public Material enemyMaterial;
    public GameObject enemyGraph;

    public Transform targetIndicatorPos;

    public Transform playerTransform;
    public Health playerHealth;

    public Collider katanaCol;

    public CharacterAttack playerCharacterAttack;

    public GameObject parryGraph;

    public HealthEnemy healthEnemy;

    public GameObject enemyGraphics;

    public EnemySound enemySound;

    public TargetIndicator targetIndicator;
    public WaveMannager waveMannager;

    private bool playerArround;

    [Header("VFX")]
    public ParticleSystem spark;
    public ParticleSystem bloodVFX;

    private Enemy enemy;
    private NavMeshAgent agent;

    [Header("Vision")]
    public Vector3 startPos;
    public Vector3 lastPlayerPos;
    public LayerMask layerMask;
    public float seePlayerDistance;
    public float attackDistance;
    public float siegeAttackDistance;
    public float timeMissingPlayer;
    public float timerMissing;

    [Header("Block/Parry")]
    public int hits;
    public int maxHit;
    public float parryTime;
    public bool isParry;

    [Header("Siege")]
    public float rotateForce;
    public float maxTimeRotateOneSide;
    private float maxTimeRotateOneSide_;
    private float timerRotate;
    public float maxSiegeDist;
    public float minSiegeDist;
    public float timeHoldingStill;
    public float timerHoldingStill;

    public enum EnemyStates { idle, idling, siege, sieging, attack, attackPesado, attacking, attackingPesado, block, walk, chase, chasing, flinching, blocking, searchPlayer, returnToStart, death };
    [Header("States")]
    public EnemyStates enemyStates;
    public float rotationSpeed;
    public float damage;
    public bool canAct;

    public float dmgBlood;

    private float checkAttackSeconds;




    // Start is called before the first frame update
    public virtual void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerCharacterAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterAttack>(); ;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        enemy = GetComponent<Enemy>();
        targetIndicator = FindObjectOfType<TargetIndicator>();

        enemyMaterial = new Material(enemy.enemyMaterial);
        enemyGraph.GetComponent<SkinnedMeshRenderer>().material = enemyMaterial;

        maxTimeRotateOneSide_ = maxTimeRotateOneSide;

        startPos = transform.position;

        enemyStates = EnemyStates.idle;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<Health>();
        }

        if (GameManager.instance.IsPlayable())
        {
            an.SetFloat("speed", agent.velocity.magnitude);
            HitsTaken();
            States();
            //Debug.Log(agent.destination);
            //AssignFresnelIntesity();

        }

    }

    void States()
    {
        switch (enemyStates)
        {
            case (EnemyStates.idle):
                {
                    UpdateEnemyMaterial();

                    an.SetBool("PlayerOnDistSiege", false);
                    enemyStates = EnemyStates.idling;
                    break;
                }
            case (EnemyStates.idling):
                {
                    if (agent.isActiveAndEnabled)
                    {
                        agent.isStopped = true;
                    }

                    if (!canAct)
                    {
                        return;
                    }
                    if (IsPlayerOnSight())
                    {
                        if (GetDistanceFromPlayer() < minSiegeDist)
                        {
                            timerHoldingStill = 0;
                            enemyStates = EnemyStates.siege;
                        }
                        else
                        {

                            enemyStates = EnemyStates.chase;
                        }
                    }
                    else if (IsPlayerArround())
                    {
                        RotationToTarget();
                    }

                    break;
                }

            case (EnemyStates.siege):
                {
                    MakeColliderOff();
                    an.SetBool("PlayerOnDistSiege", true);
                    timerHoldingStill = 0;
                    enemyStates = EnemyStates.sieging;
                    rb.isKinematic = true;
                    if (agent.isActiveAndEnabled)
                    {
                        agent.isStopped = true;
                    }


                    break;
                }

            case (EnemyStates.sieging):
                {

                    if (IsPlayerOnSight())
                    {

                        if (GetDistanceFromPlayer() > maxSiegeDist)
                        {
                            timerHoldingStill += Time.deltaTime;

                            if (timerHoldingStill >= 1.5f)
                            {
                                timerHoldingStill = 0;
                                enemyStates = EnemyStates.chase;
                            }


                        }
                        else if (GetDistanceFromPlayer() < siegeAttackDistance)
                        {
                            if (playerHealth.health > 0)
                            {
                                enemyStates = EnemyStates.attack;
                            }
                            else
                            {
                                enemyStates = EnemyStates.idle;
                            }

                        }
                        else
                        {
                            checkAttackSeconds += Time.deltaTime;

                            if (checkAttackSeconds >= 3)
                            {
                                if (playerHealth.health > 0)
                                {
                                    enemyStates = EnemyStates.attack;
                                }
                                else
                                {
                                    enemyStates = EnemyStates.idle;
                                }
                                checkAttackSeconds = 0;
                            }
                            else if (checkAttackSeconds % 2 == 0)
                            {

                                int rand = Random.Range(0, 5);

                                if (rand >= 3)
                                {
                                    if (playerHealth.health > 0)
                                    {
                                        enemyStates = EnemyStates.attack;
                                    }
                                    else
                                    {
                                        enemyStates = EnemyStates.idle;
                                    }
                                }
                            }
                        }
                        RotationToTarget();
                        RotateAroundPlayer();
                        an.SetFloat("Horizontal", rotateForce);

                    }
                    else 
                    {
                        enemyStates = EnemyStates.searchPlayer;

                    }

                    break;
                }

            case (EnemyStates.chase):
                {
                    rb.isKinematic = true;
                    agent.isStopped = false;
                    UpdateEnemyMaterial();
                    an.SetBool("PlayerOnDistSiege", false);
                    an.Play("Walk", 0, 0);
                    timerHoldingStill = 0;
                    enemyStates = EnemyStates.chasing;
                    break;
                }

            case (EnemyStates.chasing):
                {
                    if (IsPlayerOnSight())
                    {
                        if (GetDistanceFromPlayer() < minSiegeDist)
                        {
                            enemyStates = EnemyStates.siege;
                        }
                        else
                        {
                            RotationToTarget();
                            agent.SetDestination(playerTransform.position);
                        }

                    }
                    else
                    {
                        enemyStates = EnemyStates.searchPlayer;
                    }

                    break;
                }

            case (EnemyStates.flinching):
                {
                    //if (!an.GetCurrentAnimatorStateInfo(0).IsName("Standing React Large From Right") || !an.GetCurrentAnimatorStateInfo(0).IsName("Standing React Large From Left"))
                    //{
                    //    enemyStates = EnemyStates.idle;
                    //}
                    break;
                }

            case (EnemyStates.block):
                {
                    an.ResetTrigger("Attack");
                    an.SetTrigger("Blocking");

                    UpdateEnemyMaterial();
                    RotationToTarget();
                    enemyStates = EnemyStates.blocking;
                    break;
                }
            case (EnemyStates.blocking):
                {
                    //MakeColliderOff();
                    agent.velocity = Vector3.zero;

                    break;
                }

            case (EnemyStates.attack):
                {
                    if  (playerHealth.health > 0)
                    {
                        if (GetWorse())
                        {
                            int randWorse = Random.Range(0, 7);

                            if (randWorse == 0)
                            {
                                agent.velocity = Vector3.zero;
                                agent.isStopped = true;
                                MakeColliderOff();
                                RotationToTarget();

                                int rand2 = Random.Range(0, 5);
                                if (rand2 == 2)
                                {
                                    an.SetTrigger("HeavyAttack");
                                    katanaCol.GetComponent<KatanaColliderEnemy>().isLightAttack = false;
                                    enemyStates = EnemyStates.attackingPesado;
                                }
                                else
                                {
                                    an.SetTrigger("Attack");
                                    katanaCol.GetComponent<KatanaColliderEnemy>().isLightAttack = true;
                                    enemyStates = EnemyStates.attacking;
                                }
                            }
                            else
                            {
                                enemyStates = EnemyStates.idle;
                            }
                        }
                        else
                        {
                            agent.velocity = Vector3.zero;
                            agent.isStopped = true;
                            MakeColliderOff();
                            RotationToTarget();

                            int rand = Random.Range(0, 5);
                            if (rand == 2)
                            {
                                an.SetTrigger("HeavyAttack");
                                katanaCol.GetComponent<KatanaColliderEnemy>().isLightAttack = false;
                                enemyStates = EnemyStates.attackingPesado;
                            }
                            else
                            {
                                an.SetTrigger("Attack");
                                katanaCol.GetComponent<KatanaColliderEnemy>().isLightAttack = true;
                                enemyStates = EnemyStates.attacking;
                            }
                        }
                    }
                    else
                    {
                        enemyStates = EnemyStates.idle;
                    }
                    
                    break;
                }

            case (EnemyStates.attackingPesado):
                {
                    UpdateEnemyMaterial();
                    //agent.velocity = Vector3.zero;
                    //agent.isStopped = true;
                    //enemyStates = EnemyStates.attacking;
                    //an.SetTrigger("Attack");

                    break;
                }

            case (EnemyStates.attacking):
                {
                    //agent.velocity = Vector3.zero;
                    //RotationToTarget();
                    //RotationToTarget();

                    break;
                }

            case (EnemyStates.searchPlayer):
                {

                    timerMissing += Time.deltaTime;

                    if (timerMissing >= timeMissingPlayer)
                    {
                        timerMissing = 0;
                        enemyStates = EnemyStates.returnToStart;

                    }
                    else
                    {                     
                        if (IsPlayerOnSight())
                        {
                            timerMissing = 0;
                            enemyStates = EnemyStates.chase;
                        }
                        else
                        {
                            agent.SetDestination(lastPlayerPos);
                        }
                    }

                    break;
                }

            case (EnemyStates.returnToStart):
                {
                    if (IsPlayerOnSight())
                    {
                        enemyStates = EnemyStates.chase;
                    }
                    else if (transform.position == startPos)
                    {
                        enemyStates = EnemyStates.idle;
                    }

                    agent.SetDestination(startPos);

                    break;
                }

            case (EnemyStates.death):
                {
                    agent.velocity = Vector3.zero;
                    break;
                }
        }

    }

    bool GetWorse()
    {
        if (targetIndicator.PlayerIsLocked())
        {
            if (targetIndicatorPos.childCount >= 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public void ActionBasedOnHealth()
    {
        //Debug.Log("Vida do inimigo: " + healthEnemy.health);
        if (healthEnemy.health <= 100 && healthEnemy.health > 80)
        {
            int rand = Random.Range(0, 100);

            if (rand >= 95)
            {
                enemyStates = EnemyStates.block;
            }
            else if (rand < 95 && rand > 15)
            {
                enemyStates = EnemyStates.attack;
            }
            else
            {
                enemyStates = EnemyStates.attackPesado;
            }
            //Debug.Log("Estado foi para: " + enemyStates);
        }
        else if (healthEnemy.health <= 80 && healthEnemy.health > 30)
        {
            int rand = Random.Range(0, 100);

            if (rand >= 70)
            {
                enemyStates = EnemyStates.block;
            }
            else if (rand < 70 && rand > 30)
            {
                enemyStates = EnemyStates.attack;
            }
            else
            {
                enemyStates = EnemyStates.attackPesado;
            }
            Debug.Log("Estado foi para: " + enemyStates);
        }
        else
        {
            int rand = Random.Range(0, 100);

            if (rand >= 60)
            {
                enemyStates = EnemyStates.block;
            }
            else if (rand < 60 && rand > 50)
            {
                enemyStates = EnemyStates.attack;
            }
            else
            {
                enemyStates = EnemyStates.attackPesado;
            }
            Debug.Log("Estado foi para: " + enemyStates);
        }
    }

    void RotateAroundPlayer()
    {
        timerRotate += Time.deltaTime;

        if (timerRotate >= maxTimeRotateOneSide_)
        {
            float rand = Random.Range(1, maxTimeRotateOneSide);
            maxTimeRotateOneSide_ = rand;
            timerRotate = 0;
            rotateForce *= -1;

        }

        transform.RotateAround(playerTransform.position, Vector3.up, rotateForce * Time.deltaTime);

        //rb.velocity = (transform.right * rotateForce);

        Debug.Log("FORCE!");
    }

    public bool IsPlayerVisibleToEnemy()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        Vector3 targetDir = playerTransform.position - transform.position;

        float angle = Vector3.Angle(targetDir, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, targetDir, out hit, 50, layerMask))
        {
            Debug.DrawRay(transform.position, targetDir, Color.red);

            if (hit.transform.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }

    bool IsPlayerOnSight()
    {
        if (playerTransform == null)
        {
            return false;
        }


        Vector3 targetDir = playerTransform.position - transform.position;

   
        float angle = Vector3.Angle(targetDir, transform.forward);

        if (angle < 70)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, targetDir, out hit, seePlayerDistance, layerMask))
            {
                if (hit.transform.tag == "Player")
                {
                    //Debug.Log("Dist to Player: " + hit.distance);
                    //Debug.DrawRay(transform.position, targetDir, Color.red);
                    //Debug.Log("Player on Sight" + angle);
                    //Debug.Log(hit.transform.name);
                    lastPlayerPos = playerTransform.position;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }

    bool IsPlayerOnRangeToAttack()
    {
        Vector3 targetDir = playerTransform.position - transform.position;

        float angle = Vector3.Angle(targetDir, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, targetDir, out hit, 7, layerMask))
        {
            Debug.DrawRay(transform.position, targetDir, Color.red);


            if (hit.distance < attackDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool IsPlayerArround()
    {
        if (playerTransform == null)
        {
            return false;
        }
        Vector3 targetDir = playerTransform.position - transform.position;

        float angle = Vector3.Angle(targetDir, transform.forward);

        if (angle < 360)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, targetDir, out hit, seePlayerDistance, layerMask))
            {
                if (hit.transform.tag == "Player")
                {
                    //Debug.Log("Dist to Player: " + hit.distance);
                    //Debug.DrawRay(transform.position, targetDir, Color.red);
                    //Debug.Log("Player on Sight" + angle);
                    //Debug.Log(hit.transform.name);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    float GetDistanceFromPlayer()
    {
        Vector3 targetDir = playerTransform.position - transform.position;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, targetDir, out hit, 7, layerMask))
        {
            Debug.DrawRay(transform.position, targetDir, Color.red);

            return hit.distance;

        }
        else
        {
            return Mathf.Infinity;
        }

    }

    public void RotationToTarget()
    {
        if (playerTransform == null || enemy == null)
        {
            return;
        }

        Vector3 targetPos = playerTransform.position - transform.position;

        Quaternion newRotation = Quaternion.LookRotation(targetPos);
        newRotation.x = 0;
        newRotation.z = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }

    void HitsTaken()
    {
        if (hits >= maxHit)
        {
            if (GetDistanceFromPlayer() < attackDistance && GetDistanceFromPlayer() < seePlayerDistance) //Checa se o player esta no range do ataque
            {
                //enemyStates = EnemyStates.block;


                //hits = 0;
            }


        }
    }

    IEnumerator Parry()
    {
        float time = 0;
        isParry = true;
        parryGraph.SetActive(true);
        while (time <= parryTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        hits = 0;
        parryGraph.SetActive(false);
        isParry = false;

    }

    public void TakeDamage(float mult)
    {
        if (enemyStates == EnemyStates.sieging)
        {
            hits++;

            if (hits >= maxHit)
            {
                int rand = Random.Range(0, 5);

                    StartCoroutine(Parry());
                if (rand < 2)
                {
                }
                //else if (rand < 2)
                //{
                //    an.SetTrigger("Hit");
                //    playerTransform.GetComponent<CharacterMovement>().ParryStagger();
                //    enemyStates = EnemyStates.flinching;
                //    enemySound.PlayBlock();
                //    spark.Play();
                //    rb.isKinematic = false;
                //    rb.AddForce(-transform.forward * 1.6f, ForceMode.Impulse);

                //}
                hits = 0;
            }

            if (isParry)
            {
                playerTransform.GetComponent<CharacterMovement>().ParryStagger();
                //ExitBlocking();
                an.SetTrigger("Parry");
                enemySound.PlayParry();
                spark.Play();

                enemyStates = EnemyStates.blocking;
            }
            else
            {
                //an.ResetTrigger("Hit");
                enemySound.PlayBlock();
                spark.Play();


                rb.isKinematic = false;
                rb.AddForce(-transform.forward * 1.6f, ForceMode.Impulse);
            }

            checkAttackSeconds = 0;

        }
        else
        {
            if (enemyStates == EnemyStates.attackingPesado)
            {
                enemySound.PlayHurt();
            }
            else if (enemyStates == EnemyStates.blocking)
            {

            }
            else
            {
                Flinch(mult);
                enemySound.PlayHurt();
            }
        }

        MakeColliderOff();
        rb.isKinematic = true;
    }

    void Flinch(float mult)
    {



        healthEnemy.health -= healthEnemy.damageTake * mult;
        IncreaseBlood();

        if (healthEnemy.health <= 0)
        {
            if (enemyStates != EnemyStates.death)
            {
                FindObjectOfType<Health>().Heal();
                an.SetTrigger("Death");
                rb.isKinematic = true;



                if (targetIndicatorPos.childCount >= 1)
                {
                    targetIndicatorPos.GetChild(0).transform.SetParent(null);
                }


                //if (waveMannager == null)
                //{
                //    waveMannager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveMannager>();
                //}

                if (waveMannager != null && waveMannager.isActiveAndEnabled)
                {
                    waveMannager.SubtractAcive();
                }


                Destroy(collider);
                agent.enabled = false;


                enemyStates = EnemyStates.death;


            }
        }
        else
        {

            an.SetTrigger("Hit");
            enemyStates = EnemyStates.flinching;
        }

        int rand = Random.Range(0, 2);

        if (rand == 1)
        {
            bloodVFX.transform.localEulerAngles = new Vector3(0, 86.258f, 0);
        }
        else
        {
            bloodVFX.transform.localEulerAngles = new Vector3(180, 86.258f, 0);
        }
        bloodVFX.Play();

        //GameManager.instance.DoHitStop();



    }

    public void Morte()
    {
        enemyGraphics.transform.SetParent(null);
        enemyMaterial.SetFloat("fresnelIntensity", 0);
        Destroy(gameObject);
        //Destroy(enemy);
    }

    public void Stagger() //Alterar para o stagger, criar estado de stagger forma de saida do estado
    {

        an.SetTrigger("Hit");
        enemyMaterial.SetFloat("_fresnelIntensity", 0);
        enemyStates = EnemyStates.flinching;
    }

    public void UpdateEnemyMaterial()
    {

        if (enemyStates == EnemyStates.attackingPesado)
        {
            //enemyMaterial.SetColor("fresnelColor", Color.red);
            enemyMaterial.SetFloat("_fresnelIntensity", 6);

            
        }
        else
        {
            //enemyMaterial.SetColor("fresnelColor", Color.magenta);
            enemyMaterial.SetFloat("_fresnelIntensity", 0);
        }

    }

    public void IncreaseBlood()
    {
        dmgBlood += 0.15f;
        dmgBlood = Mathf.Clamp(dmgBlood,0, 1);
        enemyMaterial.SetFloat("_blood_amount", dmgBlood);
    }

    public void AssignFresnelIntesity()
    {
        if (targetIndicatorPos.childCount >= 1)
        {


        }
        else
        {
            enemyMaterial.SetFloat("_fresnelIntensity", 0f);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 targetDir = playerTransform.position - transform.position;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, targetDir, out hit, 7))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    playerArround = true;
                }

            }
            else
            {
                playerArround = false;
            }

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerArround = false;
        }
    }

    public void ExitBlocking()
    {
        an.SetBool("Blocking", false);
        an.ResetTrigger("Hit");
        an.ResetTrigger("Hit2");
        rb.isKinematic = true;

        enemyStates = EnemyStates.idle;
    }

    public void DoParry()
    {
        //StartCoroutine(Parry());
    }

    public void MakeColliderOn()
    {
        katanaCol.enabled = true;
    }

    public void MakeColliderOff()
    {
        katanaCol.enabled = false;
    }

    public void Attack()
    {
        if (IsPlayerOnRangeToAttack())
        {
            an.SetTrigger("Attack");
        }
    }

    public void ChanceToHeavyAttack()
    {
        if (IsPlayerOnRangeToAttack())
        {
            int rand = Random.Range(0, 4);

            if (rand == 0)
            {
                an.SetTrigger("HeavyAttack");
            }

        }
    }

    public void ReturnToIdle()
    {
        RotationToTarget();
        enemyStates = EnemyStates.idle;
    }

    public void AddForceForward()
    {
        rb.isKinematic = false;
        rb.AddForce(transform.forward * 50, ForceMode.Impulse);
    }

    public void AddForceFlinch()
    {
        if(enemy == null)
        {
            return;
        }

        rb.isKinematic = false;
        rb.AddForce(-transform.forward * 10, ForceMode.Impulse);
    }

}
