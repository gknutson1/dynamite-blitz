using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public GameObject startButton;
    public GameObject levelSelect;
    public GameObject settings;
    public GameObject credits;
    private string menu;
    public GameObject baseScene;
    public GameObject creditsScene;
    public GameObject levelScene;
    public GameObject settingsScene;
    public GameObject startScene;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.creditsScene.SetActive(false);
        this.levelScene.SetActive(false);
        this.settingsScene.SetActive(false);
        this.startScene.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetMenu("ESC");
        }
    }

    public void HideButons()
    { 
        this.settings.SetActive(false);
        this.credits.SetActive(false);
        this.startButton.SetActive(false);
        this.levelSelect.SetActive(false);
    }

    public void UnhideButtons()
    {
        this.settings.SetActive(true);
        this.credits.SetActive(true);
        this.startButton.SetActive(true);
        this.levelSelect.SetActive(true);
    }

    public void SetMenu(string menuName)
    {
        if (menuName == "ESC")
        {
            HideScenesExcept("baseScene");
        }
        else if (menuName == "SET")
        {
            HideScenesExcept("settingsScene");
        }
        else if (menuName == "CRED")
        {
            HideScenesExcept("creditsScene");
        }
        else if (menuName == "LVL")
        {
            HideScenesExcept("levelScene");
        }
        else if (menuName == "STRT")
        {
            HideScenesExcept("sartScene");
        }
        else {
            Debug.Log("Unkown Code!\nPlease Try Again.");
        }
    }

    private void HideScenesExcept(string scene)
    {
        HideAllScenes();
        if (scene == "baseScene")
        {
            this.baseScene.SetActive(true);
            UnhideButtons();
        }
        else if (scene == "creditsScene")
        {
            HideButons();
            this.creditsScene.SetActive(true);
        }
        else if (scene == "levelScene")
        {
            HideButons();
            this.levelScene.SetActive(true);
        }
        else if (scene == "settingsScene")
        {
            HideButons();
            this.settingsScene.SetActive(true);
        }
        else if (scene == "startScene")
        {
            HideButons();
            this.startScene.SetActive(true);
        }
        else {
            Debug.Log("Unkown Code!\nPlease Try Again.");
        }
    }

    private void HideAllScenes()
    { 
        this.baseScene.SetActive(false);
        this.creditsScene.SetActive(false);
        this.levelScene.SetActive(false);
        this.startScene.SetActive(false);
        this.settingsScene.SetActive(false);
    }


}
