using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("ShovelAdded") > 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Collect()
    {
        PlayerPrefs.SetInt(("ShovelAdded"), 1);
        Player.Instance.GetComponent<PlayerEquipment>().Add(EquipmentType.Shovel);
        gameObject.SetActive(false);
    }
}