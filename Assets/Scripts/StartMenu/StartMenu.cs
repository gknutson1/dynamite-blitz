using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public AudioSource dynamiteFuse;
    public AudioSource explosion;
    public GameObject button;
    private bool didPress = false;

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
        this.didPress = true;
        this.dynamiteFuse.Stop();
        this.explosion.Play();
        this.button.SetActive(false);
        StartCoroutine(WaitBehaviour());
    }

    public void EndGame()
    {
        this.dynamiteFuse.Stop();
        this.explosion.Play();
        this.button.SetActive(false);
        StartCoroutine(WaitBehaviour());
    }

    private IEnumerator WaitBehaviour()
    {
        yield return new WaitForSeconds(2f);
        if (didPress)
        {
            SceneManager.LoadScene("TitleScreen");
        }
        else {
            SceneManager.LoadScene("Game-Over");
        }

    }

}
