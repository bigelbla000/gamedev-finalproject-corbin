using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Im gonna see if I can implement jumping or not for enemies.
public class EnemyMovementExperimental : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int maxPatrolPoints;
    public float moveSpeed;
    public float chaseSpeed; //This exists because of the movespeed required to be capable of chasing the people, being WAY too fast for patroling!
    public float verticalChaseSpeed; //only used during isChasing.
    public int patrolDestination;
    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;
    public bool WaitingAbility; //Determines whether it'll wait at patrol points or not.
    private bool waitingState; //Used if WaitingAbility is enabled.  Only set to public when Debugging.
    public float WaitTimeMin; //Since waittime is randomized, these determine the minimun and maxinum possible amount of seconds.
    public float WaitTimeMax; //same purpose as WaitTimeMin, except for maxinum value instead.
    private float chosenTime;
    public float EvasionDistance; //Distance before the enemy goes into Searching mode if EvasionAbility is enabled.
    private float EvasionTimer;
    public float EvasionTime;
    public bool EvasionAbility; //Determines if 'isChasing' is permenantly enabled or not when isChasing gets triggered, setting to true will enable the ability to evade from enemy.
    public bool Evading;
    public bool grounded; //public for the sake of debugging.
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    public Rigidbody2D body;
    [Range(0f, 1f)]
    public float chaseSpeedMomentumSetting;
    public float chaseSpeedMomentum;
    
    //public float yInput; //this feels like a bad idea to work around the cannot convert vector2 to float error, but I dont know any other way to get the "AI" to jump like the player.
    // Update is called once per frame

    void Update()
    {
        if(isChasing)
        {
            if((transform.position.x + 0.2) > playerTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                //chaseSpeedMomentum = chaseSpeedMomentumSetting; Wouldnt work because its executing every frame
                //MomentumBuildUp();s
                body.AddForce(new Vector2((-1 * Time.deltaTime) * chaseSpeed, 0));
            }
            if((transform.position.x - 0.2) < playerTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
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


            
            if(Vector2.Distance(transform.position, playerTransform.position) > EvasionDistance && EvasionAbility)
            {
                Evading = true;
                isChasing = false;
                EvasionTimer = EvasionTime;
            }
            
        }
        else if(Evading)
        {
            EvasionTime -= Time.deltaTime;
            Evading = (EvasionTime > 0.1f);
        }

        else
        {
            if(Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
            {
                isChasing = true;
                Evading = false;
            }

            
            PatrolLinear();
            //if(patrolDestination == 1 && !waitingState)
            //{
            //   transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
            //   if(Vector2.Distance(transform.position, patrolPoints[1].position) < .2f)
            //   {
            //       transform.localScale = new Vector3(-1, 1, 1);
            //       patrolDestination = 0;
            //       waitingState = true;
            //       chosenTime = Random.Range(WaitTimeMin, WaitTimeMax);
            //   }
            //}

            if(waitingState && WaitingAbility)
            {
                chosenTime -= Time.deltaTime;
                waitingState = (chosenTime > 0.1f);
                Debug.Log(chosenTime);   

            }
        }

    }
void FixedUpdate() {
    CheckGround();
}
void PatrolLinear()
{
    if(!waitingState)
            {
               transform.position = Vector3.MoveTowards(transform.position, patrolPoints[patrolDestination].position, moveSpeed * Time.deltaTime);
               if(Vector3.Distance(transform.position, patrolPoints[patrolDestination].position) < .2f)
               {
                   //transform.localScale = new Vector3(1, 1, 1);
                   
                   if(patrolDestination >= maxPatrolPoints)
                   {
                    patrolDestination = 0;
                    chosenTime = Random.Range(WaitTimeMin, WaitTimeMax);
                    if(WaitingAbility)
                    {
                        waitingState = true;
                    }
                   }
                   else
                   {
                    patrolDestination += 1;
                    if(WaitingAbility)
                    {
                        waitingState = true;
                    }
                    chosenTime = Random.Range(WaitTimeMin, WaitTimeMax);
                   }
                   
               }
            }
}

void CheckGround() {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }
// void PatrolRandom()
// {
//     if(!waitingState)
//     {
//         transform.position = Vector2.MoveTowards(transform.position, patrolPoints[patrolDestination].position, moveSpeed * Time.deltaTime);
//         if(Vector2.Distance(transform.position, patrolPoints[patrolDestination].position) < .2f)
//                {
//                    //transform.localScale = new Vector3(1, 1, 1);
                   
//                    if(patrolDestination >= maxPatrolPoints)
//                    {
//                     patrolDestination = 0;
//                     chosenTime = Random.Range(WaitTimeMin, WaitTimeMax);
//                     if(WaitingAbility)
//                     {
//                         waitingState = true;
//                     }

                   
//                }
//     }
// }
//private
//void MomentumBuildUp() {
//    if(chaseSpeedMomentum < 1)
//    chaseSpeedMomentum += chaseSpeedMomentum;
//}

}
