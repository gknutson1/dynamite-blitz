using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public AudioSource dynamiteFuse;
    public GameObject button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.dynamiteFuse.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void SwapFuse()
    {
        this.dynamiteFuse.Stop();
        this.button.SetActive(false);
        SceneManager.LoadScene("TitleScreen");
    }

}
