using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Im gonna see if I can implement jumping or not for enemies.
public class EnemyMovementAdvanced : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public float chaseSpeed; //This exists because of the movespeed required to be capable of chasing the people, being WAY too fast for patroling!
    public float verticalChaseSpeed; //only used during isChasing.
    private int patrolDestination;
    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;
    public bool waitingAbility; //Determines whether it'll wait at patrol points or not.
    private bool waitingState; //Used if waitingAbility is enabled.  Only set to public when Debugging.
    [Range(0f, 8f)]
    public float waitTimeMin; //Since waittime is randomized, these determine the minimun and maxinum possible amount of seconds.
    [Range(0f, 8f)]
    public float waitTimeMax; //same purpose as waitTimeMin, except for maxinum value instead.
    private float chosenTime; //public for sake of debugging
    public float evasionDistance; //Distance before the enemy goes into Searching mode if EvasionAbility is enabled.
    public float EvasionTimer; //Starts at evasionTime's value, then counts down when outside of its Chase distance (if EvasionAbility is enabled). If it gets to 0, chase ends.
    [Range (0.5f, 30f)]
    public float EvasionTime;
    public bool EvasionAbility; //Determines if 'isChasing' is permenantly enabled or not when isChasing gets triggered, setting to true will enable the ability to evade from enemy.
    private bool evading;
    private bool grounded; //public for the sake of debugging.
    private bool walled; //public for the sake of debugging.
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    public Rigidbody2D body;
    public BoxCollider2D wallCheck;
    public bool patrolMode2; //Random.
    [Range(0.1f, 40f)]
    public float speedCapLimit;
    private bool speedCapHit;
    public bool debugDirection;
    public float debugDISTANCE;
    [SerializeField] private Animator animator;

    void Start() {
        isChasing = false;
        waitingState = false;
        evading = false;
        DebugStartupCheck();
        
        //wallCollisionTimer = 3;
    }
    // Update is called once per frame
    void Update()
    {
        if(!waitingState && !isChasing && !evading) {
        PatrolDirection();
    }
        

    }
void FixedUpdate() {
    if(isChasing)
        {
            if((transform.position.x + 0.2) > playerTransform.position.x && !speedCapHit)
            {
                transform.localScale = new Vector3(1, 1, 1);
                debugDirection = false;
                body.AddForce(new Vector2((-1 * Time.deltaTime) * chaseSpeed, 0)); //I want to figure out a way to detect velocity so I can boost it or cap the speed.
            }
            if((transform.position.x - 0.2) < playerTransform.position.x && !speedCapHit)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                debugDirection = true;
                body.AddForce(new Vector2((Time.deltaTime) * chaseSpeed, 0));
            }
            // if (transform.position.y > playerTransform.position.y)
            // {
            //     transform.position += Vector3.down * verticalChaseSpeed * Time.deltaTime;
            // }
            if ((transform.position.y + 2) < playerTransform.position.y && grounded)
            {
                //yInput = 1;
                body.linearVelocity = new Vector2(body.linearVelocity.x, 1 * verticalChaseSpeed);
            }


            
            if(Vector2.Distance(transform.position, playerTransform.position) > evasionDistance && EvasionAbility)
            {
                evading = true;
                isChasing = false;
            }
            animator.SetBool("isRun", true);


        }
        else if(evading)
        {
            EvasionTimer -= Time.deltaTime;
            evading = (EvasionTimer > 0.1f);
        }

        else
        {
            if(Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
            {
                isChasing = true;
                evading = false;
            }

            
            if(patrolMode2) {
                PatrolRandom();
            }
            else {
                PatrolLinear();
            }
            EvasionTimer = EvasionTime; //not sure if this should be an if statement or if this should just run every frame.
            if(walled) {
                    body.linearVelocity = new Vector2(body.linearVelocity.x * -1, 1 * verticalChaseSpeed);
                    //Debug.Log("It Should work.");
                }
            // if (!walled && !waitingState) {
            //     wallCollisionTimer = 3;
            // }
            
            if(waitingState && waitingAbility)
            {
                chosenTime -= Time.deltaTime;
                waitingState = (chosenTime > 0.1f);
                animator.SetBool("isRun", false);
                //Debug.Log(chosenTime);   

            }
            else
            {
                animator.SetBool("isRun", true);
            }

        }
    CheckGroundAndWall();
    SpeedCapCheck();
    //DEBUG SECTION -- DISABLE IF "EnemyDebugText.cs" ISNT BEING USED
    debugDISTANCE = (Vector2.Distance(transform.position, playerTransform.position) - chaseDistance);
    
}



