using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private static Player instance;
    private PlayerAnimator playerAnimator;
    private PlayerMovement playerMovement;

    private NavMeshAgent agent;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Player();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerMovement = GetComponent<PlayerMovement>();

        agent = playerMovement.GetMovementAgent();
    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator.UpdateMovement(agent.velocity.magnitude);
    }
}
