using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //component references
    private Rigidbody2D rb;
    private CapsuleCollider2D myCol;
    private float moveInput;
    //speed for the movement of the player
    public float speed = 10;
    //struck to keep the status of the player
    public PlayerStatus playerPublicStatus;
    
    /**/
    private Animator anim;
    
    //start method to set a valid reference to this gameObject components
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCol = GetComponent<CapsuleCollider2D>();
        /**/
        anim = GetComponent<Animator>();
    }

    //fixed update to deal with all the physics and collisions of the game
    void FixedUpdate()
    {
        //setting the player movement
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        
        //setting the status according to the player movement
        playerPublicStatus.moving = false;

        if (playerPublicStatus.onLadder)
        {
            moveInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(0, moveInput * speed);
        }
        
        /**/
        if (rb.velocity.normalized.x != 0)
        {
            playerPublicStatus.moving = true;
        }

        if (!playerPublicStatus.overLadderObject)
        {
            rb.gravityScale = 10;
            myCol.isTrigger = false;
            playerPublicStatus.onLadder = false;
        }
        
        SetAnimations(rb.velocity.normalized);
        
    }

    //normal update to deal with the inputs
    private void Update()
    {
        //input to use the ladder
        if (Input.GetKeyDown(KeyCode.E) && playerPublicStatus.overLadderObject)
        {

            if (playerPublicStatus.onLadder)
            {
                rb.gravityScale = 10;
                myCol.isTrigger = false;
            }
            else
            {
                rb.gravityScale = 0;
                myCol.isTrigger = true;
            }
            
            playerPublicStatus.onLadder = !playerPublicStatus.onLadder;
        }

    }
    
    /**/
    void SetAnimations(Vector2 characterVelocity)
    {
        anim.SetFloat("velocityX", characterVelocity.x);
        if(characterVelocity.x != 0)
            anim.SetFloat("lastVelocityX", characterVelocity.x);
        
        anim.SetFloat("velocityY", characterVelocity.y);
        
        anim.SetBool("moving", playerPublicStatus.moving);
        anim.SetBool("onLadder", playerPublicStatus.onLadder);
    }
    

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            playerPublicStatus.overLadderObject = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            playerPublicStatus.overLadderObject = false;
        }
    }


    public struct PlayerStatus
    {
        public bool onLadder; //says if the player is using the ladder
        public bool overLadderObject; //says if the player is over the ladder object
        /**/
        public bool moving;
    }
}
