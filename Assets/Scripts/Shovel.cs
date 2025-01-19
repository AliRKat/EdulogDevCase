using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    private void Start()
    {
        var (_, _, shovelCollected) = SaveManager.LoadEquipment(new List<Equipment>());
        if (shovelCollected)
        {
            gameObject.SetActive(false);
        }
    }

    public void Collect()
    {
        var (equipmentsOwned, equipped, _) = SaveManager.LoadEquipment(new List<Equipment>());
        SaveManager.SaveEquipment(equipmentsOwned, equipped, true);
        gameObject.SetActive(false);
    }
}