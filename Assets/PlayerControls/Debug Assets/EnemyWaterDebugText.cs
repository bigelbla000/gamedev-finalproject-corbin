using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyWaterDebugText : MonoBehaviour
{   
    public TextMeshProUGUI isChasing;
    public TextMeshPro chosenTimeText;
    public float chosenTime;
    public WaterEnemyMovement WaterEnemyMovement;
    public EnemyDamage EnemyDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        if (WaterEnemyMovement.body.transform.localScale == new Vector3(-1, 1, 1))
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        chosenTime = WaterEnemyMovement.GetchosenTime();
        string dispTxt = "WaterEnemy" + "     HP:" + EnemyDamage.health +
            "\nChosenTime: " + chosenTime.ToString("F2") + "    waitingState:" + WaterEnemyMovement.GetWaitingState() +
            "\nEvasionTimer: " + WaterEnemyMovement.EvasionTimer.ToString("F2") + "  evading: " + WaterEnemyMovement.GetEvading() +
            "\nDist Until Chase: " + WaterEnemyMovement.debugDISTANCE.ToString("F2") + "  isChasing: " + WaterEnemyMovement.isChasing +
            "\nX Velocity: " + WaterEnemyMovement.body.linearVelocityX.ToString("F2") + "  speedcaphit: " + WaterEnemyMovement.GetspeedCapHit() +
            "\n" +
            "\ncanBreatheAir: " + WaterEnemyMovement.canBreatheAir + "   canDryUp: " + WaterEnemyMovement.canDryUp +
            "\ndryUpTime: " + WaterEnemyMovement.dryUpTimer.ToString("F2") + " / " + WaterEnemyMovement.dryUpTime +
            "nDebugDirection" + WaterEnemyMovement.debugDirection;
        chosenTimeText.text = dispTxt;
        
    }
}
