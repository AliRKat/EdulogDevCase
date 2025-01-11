using System;
using System.Collections;
using UnityEngine;

public class PlayerGathering : MonoBehaviour
{
    public event Action<GameObject, GatherableStates> OnInteractionStart;
    public event Action<GameObject, GatherableStates> OnInteractionEnd;
    private Gatherable currentGatherable;

    // takes the interactable game object and depending on the gatherable state of it starts an action like plow or gather
    public void HandleGatherableInteraction(GameObject gatherable)
    {
        if (Player.Instance.IsPlayerBusy())
        {
            return;
        }

        currentGatherable = gatherable.GetComponent<Gatherable>();

        if (currentGatherable == null)
        {
            Debug.LogWarning("PlayerGathering: The object doesn't have a Gatherable component!");
            return;
        }

        switch (currentGatherable.GetCurrentState())
        {
            case GatherableStates.Plowable:
                StartPlowing(gatherable);
                break;

            case GatherableStates.Gatherable:
                StartGathering(gatherable);
                break;

            case GatherableStates.Growing:
                Debug.Log("PlayerGathering: Nothing to do with a Growing object: " + gatherable.name);
                break;

            default:
                Debug.Log("PlayerGathering: Unknown object state: " + gatherable.name);
                break;
        }
    }
    #region Start&StopMethodsForInteractions
    // do not use this methods or listen to these events to notify another class, use the one on the 'Player'
    private void StartGathering(GameObject gatherable)
    {
        Debug.Log("PlayerGathering: Starting to gather from: " + gatherable.name);
        OnInteractionStart?.Invoke(gatherable, GatherableStates.Gatherable);
        StartCoroutine(GatherCoroutine(gatherable));
    }
    private void StartPlowing(GameObject plowable)
    {
        Debug.Log("PlayerGathering: Starting to plow: " + plowable.name);
        OnInteractionStart?.Invoke(plowable, GatherableStates.Plowable);
        StartCoroutine(PlowCoroutine(plowable));
    }
    private void StopGathering()
    {
        Debug.Log("PlayerGathering: Gathering process stopped.");
        OnInteractionEnd?.Invoke(currentGatherable.gameObject, GatherableStates.Gatherable);
        currentGatherable = null;
    }
    private void StopPlowing()
    {
        Debug.Log("PlayerGathering: Plowing process stopped.");
        OnInteractionEnd?.Invoke(currentGatherable.gameObject, GatherableStates.Plowable);
        currentGatherable = null;
    }
    #endregion
    private IEnumerator GatherCoroutine(GameObject gatherable)
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("PlayerGathering: Finished gathering: " + gatherable.name);
        StopGathering();
    }

    private IEnumerator PlowCoroutine(GameObject plowable)
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("PlayerGathering: Finished plowing: " + plowable.name);
        StopPlowing();
    }
}