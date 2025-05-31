using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public GameObject startButton;
    public GameObject levelSelect;
    public GameObject settings;
    public GameObject credits;
    public GameObject inputField;
    private string menu;
    public GameObject baseScene;
    public GameObject creditsScene;
    public GameObject levelScene;
    public GameObject settingsScene;

    private bool start = false;
    private string alias;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideAllScenes();
        HideButtons();
        this.baseScene.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) & start)
        {
            SetMenu("ESC");
        }
    }

    public void HideButtons()
    { 
        this.settings.SetActive(false);
        this.credits.SetActive(false);
        this.levelSelect.SetActive(false);
        this.inputField.SetActive(false);
    }

    public void UnhideButtons()
    {
        this.settings.SetActive(true);
        this.credits.SetActive(true);
        this.inputField.SetActive(true);
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
            HideButtons();
            this.creditsScene.SetActive(true);
        }
        else if (scene == "levelScene")
        {
            HideButtons();
            this.levelScene.SetActive(true);
        }
        else if (scene == "settingsScene")
        {
            HideButtons();
            this.settingsScene.SetActive(true);
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
        this.settingsScene.SetActive(false);
    }

    public void StartGame()
    {
        this.start = true;
        this.startButton.SetActive(false);
        SetMenu("ESC");
    }

    public void SetAlias(string newAlias)
    {
        // TODO: Send this to the server, THEN set the alias == newAlias
        if (Input.GetKeyDown(KeyCode.Return))
        {
            this.alias = newAlias;
        }
    }

    public void SelectLvl(string lvl)
    {
        if (string.IsNullOrEmpty(this.alias)) {
            Debug.Log("\nalias can NOT be empty!!\n");
        }
        else 
        { 
            if (lvl == "1")
            {
                SceneManager.LoadScene("Mission-1");
            }
            else if (lvl == "2")
            {
                SceneManager.LoadScene("Mission-2");
            }
            else if (lvl == "3")
            {
                SceneManager.LoadScene("Mission-3");
            }
            else
            {
                Debug.LogError("Invalid lvl selected\n");
            }
        }
    }

}
