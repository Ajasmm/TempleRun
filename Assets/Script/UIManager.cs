using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject gameoverWindow;

    bool isGameOver = false;
    string s_highScore = "HighScore";

    private void OnEnable()
    {
        OnScoreChange(GamePlayManager.instance.CurrentScore);
        GamePlayManager.instance.OnScoreChange += OnScoreChange;
        GamePlayManager.instance.OnGameOver += OnGameOver;

        if(PlayerPrefs.HasKey(s_highScore)) highScoreText.text = "HighScore : " + PlayerPrefs.GetInt(s_highScore).ToString();
        else highScoreText.gameObject.SetActive(false);
    }

    private void OnDisable()
    {

        GamePlayManager.instance.OnScoreChange -= OnScoreChange;
        GamePlayManager.instance.OnGameOver -= OnGameOver;
    }

    private void Update()
    {
        if (!isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene(0);
    }

    private void OnScoreChange(int score)
    {
        if(scoreText != null) scoreText.text = "Score : " + score.ToString();

    }

    private void OnGameOver()
    {
        scoreText.gameObject.SetActive(false);
        gameoverWindow.SetActive(true);
        isGameOver = true;
    }
}