void PatrolLinear()
{
    if(!waitingState)
            {
               transform.position = Vector3.MoveTowards(transform.position, patrolPoints[patrolDestination].position, moveSpeed * Time.deltaTime);
               if(Vector3.Distance(transform.position, patrolPoints[patrolDestination].position) < .2f)
               {                   
                   if(patrolDestination >= (patrolPoints.Length - 1))
                   {
                    patrolDestination = 0;
                    chosenTime = Random.Range(waitTimeMin, waitTimeMax);
                    if(waitingAbility) {
                        waitingState = true;
                    }
                   }
                   else {
                    patrolDestination += 1;
                    if(waitingAbility) {
                        waitingState = true;
                    }
                    chosenTime = Random.Range(waitTimeMin, waitTimeMax);
                   }                 
               }

            }
}

void PatrolRandom() {
    if(!waitingState) {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[patrolDestination].position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, patrolPoints[patrolDestination].position) < .2f) {
            patrolDestination = Random.Range(0, patrolPoints.Length);
            if(waitingAbility) {
                waitingState = true;
            }
            chosenTime = Random.Range(waitTimeMin, waitTimeMax);
        }
    }
}

void CheckGroundAndWall() {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        walled = Physics2D.OverlapAreaAll(wallCheck.bounds.min, wallCheck.bounds.max, groundMask).Length > 0;
    }

void PatrolDirection() { //Cosmetic related. Flips the model around when facing in a different direction.
    if(transform.position.x > patrolPoints[patrolDestination].position.x)
    {
        transform.localScale = new Vector3(1, 1, 1);
        debugDirection = false;
    }
    if(transform.position.x < patrolPoints[patrolDestination].position.x)
    {
        transform.localScale = new Vector3(-1, 1, 1);
        debugDirection = true;
    }
}

void DebugStartupCheck() {
    if(waitingAbility && (waitTimeMin > waitTimeMax)) {
        Debug.Log("waitTimeMin is greater than WaitTimeMax! Unpredicable Behavior expected!"); }
    if (EvasionAbility && (chaseDistance > evasionDistance)) {
        Debug.Log("chaseDistance is greater than evasionDistance!"); }
    if (moveSpeed <= 0 || chaseSpeed <= 0 || verticalChaseSpeed <= 0 || chaseDistance <= 0) {
        Debug.Log("Some vital values are left at 0!"); }
    // if (chaseSpeed <= 900) {
    //     Debug.Log("chaseSpeed is below what it should be!")
    // }

}

    void SpeedCapCheck() //This sets speedCapHit to true if X velocity goes beyond a certain value. This is used by IsChasing to disable Addforce until velocity falls back down.
    {
        if (body.linearVelocityX > (speedCapLimit) || body.linearVelocityX < (speedCapLimit * -1))
        {
            speedCapHit = true;
        }
        else
        {
            speedCapHit = false;
        }
    }




//DEBUG SEGMENT -- DISABLE IF "EnemyDebugText.cs" ISNT BEING USED.
public float GetchosenTime() {
    return chosenTime;
}
public float GetPatrolDestination()
    {
        return patrolDestination;
    }
public bool GetWaitingState()
    {
        return waitingState;
    }
public bool GetEvading()
    {
        return evading;
    }
public bool GetspeedCapHit()
    {
        return speedCapHit;
    }
}
