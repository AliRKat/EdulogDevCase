using System;
using System.Collections;
using UnityEngine;

public class Gatherable : MonoBehaviour
{
    public GatherableSO gatherableData;
    [SerializeField] private GatherableStates state; // serializing for debug purposes
    [SerializeField] private GameObject FXObj;
    private float plowTime;
    private float growTime;
    private float harvestTime;
    private int harvestXpAmount;
    private int harvestAmount;
    private int gatherItemValue;
    private ItemBase itemToGather;
    private string gatherableId;

    void Start()
    {
        plowTime = gatherableData.plowTime;
        growTime = gatherableData.growTime;
        harvestTime = gatherableData.harvestTime;
        state = gatherableData.state;
        harvestXpAmount = gatherableData.harvestXPAmount;
        harvestAmount = gatherableData.harvestAmount;
        gatherItemValue = gatherableData.gatherItemValue;

        itemToGather = new(gatherableData.name, gatherItemValue);
        gatherableId = gatherableData.name + "_" + transform.position.ToString();
        LoadState();
        SubscribeToPlayerEvents();

        if (state == GatherableStates.Gatherable)
        {
            FXObj.transform.position = new Vector3(0, 0, 0);
        }
    }
    public void SubscribeToPlayerEvents()
    {
        Player.Instance.OnPlowStart += HandlePlowStart;
        Player.Instance.OnPlowFinish += HandlePlowFinish;
        Player.Instance.OnHarvestStart += HandleHarvestStart;
        Player.Instance.OnHarvestFinish += HandleHarvestFinish;
    }
    public string GetUniqueId()
    {
        return gatherableId;
    }

    #region State Handlers
    private void LoadState()
    {
        var savedGatherables = SaveManager.LoadGatherables();
        var savedData = savedGatherables.Find(g => g.Id == gatherableId);
        state = savedData != null ? savedData.State : gatherableData.state;

        if(state == GatherableStates.Growing)
        {
            StartCoroutine(GrowCoroutine(this.gameObject.GetComponent<Gatherable>(), growTime, GatherableStates.Gatherable));
        }
    }

    private void SaveState()
    {
        var savedGatherables = SaveManager.LoadGatherables();
        var gatherableData = savedGatherables.Find(g => g.Id == gatherableId);

        if (gatherableData != null)
        {
            gatherableData.State = state;
        }
        else
        {
            savedGatherables.Add(new GatherableData { Id = gatherableId, State = state });
        }

        SaveManager.SaveGatherables(savedGatherables);
    }

    private void SwitchState(Gatherable obj, GatherableStates state)
    {
        obj.state = state;
        SaveState();
    }
    #endregion
    #region Getters
    public float GetPlowTime()
    {
        return plowTime;
    }
    public float GetGrowTime()
    {
        return growTime;
    }
    public float GetHarvestTime()
    {
        return harvestTime;
    }
    public int GetHarvestXpAmount()
    {
        return harvestXpAmount;
    }
    public int GetHarvestAmount()
    {
        return harvestAmount;
    }
    public GatherableStates GetCurrentState()
    {
        return state;
    }

    public ItemBase GetGatherItem()
    {
        return itemToGather;
    }
    #endregion
    #region Event Handlers
    // Listens 'Player' class' OnPlowStart event 
    private void HandlePlowStart(GameObject obj)
    {
        if (obj == this.gameObject && state != GatherableStates.Growing)
        {
            // FX and other visuals related to Gatherable will be handled here
            Debug.Log("Gatherable: Handling plow start");
        }
    }

    private IEnumerator GrowCoroutine(Gatherable obj, float time, GatherableStates state)
    {
        float startY = obj.FXObj.transform.position.y;
        float endY = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            float newY = Mathf.Lerp(startY, endY, elapsedTime / time);
            obj.FXObj.transform.position = new Vector3(obj.FXObj.transform.position.x, newY, obj.FXObj.transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.FXObj.transform.position = new Vector3(obj.FXObj.transform.position.x, endY, obj.FXObj.transform.position.z);

        SwitchState(obj, state);
    }

    // Listens 'Player' class' OnPlowFinish event 
    private void HandlePlowFinish(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            // FX and other visuals related to Gatherable will be handled
            Gatherable gatherableObj = obj.GetComponent<Gatherable>();
            Debug.Log("Gatherable: Handling plow finish");
            SwitchState(gatherableObj, GatherableStates.Growing);
            StartCoroutine(GrowCoroutine(gatherableObj, growTime, GatherableStates.Gatherable));
        }
    }

    // Listens 'Player' class' OnHarvestStart event 
    private void HandleHarvestStart(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            // FX and other visuals related to Gatherable will be handled here
            Debug.Log("Gatherable: Handling harvest start");
        }
    }
    // Listens 'Player' class' OnHarvestFinish event 
    private void HandleHarvestFinish(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            Gatherable gatherableObj = obj.GetComponent<Gatherable>();
            Debug.Log("Gatherable: Handling harvest finish");
            StartCoroutine(ReturnFXToOriginalPosition(gatherableObj));
            SwitchState(gatherableObj, GatherableStates.Plowable);
        }
    }

    private IEnumerator ReturnFXToOriginalPosition(Gatherable gatherableObj)
    {
        Vector3 originalPosition = gatherableObj.FXObj.transform.position;
        float targetY = -1f;
        float timeToReturn = 0.1f;
        float elapsedTime = 0f;

        while (elapsedTime < timeToReturn)
        {
            float newY = Mathf.Lerp(gatherableObj.FXObj.transform.position.y, targetY, elapsedTime / timeToReturn);
            gatherableObj.FXObj.transform.position = new Vector3(gatherableObj.FXObj.transform.position.x, newY, gatherableObj.FXObj.transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gatherableObj.FXObj.transform.position = new Vector3(gatherableObj.FXObj.transform.position.x, targetY, gatherableObj.FXObj.transform.position.z);
    }

    #endregion
    private void OnDisable()
    {
        Player.Instance.OnPlowStart -= HandlePlowStart;
        Player.Instance.OnPlowFinish -= HandlePlowFinish;
        Player.Instance.OnHarvestStart -= HandleHarvestStart;
        Player.Instance.OnHarvestFinish -= HandleHarvestFinish;
    }
}