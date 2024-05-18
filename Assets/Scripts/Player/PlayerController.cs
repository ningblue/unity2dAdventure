using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    private Rigidbody2D rb;
    public Vector2 inputDirection;
    [Header("基本参数")] public float speed;
    private float runSpeed;
    private float walkSpeed => speed / 2.5f;
    public float jumpForce;
    private PhysicsCheck physicsCheck;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // get the PhysicsCheck component
        physicsCheck = GetComponent<PhysicsCheck>();
        inputControl = new PlayerInputControl();
        inputControl.Gameplay.Jump.performed += ctx => Jump();

        #region 强制走路

        runSpeed = speed;
        inputControl.Gameplay.WalkButton.performed += ctx => speed
            = physicsCheck.isGrounded ? walkSpeed : runSpeed;
        inputControl.Gameplay.WalkButton.canceled += ctx => speed
            = physicsCheck.isGrounded ? runSpeed : walkSpeed;

        #endregion
    }


    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
        {
            faceDir = 1;
        }
        else if (inputDirection.x < 0)
        {
            faceDir = -1;
        }

        // 人物反转 localScale x轴的位置,如果位置和人物的比例发生变化,则对应数值也需要对应调整
        transform.localScale = new Vector3(faceDir, 1, 1);
    }

    private void Jump()
    {
        if (physicsCheck.isGrounded)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}