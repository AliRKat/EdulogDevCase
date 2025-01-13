using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void UpdateMovement(float speed) 
    {
        animator.SetFloat("forwardSpeed", speed);
    }
}