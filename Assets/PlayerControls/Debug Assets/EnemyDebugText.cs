using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDebugText : MonoBehaviour
{   
    public TextMeshProUGUI isChasing;
    public TextMeshPro chosenTimeText;
    public float chosenTime;
    public EnemyMovementAdvanced EnemyMovementAdvanced;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        if (EnemyMovementAdvanced.body.transform.localScale == new Vector3(-1, 1, 1))
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
        chosenTime = EnemyMovementAdvanced.GetchosenTime();
        string dispTxt = "ChosenTime: " + chosenTime.ToString("F2") + "    waitingState:" + EnemyMovementAdvanced.GetWaitingState() +
            "\nEvasionTimer: " + EnemyMovementAdvanced.EvasionTimer.ToString("F2") + "  evading: " + EnemyMovementAdvanced.GetEvading() +
            "\nDist Until Chase: " + EnemyMovementAdvanced.debugDISTANCE.ToString("F2") + "  isChasing: " + EnemyMovementAdvanced.isChasing +
            "\nX Velocity: " + EnemyMovementAdvanced.body.linearVelocityX.ToString("F2") + "  speedcaphit: " + EnemyMovementAdvanced.GetspeedCapHit();
        chosenTimeText.text = dispTxt;
        
    }
}
