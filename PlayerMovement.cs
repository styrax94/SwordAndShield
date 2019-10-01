using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

    Vector3 targetPosition;
    Vector3 lookAtTarget;
    Vector3 direction;
    Quaternion playerRot;
    float rotSpeed = 5f;
    float speed = 4f;
    bool rightClick;
    bool leftClick;
    bool moving;
    Rigidbody playerRB;
    Animator anim;
    NavMeshAgent playerAgent;
    float basicAttackRange;
    bool enemyTargeted;
    bool isAttacking;
    Transform target;

    public Transform GateKeeper;

    //combine

    //Game Objects
    GameObject characterObject, abilityObject;

    //Animators
    Animator abiltyAnimator, characterAnimator;

   public bool iAmShield;
   public bool iAmSword;
   

    void Start()
    {
        rightClick = false;
        leftClick = false;
        playerRB = GetComponentInParent<Rigidbody>();
        anim = GetComponent<Animator>();
        moving = false;
        basicAttackRange = 2.5f;
        playerAgent = GetComponentInChildren<NavMeshAgent>();
        isAttacking = false;

        //combine

      
    }

    // Update is called once per frame
    void Update()
    {
        playerRB.velocity = Vector3.zero;

        if ((PlayerSwticher.instance.currentCharacter == PlayerSwticher.Character.Sword && iAmShield)||
            (PlayerSwticher.instance.currentCharacter == PlayerSwticher.Character.Shield && iAmSword)) return;

        if (Input.GetMouseButton(1) && !rightClick)
        {
            SetPosition();

            rightClick = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            rightClick = false;
        }

        if (Input.GetMouseButton(0) && !leftClick)
        {
            //  Attack();


        }

     


    }

    private void FixedUpdate()
    {
        if (moving)
        {
            Move();
        }
        if (Vector3.Distance(targetPosition, transform.position) > basicAttackRange && enemyTargeted)
        {
            moving = true;
            anim.SetBool("isMoving", true);
        }
        else if (enemyTargeted && !isAttacking)
        {
            Attack();
        }
    }

    void SetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {
            enemyTargeted = false;
            targetPosition = hit.point;
            this.transform.LookAt(targetPosition);
            lookAtTarget = new Vector3(targetPosition.x - transform.position.x, transform.position.y, targetPosition.z - transform.position.z);
            playerRot = Quaternion.LookRotation(lookAtTarget);
            transform.rotation = playerRot;

            if ((hit.collider.tag == "Enemy" || hit.collider.tag == "GoblinMale") && iAmSword)
            {
                enemyTargeted = true;

                if (Vector3.Distance(targetPosition, transform.position) <= basicAttackRange)
                {
                    if (!isAttacking)
                        Attack();
                    return;
                }
                target = hit.collider.gameObject.transform;

            }
            playerAgent.isStopped = false;
            playerAgent.SetDestination(targetPosition);

            moving = true;
            anim.SetBool("isMoving", true);

        }
    }

    void Move()
    {

        if (enemyTargeted)
        {
            //direction = new Vector3(target.position.x- transform.position.x, transform.position.y, target.position.z-transform.position.z);
            playerRot = Quaternion.LookRotation(targetPosition);


            if (Vector3.Distance(targetPosition, transform.position) <= basicAttackRange && enemyTargeted)
            {
                playerAgent.isStopped = true;
                Attack();
                return;
            }
            if (playerAgent.isStopped)
            {
                playerAgent.isStopped = false;
            }
            playerAgent.SetDestination(target.position);
        }
        else transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, rotSpeed * Time.fixedDeltaTime);



        if (playerAgent.remainingDistance <= playerAgent.stoppingDistance)
        {
            moving = false;
            anim.SetBool("isMoving", false);

        }
    }

    void Attack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);

        moving = false;
        anim.SetBool("isMoving", false);
        SwordAttack sword = GetComponentInChildren<SwordAttack>();
        sword.SetCanDealDamage(true);

    }

    void AttackFinished()
    {
        anim.SetBool("isAttacking", false);
        isAttacking = false;


    }
}
