using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseController : MonoBehaviour
{
    public GameObject loseWallpaper;
    public static LoseController instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        loseWallpaper.SetActive(false);
    }

    public void LoseGame()
    {
        loseWallpaper.SetActive(true);
        Time.timeScale = 0f;
    }
}
