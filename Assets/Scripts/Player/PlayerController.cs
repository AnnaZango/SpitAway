using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class PlayerController : MonoBehaviour
{
    // This script controls the player movements (walk, jump)

    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] float movementSpeed = 1000f;
    [SerializeField] float jumpForce = 1000;
    [SerializeField] float inputX;
    [SerializeField] bool isFacingRight = true;
    [SerializeField] bool isGrounded = true;
    [SerializeField] float radiusRaycastGround = 0.5f;
    [SerializeField] float distanceRaycastGround = 2f;
        
    private Vector3 localScale;

    LayerMask layerGround;

    PlayerAiming playerAiming;

    [SerializeField] AudioSource jumpSound;
    [SerializeField] Animator spritesAlpacaAnimator;

    void Start()
    {
        playerAiming = GetComponent<PlayerAiming>();
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        layerGround = (1 << LayerMask.NameToLayer("Default"))
                     | (1 << LayerMask.NameToLayer("Tiles"));
        
    }

    
    void Update()
    {
        if (GameManager.GetIfGameFinished()) { return; }

        if(!playerAiming.GetIfTurnActive() || !playerAiming.GetIfCanAct()) 
        {
            //freeze movement if player cannot act or turn is over
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            return; 
        }

        //otherwise no constraints except rotation
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isGrounded = rb.RaycastFirstHit(Vector2.down, radiusRaycastGround, distanceRaycastGround, layerGround);

        // Movement code
        inputX = CrossPlatformInputManager.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;

        if(CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded)
        {
            jumpSound.Play();
            rb.AddForce(Vector2.up * jumpForce);            
        }
        if (Mathf.Abs(rb.velocity.y) > 0.1f)
        {
            spritesAlpacaAnimator.SetBool("jump", true);
        }
        else
        {
            spritesAlpacaAnimator.SetBool("jump", false);
        }
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            spritesAlpacaAnimator.SetBool("move", true);
        }
        else
        {
            spritesAlpacaAnimator.SetBool("move", false);
        }
        
    }


    private void FixedUpdate()
    {
        rb.velocity = new Vector2(inputX, rb.velocity.y);
    }

    private void LateUpdate()
    {
        //flip sprites depending on the direction the player is going

        if (inputX > 0)
        {
            isFacingRight = true;
        } else if (inputX < 0)
        {
            isFacingRight = false;
        }

        if((isFacingRight && localScale.x<0) || !isFacingRight && localScale.x > 0)
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

}
