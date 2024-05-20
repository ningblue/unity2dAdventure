using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private CapsuleCollider2D _coll;
    public Vector2 inputDirection;
    [Header("基本参数")] public float speed;
    private float runSpeed;
    private float walkSpeed => speed / 2.5f;
    public float jumpForce;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private Vector2 originalOffset;
    private Vector2 originalSize;

    [Header("状态")] public bool isCrouch;
    public bool isHurt;
    public float hurtForce;
    public bool isDead;
    public bool isAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // get the PhysicsCheck component
        physicsCheck = GetComponent<PhysicsCheck>();
        _coll = GetComponent<CapsuleCollider2D>();
        originalOffset = _coll.offset;
        originalSize = _coll.size;
        inputControl = new PlayerInputControl();
        playerAnimation = GetComponent<PlayerAnimation>();

        // 跳跃
        inputControl.Gameplay.Jump.performed += ctx => Jump();

        #region 强制走路

        runSpeed = speed;
        inputControl.Gameplay.WalkButton.performed += ctx => speed
            = physicsCheck.isGrounded ? walkSpeed : runSpeed;
        inputControl.Gameplay.WalkButton.canceled += ctx => speed
            = physicsCheck.isGrounded ? runSpeed : walkSpeed;

        #endregion

        #region 攻击

        inputControl.Gameplay.Attack.started += PlayerAttack;

        #endregion
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        playerAnimation.PlayerAttack();
        isAttack = true;
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
        if (!isHurt)
            Move();
    }

    public void Move()
    {
        if (!isCrouch)
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

        // 下蹲
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGrounded;
        // 修改碰撞体的大小
        if (isCrouch)
        {
            // 人物下蹲时,碰撞体的大小
            _coll.offset = new Vector2(_coll.offset.x, 0.85f);
            _coll.size = new Vector2(_coll.size.x, 1.7f);
        }
        else
        {
            _coll.offset = originalOffset;
            _coll.size = originalSize;
        }
    }

    private void Jump()
    {
        if (physicsCheck.isGrounded)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    #region UnityEvent

    public void GetHurt(Transform attacker)
    {
        // 受伤
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable(); //禁用输入
    }

    #endregion
}