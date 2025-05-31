using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

    public float speed = 2f;
    public float upperXValue = 17.8f;
    public float lowerXValue = -35.6f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0f, 0f);


       // Debug.Log(transform.position.x);
        if (transform.position.x >= upperXValue)
        {
            Debug.Log("Back to start!");
            transform.Translate(lowerXValue, 0f, 0f);
        }
    }
}
