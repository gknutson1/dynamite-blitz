using UnityEngine;
using UnityEngine.Serialization;

public class ScoreCaddy : MonoBehaviour {
    public float time;
    public int kills;
    public int shotsFired;

    public int GetScore() {
        return (int)(( kills * 10f * (shotsFired > 0 ? (float) kills / (float) shotsFired : 1)) - time);
    }

    void Start() {
        DontDestroyOnLoad(gameObject);
    }
}
