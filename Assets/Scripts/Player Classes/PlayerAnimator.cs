using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        SubscribeToPlayerEvents();
    }
    public void UpdateMovement(float speed) 
    {
        animator.SetFloat("forwardSpeed", speed);
    }
    public void StartGather(GameObject _)
    {
        animator.SetBool("isDigging", true);
    }
    public void StopGather(GameObject _)
    {
        animator.SetBool("isDigging", false);
    }

    private void SubscribeToPlayerEvents() 
    {
        Player.Instance.OnPlowStart += StartGather;
        Player.Instance.OnHarvestStart += StartGather;
        Player.Instance.OnPlowFinish += StopGather;
        Player.Instance.OnHarvestFinish += StopGather;
    }
    private void OnDisable()
    {
        Player.Instance.OnPlowStart -= StartGather;
        Player.Instance.OnHarvestStart -= StartGather;
        Player.Instance.OnPlowFinish -= StopGather;
        Player.Instance.OnHarvestFinish -= StopGather;
    }
}