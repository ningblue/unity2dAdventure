using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("检测参数")]
   
    public float checkRaduis;
    public LayerMask groundLayer; // 地面的Layer
    public Vector2 bottomOffset; // 地面检测的偏移量
    [Header("状态")]
    public bool isGrounded;
    private void Update()
    {
        Check();
    }

    private void Check()
    {
        // 检测地面
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset, checkRaduis, groundLayer);
    }
    
    // 在Scene视图中显示地面检测的范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position+bottomOffset, checkRaduis);
    }
}