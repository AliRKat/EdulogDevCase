using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "Gatherable")]
public class GatherableSO : ScriptableObject
{
    public GatherableStates state;
    public string gatherableName;
    public float plowTime;
    public float growTime;
    public float harvestTime;
    public int harvestXPAmount;
    public int harvestAmount;
    public int gatherItemValue;
}