using System;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static event Action OnRestartGame;

    public void RestartGame()
    {
        OnRestartGame?.Invoke();
    }
}
