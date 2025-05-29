using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float time = 10;
    public TMP_Text timeLeft;
    public GameObject handler;

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            timeLeft.text = string.Format("{0:N2}", time);
        }
        else {
            handler.SendMessage("EndGame");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            handler.SendMessage("SwapFuse");
        }
    }
}
