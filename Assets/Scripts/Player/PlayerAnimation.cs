using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SetAnimation();
    }

    // 跑动动画
    public void SetAnimation()
    {
        anim.SetFloat("velocityX", Math.Abs(rb.velocity.x));
    }
}