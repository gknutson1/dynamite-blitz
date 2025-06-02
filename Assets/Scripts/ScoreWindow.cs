using UnityEngine;

public class ScoreWindow : MonoBehaviour {
    private ScoreCaddy _caddy;
    
    private TMPro.TMP_Text _scoreText;
    
    
    void Start() {
        _caddy = GameObject.Find("score_caddy").GetComponent<ScoreCaddy>();
        _scoreText = GameObject.Find("Canvas").transform.Find("report").GetComponent<TMPro.TMP_Text>();
        _scoreText.text = $"REPORT:\n" +
                          $"{_caddy.kills} kills x 10pts\n" +
                          $"x {(_caddy.shotsFired > 0 ? (float) _caddy.kills / (float) _caddy.shotsFired : 1):.00} (accuracy modifier)\n" +
                          $"- {_caddy.time:.00} (time penalty)\n\n" +
                          $"TOTAL SCORE:\n" +
                          $"{_caddy.GetScore()}";
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
