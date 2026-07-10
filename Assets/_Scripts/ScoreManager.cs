using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private bool _isFell = false;
    private int _fallenPinsCount;

    public static event Action <int> OnFallenPinsCountChange;

    public bool IsFell { get { return _isFell; } }

    //private void OnEnable()
    //{
    //    GameManager.OnResetPins += CountReset; 
    //    GameManager.OnGameRestarted += CountReset;
    //}

    //private void OnDestroy()
    //{
    //    GameManager.OnGameRestarted -= CountReset;
    //    GameManager.OnResetPins -= CountReset;
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 || other.gameObject.layer == 6)
        {   
            if (!_isFell)
            {
                _isFell = true;

                _fallenPinsCount++;
                OnFallenPinsCountChange?.Invoke(_fallenPinsCount);
            }
            
        }
    }

    public void SetNotFallen()
    {
        _isFell = false;
    }
}
