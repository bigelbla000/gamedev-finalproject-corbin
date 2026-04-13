using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int maxPatrolPoints;
    public float moveSpeed;
    public float verticalSpeed; //only used during isChasing
    public int patrolDestination;
    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;
    public bool WaitingAbility;
    public bool waitingState; //Used if RandomType1 is enabled
    public float WaitTimeMin; 
    public float WaitTimeMax;
    private float chosenTime;
    public float EvasionDistance; //Distance before the enemy goes into Searching mode if EvasionAbility is enabled.
    private float EvasionTimer;
    public float EvasionTime;
    public bool EvasionAbility; //Determines if 'isChasing' is permenantly enabled or not when isChasing gets triggered, setting to true will enable the ability to evade from the enemy.
    public bool Evading; //A flag of whether 

    // Update is called once per frame

    void Update()
    {
        if(isChasing)
        {
            if(transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            }
            if(transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            }
            //if (transform.position.y > playerTransform.position.y)
            {
                transform.position += Vector3.down * verticalSpeed * Time.deltaTime;
            }
            //if (transform.position.y < playerTransform.position.y)
            {
                transform.position += Vector3.up * verticalSpeed * Time.deltaTime;
            }
            
            if(Vector2.Distance(transform.position, playerTransform.position) > EvasionDistance && EvasionAbility)
            {
                Evading = true;
                isChasing = false;
            }
            
        }
        else if(Evading)
        {
            Debug.Log("It worked");
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
void PatrolLinear()
{
    if(!waitingState)
            {
               transform.position = Vector2.MoveTowards(transform.position, patrolPoints[patrolDestination].position, moveSpeed * Time.deltaTime);
               if(Vector2.Distance(transform.position, patrolPoints[patrolDestination].position) < .2f)
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
}
