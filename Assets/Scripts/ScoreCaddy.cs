using UnityEngine;
using UnityEngine.Serialization;

public class ScoreCaddy : MonoBehaviour {
    public float time;
    public int kills;
    public int shotsFired;

    public int GetScore() {
        return (int)((kills * 10f * (kills / shotsFired)) - time);
    }
}
