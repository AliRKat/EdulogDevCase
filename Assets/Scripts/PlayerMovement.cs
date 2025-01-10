using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator animator;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (GameManager.Instance.SelectedObject != null)
        {
            MoveToSelectedObject();
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        UpdateAnimator();
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            agent.destination = hit.point;
        }
    }

    // GameManager.Instance.SelectedObject should not be able to used like this.
    // Update this section
    private void MoveToSelectedObject()
    {
        Vector3 targetPosition = GameManager.Instance.SelectedObject.transform.position;
        agent.destination = targetPosition;

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Player reached the target: " + GameManager.Instance.SelectedObject.name);
                    GameManager.Instance.SelectedObject = null;
                }
            }
        }
        RotateTowardsTarget(targetPosition);
    }

    // Animator should get its own logic
    private void UpdateAnimator()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("forwardSpeed", speed);
    }

    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}