using UnityEngine;

public class Shovel : MonoBehaviour
{
    private string shovelKey = "ShovelCollected";

    private void Start()
    {
        // E�er shovel al�nm��sa, objeyi devre d��� b�rak
        if (PlayerPrefs.GetInt(shovelKey, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void Collect()
    {
        // Shovel al�nd� olarak i�aretle
        PlayerPrefs.SetInt(shovelKey, 1);
        PlayerPrefs.Save();

        // Obje sahneden kald�r
        gameObject.SetActive(false);
    }
}