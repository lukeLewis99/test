using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordGruntController : MonoBehaviour {

    GameObject target;
    public AiState state;
    public float movementSpeed;
    Animator anim;
    Rigidbody2D body;
    Vector2 direction;
    public GameObject attackPrefab;
    bool isAttacking;
    public float agroRange;
    GameObject AiController;
    
    private bool windup;
    private bool midAttack;

    [SerializeField]
    int checkpointOn = 0;

    Vector3 movementVector;

    [SerializeField]
    Vector3 checkpointTarget;

    public enum AiState {
        MOVINGTOOBJECTIVE,
        MOVINGTOTARGET,
        ATTACKING,
        STUNED
    }

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        AiController = GameObject.FindGameObjectWithTag("AiMaster");
        anim.speed = 0.5f;
	}

    private void OnDrawGizmos()
    {
        //draws agro circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, agroRange);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            //for when the enemy is moving toards map objectives
            //only triggers when enemy has not locked on to a target
            case (AiState.MOVINGTOOBJECTIVE):
                //loops through all player friendly units in game
                foreach (GameObject PUnit in AiController.GetComponent<AiMasterController>().frendlyUnits)
                {
                    //triggers when a frendly unit is found in range
                    if (Vector3.Distance(transform.position, PUnit.transform.position) < agroRange)
                    {
                        //sets target and changes state
                        target = PUnit;
                        state = AiState.MOVINGTOTARGET;
                        break;
                    }
                }
                checkpointTarget = AiController.GetComponent<AiMasterController>().enemyUnitPath[checkpointOn].position;
                if (Vector2.Distance(transform.position, checkpointTarget) > 1)
                {
                    
                    //preform movement
                    movementVector = transform.position - checkpointTarget;
                    body.velocity = movementVector * movementSpeed * Time.deltaTime;

                    //calculate direction

                    if (movementVector.x > 0)
                    {
                        direction.x = 1;
                    }
                    else if (movementVector.x == 0)
                    {
                        direction.x = 0;
                    }
                    else
                    {
                        direction.x = -1;
                    }

                    if (movementVector.y > 0)
                    {
                        direction.y = 1;
                    }
                    else if (movementVector.y == 0)
                    {

                        direction.y = 0;
                    }
                    else
                    {
                        direction.y = -1;
                    }

                }
                else {
                    checkpointOn += 1;
                }

                break;
            
            //triggers when unit has seen a target in range
            case (AiState.MOVINGTOTARGET):

                //sets animator to update walk animation and direction
                anim.SetBool("isMoving", true);

                //preform movement
                movementVector = target.transform.position - transform.position;
                body.velocity = movementVector * movementSpeed * Time.deltaTime;

                //calculate direction

                if (movementVector.x > 0)
                {
                    direction.x = 1;
                }
                else if (movementVector.x == 0)
                {
                    direction.x = 0;
                }
                else {
                    direction.x = -1;
                }

                if (movementVector.y > 0)
                {
                    direction.y = 1;
                }
                else if (movementVector.y == 0) { 
                
                    direction.y = 0;
                }
                else
                {
                    direction.y = -1;
                }


                //set animation for direction

                anim.SetInteger("xDirection", Mathf.RoundToInt(direction.x));
                anim.SetInteger("yDirection", Mathf.RoundToInt(direction.y));

                //start attacking if target is in range
                if (Vector3.Distance(transform.position, target.transform.position) < 2) 
                {

                    state = AiState.ATTACKING;

                    break;
                }


                break;

            case (AiState.ATTACKING):
                
                //keeps unit from moving
                
                anim.SetBool("isMoving", false);

                if (!isAttacking)
                {
                    //if attack isnt curently running set things up and start attack
                    anim.SetBool("isMoving", false);
                    anim.SetTrigger("attack");
                    StartCoroutine("DoAttack");
                }
                if (Vector3.Distance(transform.position, target.transform.position) > 2) 
                {
                    // if target has moved out of range move toards them
                    state = AiState.MOVINGTOTARGET;

                    break;
                }
                break;

        }
    }

    IEnumerator DoAttack()
    {
        Debug.Log("enemyAttacking");
        isAttacking = true;
        windup = true;
        
        //pre attack wait
        yield return new WaitWhile(() => windup);
        midAttack = true;

        Debug.Log("spawning attack Hitbox");
        //actual attack frames
        GameObject hBox = Instantiate(attackPrefab, new Vector3(direction.x, direction.y, 0) + transform.position, transform.rotation);


        

        yield return new WaitWhile(() => midAttack);
        //after attack frames
        Destroy(hBox);

        state = AiState.MOVINGTOTARGET;

        isAttacking = false;
        
    }

    public void windupEneded()
    {
        windup = false;
    }

    public void attackEnded()
    {
        midAttack = false;
    }
}
