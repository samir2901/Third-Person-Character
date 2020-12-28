using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerController : MonoBehaviour
{    
    [Header("Player Movement Settings")]
    [SerializeField]
    private float playerWalkSpeed = 5f;
    [SerializeField]
    private float playerRunSpeed = 15f;
    [SerializeField]
    private float playerJumpHeight;
    [SerializeField]
    private Rigidbody playerRb;
    [SerializeField]
    private float turnSmoothTime = 0.1f;
    [SerializeField]
    private Transform TPPCam;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float speedSmoothTime = 0.1f;

    [Header("Player Ground Check Settings")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float checkRadius;
    [SerializeField]
    private LayerMask groundLayer;

    
    private float hInput;
    private float vInput;
    private bool isOnGround;
    private bool isOnGroundLastFrame;
    private float turnSmoothVel;

       
    // Update is called once per frame
    void Update()
    {        
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(hInput, 0f, vInput).normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);
        float speed = (running) ? playerRunSpeed : playerWalkSpeed;

        if (direction.magnitude >= 0.1f)        
        { 
            float targetAngle = Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg + TPPCam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerRb.velocity += moveDir.normalized * speed * Time.deltaTime;            
        }
        float animationSpeedPercent = ((running) ? 1f : 0.5f) * direction.magnitude;
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);


        isOnGround = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayer);

        
        if(isOnGround && !isOnGroundLastFrame)
        {
            animator.SetBool("isJumping", false);
        }

        isOnGroundLastFrame = isOnGround;

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            playerRb.velocity = Vector3.up * playerJumpHeight;
            animator.SetBool("isJumping", true);
        }
    }

    
}
