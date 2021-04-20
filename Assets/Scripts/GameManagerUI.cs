using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{
    [SerializeField] List<Image> imagesForAvaragecolored;
    [SerializeField] List<GameObject> vklVyklOnStart;
    
    private void Awake()
    {
        PlayerSetingsUI.ChangeAverageColor += ChangeAverageColor;
    }

    private void OnDestroy()
    {
        PlayerSetingsUI.ChangeAverageColor -= ChangeAverageColor;
    }

    private void Start()
    {
        for (int i = 0; i < vklVyklOnStart.Count; i++)
        {
            vklVyklOnStart[i].SetActive(true);
            vklVyklOnStart[i].SetActive(false);
        }
    }

    private void ChangeAverageColor(Color color)
    {
        for (int i = 0; i < imagesForAvaragecolored.Count; i++)
            imagesForAvaragecolored[i].color = color;
    }
}
