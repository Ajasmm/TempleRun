using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance;

    [SerializeField] GameObject player, enemy;
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] GameObject UIManager;

    public Action OnGameOver;
    public Action<int> OnScoreChange;

    string s_highScore = "HighScore";

    private int currentScore = 0;
    public int CurrentScore
    {
        get { return currentScore; }
        private set { currentScore = value; }
    }
    private void OnEnable()
    {
        if(instance == null)
            instance = this;
        else Destroy(gameObject);

        player.SetActive(true); 
        enemy.SetActive(true);
        UIManager.SetActive(true);

        OnGameOver += OnPlayerDead;
        OnGameOver += WhenGameOver;
    }

    private void OnDisable()
    {
        OnGameOver -= OnPlayerDead;
        OnGameOver -= WhenGameOver;
    }

    public void OnPlayerDead()
    {
        virtualCam.LookAt = enemy.transform;
        virtualCam.Follow= enemy.transform;
        virtualCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, 2.6F, 5F);
    }

    public void StartFollow()
    {
        virtualCam.Follow = player.transform;
    }

    public void IncreaseScore()
    {
        CurrentScore += 1;
        if(OnScoreChange != null) OnScoreChange(currentScore);
    }
    private void WhenGameOver()
    {
        if (PlayerPrefs.HasKey(s_highScore))
        {
            int currentHighScore = PlayerPrefs.GetInt(s_highScore);
            if (currentHighScore < currentScore)
                PlayerPrefs.SetInt(s_highScore, currentScore);
        }
        else
        {
            PlayerPrefs.SetInt(s_highScore, currentScore);
        }
        PlayerPrefs.Save();

    }

}
