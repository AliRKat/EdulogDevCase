using UnityEngine;

public class Shovel : MonoBehaviour
{
    private string shovelKey = "ShovelCollected";

    private void Start()
    {
        // Eðer shovel alýnmýþsa, objeyi devre dýþý býrak
        if (PlayerPrefs.GetInt(shovelKey, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void Collect()
    {
        // Shovel alýndý olarak iþaretle
        PlayerPrefs.SetInt(shovelKey, 1);
        PlayerPrefs.Save();

        // Obje sahneden kaldýr
        gameObject.SetActive(false);
    }
}