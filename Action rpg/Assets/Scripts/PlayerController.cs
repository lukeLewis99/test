using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float movementSpeed;
    public movementState state = movementState.MOVING;
    Rigidbody2D body;
    public Vector2 direction;
    public GameObject attackPrefab;
    bool isAttacking;
    bool returnToAttacking;
    Animator anim;
    public float dashSpeed;
    public float startDashTime;
    private float dashTime;
    public GameObject dashEfect;

    private bool windup;
    private bool midAttack;
    public enum movementState {
        MOVING,
        
        STUNED,
        ATTACKING,
        DASHING
    };

	// Use this for initialization
	void Start () {
        dashTime = startDashTime;
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        switch (state) {
            case (movementState.MOVING):

                //Transition to blocking state when x is presed
                
                anim.SetBool("isMoving", true);
                //returns sprite color to normal (Temp)
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);

                //preform movement
                Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                body.velocity = movementVector * movementSpeed * Time.deltaTime;

                //calculate direction
                if (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) != new Vector2(0, 0)) {
                    direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


                    //set animation for direction
                    
                    anim.SetInteger("xDirection", Mathf.RoundToInt(direction.x));
                    anim.SetInteger("yDirection", Mathf.RoundToInt(direction.y));
                    

                }
                //transition into attacking state
                if (Input.GetKeyDown(KeyCode.Z))
                {

                    state = movementState.ATTACKING;

                    break;
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    Instantiate(dashEfect, transform);
                    dashTime = startDashTime;
                    state = movementState.DASHING;

                    break;
                }

                break;


            
            case (movementState.ATTACKING):

                body.velocity = new Vector2(0, 0);
                anim.SetBool("isMoving", false);

                if (!isAttacking) {
                    anim.SetBool("isMoving", false);
                    anim.SetTrigger("attack"); 
                    StartCoroutine("DoAttack");
                }
                break;
            case (movementState.DASHING):
                dashTime -= Time.deltaTime;
                body.velocity += direction * dashSpeed * Time.deltaTime;

                if (dashTime <= 0) {
                    state = movementState.MOVING;
                }


                break;
        }
	}

    IEnumerator DoAttack() {
        //Note: these events sould be synced with animation when i import them until then these are placeholders 
        isAttacking = true;
        windup = true;
        returnToAttacking = false;
        //pre attack wait
        yield return new WaitWhile(() => windup);
        midAttack = true;

        //actual attack frames
        GameObject hBox = Instantiate(attackPrefab, new Vector3(direction.x, direction.y, 0) * 1.5f + transform.position, transform.rotation);
        

        //supposed to make tha player attack again instantly if they hit attack during the attack animation, currently allows for holding of button must fix
        if (Input.GetKey(KeyCode.Z) && hBox.GetComponent<PlayerAttackHitbox>().hasHitEnemy)
        {
            returnToAttacking = true;
        }

        yield return new WaitWhile(() => midAttack);
        //after attack frames
        Destroy(hBox);
        

        isAttacking = false;
        if (!returnToAttacking) {
            state = movementState.MOVING;

        }
    }

    public void windupEneded() {
        windup = false;
    }

    public void attackEnded() {
        midAttack = false;
    }
}
