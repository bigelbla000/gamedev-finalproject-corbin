using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Im gonna see if I can implement jumping or not for enemies.
public class WaterEnemyMovement : MonoBehaviour
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
    [Range(10f, 50f)]
    public float evasionDistance; //Distance before the enemy goes into Searching mode if EvasionAbility is enabled.
    public float EvasionTimer; //Starts at evasionTime's value, then counts down when outside of its Chase distance (if EvasionAbility is enabled). If it gets to 0, chase ends.
    [Range(1f, 30f)]
    public float EvasionTime;
    public bool EvasionAbility; //Determines if 'isChasing' is permenantly enabled or not when isChasing gets triggered, setting to true will enable the ability to evade from enemy.
    private bool evading;
    private bool grounded; //public for the sake of debugging.
    private bool walled; //public for the sake of debugging.
    public bool watered; //public for the sake of debugging.
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    public Rigidbody2D body;
    public BoxCollider2D wallCheck;
    public LayerMask waterMask;
    public BoxCollider2D waterCheck;
    public bool patrolMode2; //Random.
    public bool canBreatheAir; //Determines whether this fish can survive out of water or not.
    public bool canDryUp; //Determines whether this fish can dry up or not.
    [Range(1f, 40f)]
    public float airBreathTime;
    public float airBreathTimer; //The actual counter that counts down. Public for sake of debugging.
    [Range(1f, 30f)]
    public float dryUpTime;
    public float dryUpTimer; //the actual counter that counts down. Public for sake of debugging.
    [Range(50f, 300f)]
    public float driedAccelPenaltySetting;
    public float driedAccelPenalty; //public for sake of debugging.
    public bool alive;
    [Range(0.1f, 40f)]
    public float speedCapLimit;
    private bool speedCapHit;
    public float debugDISTANCE; //disable before compiling game
    public int debugDirection;
    
    void Start() {
        alive = true;
        isChasing = false;
        waitingState = false;
        evading = false;
        airBreathTimer = airBreathTime;
        dryUpTimer = dryUpTime;
        driedAccelPenalty = 0;
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
    if(isChasing && alive)
        {
            if((transform.position.x + 0.2) > playerTransform.position.x && !speedCapHit)
            {
                transform.localScale = new Vector3(1, 1, 1);
                body.AddForce(new Vector2((-1 * Time.deltaTime) * (chaseSpeed - driedAccelPenalty), 0));
                debugDirection = 1;
            }
            if((transform.position.x - 0.2) < playerTransform.position.x && !speedCapHit)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                body.AddForce(new Vector2((1 * Time.deltaTime) * (chaseSpeed - driedAccelPenalty), 0));
                debugDirection = 2;
                //Debug.Log(Time.deltaTime);
            }
            // if (transform.position.y > playerTransform.position.y)
            // {
            //     transform.position += Vector3.down * verticalChaseSpeed * Time.deltaTime;
            // }
            if ((transform.position.y + 2) < playerTransform.position.y && grounded || (transform.position.y + 2) < playerTransform.position.y && watered)
            {
                //yInput = 1;
                body.linearVelocity = new Vector2(body.linearVelocity.x, 1 * verticalChaseSpeed);
            }


            
            if(Vector2.Distance(transform.position, playerTransform.position) > evasionDistance && EvasionAbility)
            {
                evading = true;
                isChasing = false;
            }
            
        }
        else if(evading && alive)
        {
            EvasionTimer -= Time.deltaTime;
            evading = (EvasionTimer > 0.1f);
        }

        else if(alive)
        {
            if(Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
            {
                isChasing = true;
                evading = false;
            }

            
            if(patrolMode2) {
                PatrolRandom();
            }
            else if(watered)
            {
                PatrolLinearWater();
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
                //Debug.Log(chosenTime);   
                if(watered)
                {
                    body.linearVelocityY = 0;
                }
            }
        }
    CheckGroundWallAndWater();
    if(!canBreatheAir) {
        AirSuffocate(); 
    }
    if(canDryUp) {
        DryUp(); 
    }
    WaterGravity();
    SpeedCapCheck();
    //DEBUG SEGMENT -- COMMENT OUT BEFORE COMPILING
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
    
    void PatrolLinearWater() //This is gonna be really messy but I don't know a better way to get vertical movement for the enemy working underwater.
    {
        if (!waitingState)
        {
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[patrolDestination].position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, patrolPoints[patrolDestination].position) < .2f)
            {
                if (patrolDestination >= (patrolPoints.Length - 1))
                {
                    patrolDestination = 0;
                    chosenTime = Random.Range(waitTimeMin, waitTimeMax);
                    if (waitingAbility)
                    {
                        waitingState = true;
                    }
                }
                else
                {
                    patrolDestination += 1;
                    if (waitingAbility)
                    {
                        waitingState = true;
                    }
                    chosenTime = Random.Range(waitTimeMin, waitTimeMax);
                }
            }
            if((transform.position.x - 0) < patrolPoints[patrolDestination].position.x)
            {
                body.AddForce(new Vector2(0, (Time.deltaTime) * 0.8f * chaseSpeed));
            }
            if ((transform.position.x - 0) > patrolPoints[patrolDestination].position.x)
            {
                body.AddForce(new Vector2(0, (Time.deltaTime) * 0.8f * chaseSpeed));
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

void CheckGroundWallAndWater() {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        walled = Physics2D.OverlapAreaAll(wallCheck.bounds.min, wallCheck.bounds.max, groundMask).Length > 0;
        watered = Physics2D.OverlapAreaAll(waterCheck.bounds.min, waterCheck.bounds.max, waterMask).Length > 0;
    }

void PatrolDirection() { //Cosmetic related. Flips the model around when facing in a different direction.
    if(transform.position.x > patrolPoints[patrolDestination].position.x)
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
    if(transform.position.x < patrolPoints[patrolDestination].position.x)
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }
}

void DebugStartupCheck() {
    if(waitingAbility && (waitTimeMin > waitTimeMax)) {
        Debug.Log("waitTimeMin is greater than WaitTimeMax! Unpredicable Behavior expected!"); }
    if (EvasionAbility && (chaseDistance > evasionDistance)) {
        Debug.Log("chaseDistance is greater than evasionDistance!"); }
    if (moveSpeed <= 0 || chaseSpeed <= 0 || verticalChaseSpeed <= 0 || chaseDistance <= 0 || dryUpTime <= 0 && canDryUp || !canBreatheAir && (airBreathTime == 0)) {
        Debug.Log("Some vital values are left at 0!"); }
    // if (chaseSpeed <= 900) {
    //     Debug.Log("chaseSpeed is below what it should be!")
    // }

}

void AirSuffocate() {
    if(!watered && (airBreathTimer > 0.1f)) {
        airBreathTimer -= Time.deltaTime;

        
    }
    if(watered && (airBreathTimer > 0.1f)) {
        airBreathTimer = airBreathTime;
        dryUpTimer = dryUpTime;
        driedAccelPenalty = 0;
    }
    if(airBreathTimer < 0.1f) {
        alive = false;
    }
}
void DryUp() {
    if(!watered && (dryUpTimer > 0.1f)) {
        dryUpTimer -= Time.deltaTime;
    }
    if(watered) {
        dryUpTimer = dryUpTime;
        driedAccelPenalty = 0;
    }
    if(dryUpTimer <= 0.1f) {
        driedAccelPenalty = driedAccelPenaltySetting;
    }
}

    void WaterGravity()
    {
        if (watered)
        {
            body.gravityScale = 0.5f;
        }
        else
        {
            body.gravityScale = 1f;
        }
            

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

    //DEBUG SEGMENT -- DISABLE IF EnemyWaterDebugText IS DISABLED.
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
