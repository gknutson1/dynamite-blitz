using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //A reference to the game manager
    public GameManager gameManager;

    // When an object enters the finish zone, let the
    // game manager know that the current game has ended
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Scenes/Score");
            gameManager.FinishedGame();
        }
    }
}
