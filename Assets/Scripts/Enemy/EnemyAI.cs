using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // This script controls everything related to the enemy movement and priorities of movement,
    // shooting, as well as its health and turn 


    [SerializeField] bool hasTurn; //to know if enemy's turn
    [SerializeField] bool hasShot; //to know if enemy has shot
    [SerializeField] Transform projectileTarget; //set to player
    [SerializeField] ShootingEnemy shootingEnemy; //script controlling shooting
    [SerializeField] ShootingLocation shootingLocation; //position from which enemy shoots (random)

    [SerializeField] int maxHealth = 3;
    [SerializeField] int currentHealth = 3;
    [SerializeField] GameObject[] hearts;

    [Header("Movement")] //targets enemy has depending on conditions
    [SerializeField] GameObject currentTarget;
    [SerializeField] GameObject heartLocation;
    [SerializeField] GameObject currentShootingLocation;
    [SerializeField] GameObject newProjectileLocation;
    [SerializeField] float speed = 10;
    [SerializeField] float jumpHeight = 10;
    [SerializeField] bool wallAhead = false;
    [SerializeField] bool wallAhead45 = false;
    [SerializeField] bool isGrounded = true;

    //list of positions the enemy aims to reach within its turn
    [SerializeField] List<GameObject> listTargetPositions = new List<GameObject>();
    [SerializeField] int currentIndexList = 0;

    Rigidbody2D rb;
    LayerMask layermaskMove; //layers to detect for movement (and pickups)

    [SerializeField] int secondsTurn = 0;
    [SerializeField] int maxTimeTurn = 15; //less than player to make game faster

    bool isWaiting = false;

    //sounds
    [SerializeField] AudioSource hurtSound;
    [SerializeField] AudioSource jumpSound;

    [SerializeField] Animator spritesAnimator; //sprites are in a child object
    SpriteRenderer[] sprites;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();

        layermaskMove = (1 << LayerMask.NameToLayer("Default"))
                     | (1 << LayerMask.NameToLayer("Tiles"));
    }

    void Start()
    {        
        shootingLocation.SetALocation(); //we set a random location for shooting
        currentHealth = maxHealth;
        UpdateHearts();
    }

    //Terrain is simpler in enemy's area because otherwise it may get stuck 

    private void FixedUpdate()
    {
        if (GameManager.GetIfGameFinished()) { return; }

        if (!hasTurn || hasShot) { return; }

        //Code controlling enemy movement

        isGrounded = rb.RaycastFirstHit(Vector2.down, 0.3f, 1, layermaskMove); //from Extensions.cs

        //check if wall ahead to jump or not (at different degrees to detect different tile positions)
        //wall at 45 degrees        
        wallAhead45 = rb.RaycastRayFirst(new Vector2(1.5f * (-transform.localScale.x), 1f), 2, layermaskMove);
        //wall with low degrees to jump one tile hights
        wallAhead = rb.RaycastRayFirst(new Vector2(1.5f * (-transform.localScale.x), 0.1f), 2, layermaskMove);
                
        if (wallAhead || wallAhead45) 
        {
            if (isGrounded)
            {
                Jump();
            }
        }
        if (!isWaiting) //if it is not waiting for pickup to fall down
        {
            Movement();
            Flip();
        }

        CheckIfCurrentTargetReached();
    }
    

    private void Flip()
    {
        if(currentTarget == null) { return; }
        if (currentTarget.transform.position.x > transform.position.x) //going to the right
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (currentTarget.transform.position.x < transform.position.x) //going to the left
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Movement()
    {
        if(currentTarget == gameObject || currentTarget == null) { return; }

        rb.position = Vector2.MoveTowards(transform.position,
            new Vector2(currentTarget.transform.position.x, transform.position.y), 
            speed * Time.deltaTime);        
        
    }

    private void Jump()
    {
        jumpSound.Play();
        rb.AddForce(new Vector2(0, jumpHeight));
        spritesAnimator.SetTrigger("jumpTrigger");
    }


    private void IncreaseTime()
    {
        secondsTurn++;
    }

    private void SetCurrentMovementTarget(GameObject target)
    {
        currentTarget = target;
    }

    private Vector2 CalculateVelocityToHit()
    {
        Vector2 direction = projectileTarget.position - transform.position; //direction to shoot
        float height = direction.y; //height difference
        direction.y = 0; //we retain horizontal diff
        float distance = direction.magnitude;
        direction.y = distance; //elevation
        distance += height; //correct for height dif
        float velocityRaw = Mathf.Sqrt(distance * Physics2D.gravity.magnitude);

        Vector2 velocity = velocityRaw * direction.normalized;
        
        //we set an offset to this velocity so enemy doesn't always hit
        return velocity + SetAccuracyOffset();
    }

    
    private Vector2 SetAccuracyOffset() 
    {
        //accuracy varies depending on enemy's health: the lower the health, the better the aim.

        Vector2 offset = new Vector2(0, 0);

        if(currentHealth == 1)
        {
            //high accuracy, low offset range
            offset.x = Random.Range(-2, 2);
            offset.y = Random.Range(-2, 2);
        } else if(currentHealth < maxHealth && currentHealth > 1)
        {
            //medium accuracy, medium offset range
            offset.x = Random.Range(-4, 4);
            offset.y = Random.Range(-4, 4);
        }
        else
        {
            //low accuracy, wide offset range
            offset.x = Random.Range(-10, 10);
            offset.y = Random.Range(-10, 10);
        }
        return offset;
    }
        

    private void SetEnemyMovementPreferences()
    {
        if (currentHealth < maxHealth) //if health is below max and there are hearts, add heart target
        {
            if(heartLocation != null)
            {
                listTargetPositions.Add(heartLocation);
            }            
        } 
        if(newProjectileLocation != null) //if there is a new projectile pickup, add projectile target
        {
            listTargetPositions.Add(newProjectileLocation);
        }

        listTargetPositions.Add(currentShootingLocation); //shooting point is always last target position

        SetCurrentMovementTarget(listTargetPositions[0]);                     
    }

    private void CheckIfCurrentTargetReached()
    {
        if (secondsTurn > maxTimeTurn - 3) //if time about to finish, shoot (3 seconds margin)
        {
            StartCoroutine(Shoot()); 
        }
        else
        {            
            if (currentTarget == null) // if it is null, it means it has been picked up
            {
                isWaiting = false;
                if(currentIndexList+1 <= listTargetPositions.Count)
                {
                    //go to next target position
                    SetCurrentMovementTarget(listTargetPositions[currentIndexList + 1]);
                    currentIndexList++;
                }                
            } else //element not picked up or target is shooting position
            {
                float distance = Vector2.Distance
                    (transform.position, 
                    new Vector2(currentTarget.transform.position.x, transform.position.y));

                if(distance < 0.2f)
                {
                    if(currentIndexList == listTargetPositions.Count - 1)
                    {
                        //it is last target, it is the shooting point:
                        StartCoroutine(Shoot());
                    }
                    else
                    {
                        isWaiting = true; //wait for pickup to fall in this position
                    }
                }
            }
        }              
    }

    void OnDrawGizmos() //for debugging purposes, useful to see enemy's range detection
    {
        if (!Application.isPlaying) return;
        // Draw a sphere at the transform's position
        Gizmos.color = new Color32(255, 0, 0, 100);
        Gizmos.DrawSphere(transform.position, 60);
    }

    private void CheckIfPickupsInRange()
    {
        List<GameObject> hits = rb.RaycastAll(Vector2.down, 60, 0, layermaskMove); //from extensions

        //we check all hits and set the first heart and projectile we find as potential target locations
        foreach (GameObject hit in hits)
        {
            if (hit.gameObject.tag == "heart")
            {
                heartLocation = hit;
            } if(hit.gameObject.tag == "pickup")
            {
                newProjectileLocation = hit;
            }
        }
    }

    IEnumerator Shoot()
    {
        spritesAnimator.SetBool("move", false);
        hasShot = true;        
        transform.localScale = new Vector3(1, 1, 1); //look to the left to shoot

        CancelInvoke(nameof(IncreaseTime));

        yield return new WaitForSeconds(1.5f);

        spritesAnimator.SetTrigger("shoot");

        yield return new WaitForSeconds(0.2f); //wait a bit to shoot when appropriate in animation

        shootingEnemy.FireProjectile(CalculateVelocityToHit());
        listTargetPositions.Clear(); //remove all targets from list to set new ones next turn
    }

    

    public void SetTurn( bool giveTurn)
    {
        hasTurn = giveTurn;
        if (giveTurn)
        {  
            StartCoroutine(EnemySequence());
        }
    }

    IEnumerator EnemySequence()
    {
        shootingLocation.SetALocation();
        currentIndexList = 0;
        secondsTurn = 0;
        yield return new WaitForSeconds(1);
        spritesAnimator.SetBool("move", true);
        CheckIfPickupsInRange(); //check what's around
        SetEnemyMovementPreferences(); //set enemy preferences
        InvokeRepeating(nameof(IncreaseTime), 0, 1);

        hasShot = false;
    }


    public void ChangeHealthPoints(int pointsToAdd)
    {
        currentHealth += pointsToAdd;
       
        if (pointsToAdd < 0)
        {
            hurtSound.Play();
            if (currentHealth <= 0)
            {
                //die
                spritesAnimator.SetTrigger("die");
                GameManager.SetGameFinished(true);
                GameManager.SetPlayerWins(true);
                FindObjectOfType<EndGameController>().ShowPanelEnd();
            }
            else
            {
                Hurt();
            }                     
        }
        
        SetHealthWithinRange();
        UpdateHearts();
    }

    private void Hurt()
    {
        //Sprites red when hurt
        foreach (SpriteRenderer spriteRenderer in sprites)
        {
            spriteRenderer.color = new Color32(255, 130, 130, 255);
        }
        Invoke(nameof(ResetSpritesColor), 1f);
    }

    private void ResetSpritesColor()
    {
        foreach (SpriteRenderer spriteRenderer in sprites)
        {
            spriteRenderer.color = new Color32(255, 255, 255, 255);
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private void SetHealthWithinRange()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i > currentHealth - 1)
            {
                hearts[i].SetActive(false);
            }
            else
            {
                hearts[i].SetActive(true);
            }
        }
    }
}
