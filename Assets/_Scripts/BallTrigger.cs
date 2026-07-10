using System;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    public static event Action OnBallEnteredZone;

    private bool _isBallentered = false;

    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.layer == 8)
        {
            _isBallentered = true;

            OnBallEnteredZone?.Invoke();
        }
    }
}
