using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerDebugText : MonoBehaviour
{   
    public TextMeshPro DebugTextBox;
    public PlayerHealth PlayerHealth;
    public PlayerMovement PlayerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        if (PlayerMovement.body.transform.localScale == new Vector3(-1, 1, 1))
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
        string dispTxt = "HP: " + PlayerHealth.health + " / " + PlayerHealth.maxHealth;
        DebugTextBox.text = dispTxt;
        
    }
}
