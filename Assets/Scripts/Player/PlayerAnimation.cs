using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController _playerController;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        SetAnimation();
    }

    // 跑动动画
    public void SetAnimation()
    {
        anim.SetFloat("velocityX", Math.Abs(rb.velocity.x));
        //jump animation 
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGrounded);
        anim.SetBool("isCrouch", _playerController.isCrouch);
        anim.SetBool("isDead", _playerController.isDead);
        anim.SetBool("isAttack", _playerController.isAttack);
    }

    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }

    public void PlayerAttack()
    {
        anim.SetTrigger("attack");
    }
}