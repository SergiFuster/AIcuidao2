using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winController : MonoBehaviour
{
    public GameObject winWallpaper;

    private void Start()
    {
        winWallpaper.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        winWallpaper.SetActive(true);
        Time.timeScale = 0f;
    }
}
