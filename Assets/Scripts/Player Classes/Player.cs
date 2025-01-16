using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event Action<GameObject> OnPlowStart;
    public event Action<GameObject> OnPlowFinish;
    public event Action<GameObject> OnHarvestStart;
    public event Action<GameObject> OnHarvestFinish;
    public event Action OnInventoryUpdated;
    public event Action OnMarketEnter;

    private PlayerAnimator playerAnimator;
    private PlayerMovement playerMovement;
    private PlayerGathering playerGathering;
    private PlayerInventory playerInventory;
    private NavMeshAgent agent;
    private bool isBusy;

    // singleton logic
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

    void Start()
    {
        // cache related stuff
        playerAnimator = GetComponent<PlayerAnimator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerGathering = GetComponent<PlayerGathering>();
        playerInventory = GetComponent<PlayerInventory>();
        agent = playerMovement.GetMovementAgent();

        SubscribeToServiceEvents();
    }

    void Update()
    {
        playerAnimator.UpdateMovement(agent.velocity.magnitude);
    }

    // notifies classes that player has arrived to a destination given by game manager
    public void PlayerArrivedDestination(GameObject destination)
    {
        if (destination.GetComponent<Gatherable>() != null) 
        {
            playerGathering.HandleGatherableInteraction(destination);
            if (destination.GetComponent<Gatherable>().GetCurrentState() != GatherableStates.Growing)
            {
                isBusy = true;
            }
        }

        if (destination.GetComponent<Market>() != null) 
        {
            OnMarketEnter?.Invoke();
            isBusy = true;
        }
    }

    public bool IsPlayerBusy()
    {
        return isBusy;
    }

    public void SetPlayerFree()
    {
        isBusy = false;
    }

    public Dictionary<ItemBase, int> GetPlayerInventory()
    {
        return playerInventory.inventory;
    }

    private void SubscribeToServiceEvents()
    {
        playerGathering.OnInteractionStart += HandleInteractionStart;
        playerGathering.OnInteractionEnd += HandleInteractionEnd;
        playerInventory.InventoryUpdated += HandleInventoryUpdate;
    }

    private void HandleInventoryUpdate()
    {
        OnInventoryUpdated?.Invoke();
    }

    // invokes related start events depending on the state of the gatherable
    // use the invoked events for the animator and equipment classes to handle stuff up
    private void HandleInteractionStart(GameObject gatherable, GatherableStates state)
    {
        switch (state)
        {
            case GatherableStates.Plowable:
                Debug.Log("Player: Plow interaction started.");
                OnPlowStart?.Invoke(gatherable);
                break;

            case GatherableStates.Gatherable:
                Debug.Log("Player: Harvest interaction started.");
                OnHarvestStart?.Invoke(gatherable);
                break;

            case GatherableStates.Growing:
                Debug.Log("Player: Currently won't do anything related to growing.");
                isBusy = false;
                break;

            default:
                Debug.Log("Player: Unknown interaction started.");
                break;
        }
    }

    // invokes related finish events depending on the state of the gatherable
    private void HandleInteractionEnd(GameObject gatherable, GatherableStates state)
    {
        switch (state)
        {
            case GatherableStates.Plowable:
                Debug.Log("Player: Plow interaction finished.");
                OnPlowFinish?.Invoke(gatherable);
                break;

            case GatherableStates.Gatherable:
                Debug.Log("Player: Harvest interaction finished.");
                OnHarvestFinish?.Invoke(gatherable);
                break;

            case GatherableStates.Growing:
                Debug.Log("Player: Currently won't do anything related to growing.");
                break;

            default:
                Debug.Log("Player: Unknown interaction finished.");
                break;
        }
        isBusy = false;
    }
}