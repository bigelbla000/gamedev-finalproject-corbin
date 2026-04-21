using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    public float groundSpeed;
    public float jumpSpeed;
    public float swimSpeed;
    public float swimJumpSpeed;
    public float speedPanelSpeed;
    [Range(0f, 1f)]
    public float groundDecay;
    public Rigidbody2D body;
    public bool grounded; //public for the sake of debugging.
    public BoxCollider2D groundCheck;
    public BoxCollider2D WaterCheck;
    //public BoxCollider2D SpeedPanelCheck; //Disabled.
    public LayerMask WaterMask;
    public LayerMask groundMask;
    //public LayerMask speedblockMask; //Disabled.
    public float xInput;
    float yInput;
    public bool waterdetect; //public for the sake of debugging.
    public bool speedPanel; //public for the sake of debugging.
    public bool SpeedPanelMomentumDEBUG; //Used for experimental momentum. Leave as false on startup to avoid problems.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        GetInput();
        MoveWithInput();
    }

    void GetInput() {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        //Make sure to change the settings. Select "Edit" > "Project Settings", select "Player", scroll down to "Other Settings", and find "Configuration", Locate "Active Input Handling" then make "Input System Package(new)" to "Both", and click "Apply" 
    }

    //The if statement below, disables its self when speedpanel or waterdetect is true, this was done to prevent "unpredictable behavior".
    void MoveWithInput() {
        if(Mathf.Abs(xInput) > 0 && ! speedPanel && ! waterdetect && SpeedPanelMomentumDEBUG == false) {
            body.linearVelocity = new Vector2(xInput * groundSpeed, body.linearVelocity.y);
            //body.AddForce(new Vector2((xInput * Time.deltaTime) * groundSpeed, 0));

            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1, 1);
       }


       if(Mathf.Abs(yInput) > 0 && (grounded || speedPanel)) {
            body.linearVelocity = new Vector2(body.linearVelocity.x, yInput * jumpSpeed);
       }
    //The next 2 if statements below are used when the player is inside of a water block.
       if(Mathf.Abs(xInput) > 0 && waterdetect) {
            body.linearVelocity = new Vector2(xInput * swimSpeed, body.linearVelocity.y);

            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1, 1);
       }

       if(Mathf.Abs(yInput) > 0 && waterdetect) {
            body.linearVelocity = new Vector2(body.linearVelocity.x, yInput * swimJumpSpeed);
       }
    //The if statement below is used to calculate the speed of when your on a speed panel.
       if(Mathf.Abs(xInput) > 0 && speedPanel) {
            body.linearVelocity = new Vector2(xInput * speedPanelSpeed, body.linearVelocity.y);
            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1, 1);
            SpeedPanelMomentumDEBUG = true; //I'm not sure how to program proper gradual slowdown physics for when you fly off of a speed panel so I made it so the normal 
        //movement physics aren't enabled when stepping off of a speed panel until grounded is set to true. Add a // to this line and the if(grounded) statement to disable.
       }
        
        if(grounded == true) {
            SpeedPanelMomentumDEBUG = false;
        }  
    }

    void FixedUpdate() {
        CheckGround();
        ApplyFriction();
        CheckWater();
        //CheckSpeedPanel();
       
    }

    void CheckGround() {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void ApplyFriction() {
     if (grounded && xInput == 0 && yInput == 0 && SpeedPanelMomentumDEBUG == false) {
        body.linearVelocity *= groundDecay;
        }
    }
    void CheckWater() {
        waterdetect = Physics2D.OverlapAreaAll(WaterCheck.bounds.min, WaterCheck.bounds.max, WaterMask).Length > 0;
    }
    //void CheckSpeedPanel() {
    //    speedPanel = Physics2D.OverlapAreaAll(SpeedPanelCheck.bounds.min, SpeedPanelCheck.bounds.max, speedblockMask).Length > 0;
    //}
}
