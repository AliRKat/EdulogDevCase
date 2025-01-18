using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    private void Start()
    {
        // SaveManager'dan shovel durumu yükleniyor
        var (_, _, shovelCollected) = SaveManager.LoadEquipment(new List<Equipment>());

        // Eðer shovel alýnmýþsa objeyi devre dýþý býrak
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

        // Obje sahneden kaldýrýlýyor
        gameObject.SetActive(false);
    }
}