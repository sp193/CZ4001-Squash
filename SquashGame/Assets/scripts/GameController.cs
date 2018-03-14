using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private int score;
    private bool gameOver;
    public Text scoreText;
    public Text gameOverText;
    public Text restartGameText;
    private RacketController rController = null;

    // Use this for initialization
    void Start () {
        score = 0;
        scoreText.text = score.ToString();
        gameOver = false;
        gameOverText.enabled = false;
        restartGameText.enabled = false;
    }

    private void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

    public void Reset()
    {
        Debug.Log("GC Reset");

        gameOver = false;
        gameOverText.enabled = false;
        restartGameText.enabled = false;

        //Reset score
        ResetScore();
    }

    public void RestartGame()
    {
        //Reset the player.
        this.BroadcastMessage("Reset");
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }

    public void InitGameOver()
    {   //This function must not be named "GameOver", otherwise there will be self-recursion.
        gameOver = true;
        gameOverText.enabled = true;
        restartGameText.enabled = true;

        //this.BroadcastMessage("GameOver");
        rController.GameOver();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            if (rController.IsControllerSwung())
            {   //Change control set.
                RestartGame();
            }
        }

        if (rController == null)
            rController = FindObjectOfType<RacketController>();
    }
}
