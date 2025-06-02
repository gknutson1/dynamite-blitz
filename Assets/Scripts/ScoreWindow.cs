using UnityEngine;

public class ScoreWindow : MonoBehaviour {
    private ScoreCaddy _caddy;
    
    private TMPro.TMP_Text _scoreText;
    private TMPro.TMP_Text _boardText;
    
    
    void Start() {
        GameObject.Find("score_caddy").GetComponent<ScoreCaddy>();
        _scoreText = GameObject.Find("Canvas").transform.Find("report").GetComponent<TMPro.TMP_Text>();
        _boardText = GameObject.Find("Canvas").transform.Find("board").GetComponent<TMPro.TMP_Text>();
    }

    private void UpdateScoreText() {
        _scoreText.text = $"REPORT:\n" +
                          $"{_caddy.kills} kills x 10pts\n" +
                          $"x {_caddy.kills / _caddy.shotsFired} (accuracy modifier)\n" +
                          $"- {_caddy.time} (time penalty)\n\n" +
                          $"TOTAL SCORE:\n" +
                          $"{_caddy.GetScore()}";
        
        Destroy(_caddy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
