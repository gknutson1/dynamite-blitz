using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float time = 10;
    public TMP_Text timeLeft;
    public GameObject handler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            timeLeft.text = string.Format("{0:N2}", time);
        }
        else {
            handler.SendMessage("SwapFuse");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            handler.SendMessage("SwapFuse");
        }
    }
}
