using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator animator;
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Player.Instance.IsPlayerBusy())
        {
            GameManager.Instance.ClearSelectedObj();
            return;
        }

        if (GameManager.Instance.IsObjectSelected())
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
    }

    public NavMeshAgent GetMovementAgent()
    {
        return agent;
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

    private void MoveToSelectedObject()
    {
        Vector3 targetPosition;
        Vector3 predefinedPosition = GameManager.Instance.GetSelectedObj().ReturnPredefinedTransform().position;
        if (predefinedPosition != null)
        {
            targetPosition = predefinedPosition;
        }
        else 
        {
            targetPosition = GameManager.Instance.GetSelectedObj().transform.position;
        }
        agent.destination = targetPosition;

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Player.Instance.PlayerArrivedDestination(GameManager.Instance.GetSelectedObj().gameObject);
                    GameManager.Instance.ClearSelectedObj();
                }
            }
        }
        RotateTowardsTarget(targetPosition);
    }

    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}