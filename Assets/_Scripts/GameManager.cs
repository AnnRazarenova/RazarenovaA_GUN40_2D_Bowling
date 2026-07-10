using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static event Action<int> OnScoreChangeUI;
    public static event Action<int> OnGameEnded;
    public static event Action OnResetPins;
    public static event Action OnShowResult;
    public static event Action OnGameRestarted;

    [SerializeField]
    private GameObject _ballPrefab;

    [SerializeField]
    private GameObject _pinsPrefab;

    [SerializeField]
    private GameObject _ballPoint;

    [SerializeField]
    private GameObject _pinsPoint;

    [SerializeField] 
    private int _maxPins = 10;

    [SerializeField]
    private float _waitTime = 5f;

    private Coroutine _waitCoroutine = null;

    private bool _isBallEntered = false;

    private GameObject _ballInGame;
    private GameObject _pinsInGame;

    private int _totalGameScore = 0;
    private int _fallenPinsCount = 0;

    private int _rollsLeft = 3;

    private bool _gameEnd = false;

    private Result _result = Result.Open;

    public Result Result {  get { return _result; } }

    public bool GameEnd {  get { return _gameEnd; } }

    public static GameManager GameManagerInstance { get { return FindObjectOfType<GameManager>(); } }

    public PushBall BallInGamePrefab { get { return _ballInGame.GetComponent<PushBall>(); } }


    private void Awake()
    {
        SpawnInStart();
        ScoreManager.OnFallenPinsCountChange += FallenPinsCountChange;
        PushBall.OnBallThrown += ThrowBall;
        BallTrigger.OnBallEnteredZone += BallEnteredZone;
        ButtonManager.OnRestartGame += StartAndResetGame;
    }
    private void OnDestroy()
    {
        ScoreManager.OnFallenPinsCountChange -= FallenPinsCountChange;
        PushBall.OnBallThrown -= ThrowBall;
        BallTrigger.OnBallEnteredZone -= BallEnteredZone;
        ButtonManager.OnRestartGame -= StartAndResetGame;
    }

    private void FallenPinsCountChange(int fallenPinsCount)
    {
        _fallenPinsCount += fallenPinsCount;
    }

    private void SpawnInStart()
    {
        if (_ballPrefab == null || _pinsPrefab == null)
        {
            Debug.Log("Не назначен префаб мяча/кеглей!");
            return;
        }
        else if (_ballPoint == null || _pinsPrefab == null)
        {
            Debug.Log("Не назначена точка спавна мяча/кеглей!");
            return;
        }
        else
        {
            _ballInGame = Instantiate(_ballPrefab, _ballPoint.transform.position, _pinsPrefab.transform.rotation);
            _pinsInGame = Instantiate(_pinsPrefab, _pinsPoint.transform.position, _ballPrefab.transform.rotation);
        }
    }

    private void AllPinsFallen()
    {
        int fallenPinsCount = _fallenPinsCount;

        UpdateScore(_fallenPinsCount);

        if (_rollsLeft == 3 )
        {
            if (fallenPinsCount == 0)
            {
                _result = Result.Miss;
                _rollsLeft = 2;
            }
            else
            if (fallenPinsCount > 0 && fallenPinsCount < _maxPins)
            {
                _result = Result.Open;
                _rollsLeft = 2;
            }
            else
            if (fallenPinsCount == _maxPins)
            {
                _result = Result.Strike;
                _rollsLeft = 2;
            }
            else
            if (fallenPinsCount > _maxPins)
            {
                Debug.Log("Ошибка в подсчёте очков за бросок");
            }

            RestartPush(fallenPinsCount);
        }
        else
        if (_rollsLeft == 2)
        {
            if (fallenPinsCount == _maxPins)
            {
                _rollsLeft = 1;
            }
            else
            if (_totalGameScore == _maxPins)
            {
                _result = Result.Spare;
                _rollsLeft = 1;
            }
            else
            if (_totalGameScore < _maxPins || _totalGameScore > _maxPins)
            {
                _result = Result.Open;
                EndGame();
                return;
            }
            else
            if (fallenPinsCount > _maxPins)
            {
                Debug.Log("Ошибка в подсчёте очков за бросок");
            }

            RestartPush(fallenPinsCount);
        }
        else
        if (_rollsLeft == 1)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        OnResetPins?.Invoke();

        _gameEnd = true;

        UpdateScoreUI(_totalGameScore);

        OnGameEnded?.Invoke(_totalGameScore);
        
        if (_ballInGame != null || _pinsInGame != null)
        {
            Destroy(_ballInGame);
            Destroy(_pinsInGame);
        }
    }

    private void RestartPush(int fallenPinsCount)
    {
        if (_ballInGame != null && _pinsInGame != null)
        {
            if(_result == Result.Strike || _result == Result.Spare)
            {
                if (_ballInGame != null)
                {
                    Destroy(_ballInGame);
                }
                if (_pinsInGame != null)
                {
                    Destroy(_pinsInGame);
                }
                _ballInGame = Instantiate(_ballPrefab, _ballPoint.transform.position, _pinsPrefab.transform.rotation);
                _pinsInGame = Instantiate(_pinsPrefab, _pinsPoint.transform.position, _ballPrefab.transform.rotation);

                _fallenPinsCount = 0;

                UpdateScoreUI(_totalGameScore);
            }
            else
            {
                ScoreManager[] pins = _pinsInGame.GetComponentsInChildren<ScoreManager>();

                foreach (ScoreManager pin in pins)
                {
                    if (pin.IsFell)
                    {
                        Destroy(pin.gameObject);
                    }
                    Vector3 currentRotation = pin.transform.eulerAngles;
                    currentRotation.y = 0;
                    currentRotation.x = 0;
                    currentRotation.z = 0;
                    pin.transform.rotation = Quaternion.Euler(currentRotation);
                }
                Destroy(_ballInGame);

                _fallenPinsCount = 0;

                UpdateScoreUI(_totalGameScore);

                _ballInGame = Instantiate(_ballPrefab, _ballPoint.transform.position, _pinsPrefab.transform.rotation);
            }

            
        }
    }

    private void StartAndResetGame()
    {
        OnGameRestarted?.Invoke();

        _isBallEntered = false;
        _totalGameScore = 0;
        _fallenPinsCount = 0;
        _rollsLeft = 3;
        _gameEnd = false;

        if (_waitCoroutine != null)
        {
            StopCoroutine(_waitCoroutine);
            _waitCoroutine = null;
        }

        if (_ballInGame != null)
        {
            Destroy(_ballInGame);
        }
        if (_pinsInGame != null)
        {
            Destroy(_pinsInGame);   
        }
        _ballInGame = Instantiate(_ballPrefab, _ballPoint.transform.position, _pinsPrefab.transform.rotation);
        _pinsInGame = Instantiate(_pinsPrefab, _pinsPoint.transform.position, _ballPrefab.transform.rotation);

        OnScoreChangeUI?.Invoke(0);
    }

    private void UpdateScore(int score)
    {
        _totalGameScore += score;
    }
    private void UpdateScoreUI(int score)
    {
        OnScoreChangeUI?.Invoke(_totalGameScore);
    }

    private void BallEnteredZone()
    {
        _isBallEntered = true;

        if (_waitCoroutine != null)
        {
            StopCoroutine( _waitCoroutine );
        }

        _waitCoroutine = StartCoroutine(WaitForPinsToFall());

    }

    private void ThrowBall()
    {
        _isBallEntered = false;

        if (_waitCoroutine != null)
        {
            StopCoroutine( _waitCoroutine );
            _waitCoroutine = null;
        }
    }

    private IEnumerator WaitForPinsToFall()
    {
        float timer = 0f;
        int lastCountFallenPin = -1;

        while (timer < _waitTime)
        {
            timer += Time.deltaTime;

            int currentCount = _fallenPinsCount;

            if (currentCount > lastCountFallenPin)
            {
                lastCountFallenPin = currentCount;
                timer = 0f;
            }

            yield return null;
        }
        _waitCoroutine = null;
        AllPinsFallen();
    }


}
