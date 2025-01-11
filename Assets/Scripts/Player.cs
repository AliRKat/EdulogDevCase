using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private PlayerAnimator playerAnimator;
    private PlayerMovement playerMovement;

    private NavMeshAgent agent;

    public event Action<GameObject> OnPlowStart;
    public event Action<GameObject> OnPlowFinish;
    public event Action<GameObject> OnHarvestStart;
    public event Action<GameObject> OnHarvestFinish;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
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

    public void PlayerArrivedGatherable(GameObject gatherable)
    {
        Debug.Log("Player has arrived to this gatherable: " + gatherable.name);
        //OnGatherStarted?.Invoke(gatherable);
    }
}
