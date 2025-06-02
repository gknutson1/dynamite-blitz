using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Place holders to allow connecting to other objects
    public Transform spawnPoint;
    public GameObject player;

    // Flags that control the state of the game
    private float elapsedTime = 0;
    private bool isRunning = false;
    private bool isFinished = false;


    // Use this for initialization
    void Start()
    {
        //Tell Unity to allow character controllers to have their position set directly. This will enable our respawn to work
        Physics.autoSyncTransforms = true;
        StartGame();
    }


    //This resets to game back to the way it started
    private void StartGame()
    {
        elapsedTime = 0;
        isRunning = true;
        isFinished = false;

        // Move the player to the spawn point, and allow it to move.
        PositionPlayer();
    }


    // Update is called once per frame
    void Update()
    {
        // Add time to the clock if the game is running
        /*if (isRunning)
        {
            elapsedTime = elapsedTime + Time.deltaTime;
        }*/
    }


    //Runs when the player needs to be positioned back at the spawn point
    public void PositionPlayer()
    {
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;
    }


    // Runs when the player enters the finish zone
    public void FinishedGame()
    {
        SceneManager.LoadScene("Score");
    }
}
