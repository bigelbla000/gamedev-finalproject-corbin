using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    public float groundSpeed;
    public float jumpSpeed;
    [Range(0f, 1f)]
    public float groundDecay;
    public Rigidbody2D body;
    public bool grounded;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    float xInput;
    float yInput;

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

    void MoveWithInput() {
        if(Mathf.Abs(xInput) > 0) {
            body.linearVelocity = new Vector2(xInput * groundSpeed, body.linearVelocity.y);

            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1, 1);
       }

       if(Mathf.Abs(yInput) > 0 && grounded) {
            body.linearVelocity = new Vector2(body.linearVelocity.x, yInput * jumpSpeed);
       }
    }

    void FixedUpdate() 
    {
        CheckGround();
        ApplyFriction();
       
    }

    void CheckGround() {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void ApplyFriction() {
     if (grounded && xInput == 0 && yInput == 0) {
        body.linearVelocity *= groundDecay;
        }
    }

}
