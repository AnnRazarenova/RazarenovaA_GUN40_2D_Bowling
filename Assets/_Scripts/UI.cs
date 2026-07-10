using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    

    [SerializeField]
    private TMP_Text _currentThrustText;

    [SerializeField]
    private TMP_Text _scoreText;

    [SerializeField]
    private TMP_Text _gameEndText;

    [SerializeField]
    private TMP_Text _gameEndScoreText;

    [SerializeField]
    private Button _restartGameButton;


    public static UI UIInstance { get { return FindObjectOfType<UI>(); } }

    private void OnEnable()
    {
        GameManager.OnScoreChangeUI += UpdateScoreUI;
        GameManager.OnGameEnded += ShowGameEnd;
        GameManager.OnGameRestarted += RestartGameUI;
        PushBall.OnThrustChange += ChangeShowenThrust;
    }

    private void OnDestroy()
    {
        GameManager.OnScoreChangeUI -= UpdateScoreUI;
        GameManager.OnGameEnded -= ShowGameEnd;
        GameManager.OnGameRestarted -= RestartGameUI;
        PushBall.OnThrustChange -= ChangeShowenThrust;
    }

    private void ChangeShowenThrust(float thrust)
    {
        _currentThrustText.text = Math.Round(thrust).ToString();
    }

    private void UpdateScoreUI(int newScore)
    {
        _scoreText.text = newScore.ToString();
    }

    private void ShowGameEnd(int finalScore)
    {
        _gameEndText.gameObject.SetActive(true);
        _gameEndText.text = $"Вы молодцы!";

        _gameEndScoreText.gameObject.SetActive(true);
        _gameEndScoreText.text = $"Вы набрали {finalScore} очков!"; 

        _restartGameButton.gameObject.SetActive(true);
    }
    private void RestartGameUI()
    {
        _gameEndText.gameObject.SetActive(false);

        _gameEndScoreText.gameObject.SetActive(false);

        _restartGameButton.gameObject.SetActive(false);
    }
}
