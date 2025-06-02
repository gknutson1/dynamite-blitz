using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreContinue : MonoBehaviour
{
    public void OnClick() {
        Destroy(GameObject.Find("score_caddy"));
        SceneManager.LoadScene("Scenes/TitleScreen");
    }
}
