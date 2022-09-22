using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    [SerializeField] Text currentScore;
    [SerializeField] FB_ScoreController scoreController;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        scoreController.WriteNewScore(Score.score);
    }
    public void Replay()
    {
        SceneManager.LoadScene("Main Game");
    }
}
