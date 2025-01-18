using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    private void Start()
    {
        // SaveManager'dan shovel durumu y�kleniyor
        var (_, _, shovelCollected) = SaveManager.LoadEquipment(new List<Equipment>());

        // E�er shovel al�nm��sa objeyi devre d��� b�rak
        if (shovelCollected)
        {
            gameObject.SetActive(false);
        }
    }

    public void Collect()
    {
        // Shovel durumu kaydediliyor
        var (equipmentsOwned, equipped, _) = SaveManager.LoadEquipment(new List<Equipment>());
        SaveManager.SaveEquipment(equipmentsOwned, equipped, true);

        // Obje sahneden kald�r�l�yor
        gameObject.SetActive(false);
    }
}