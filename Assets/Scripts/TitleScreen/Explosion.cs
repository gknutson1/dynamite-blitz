using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioSource explosion;

    public void Play()
    { 
        explosion.Play();
    }
}
