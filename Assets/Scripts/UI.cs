using UnityEngine;
using TMPro;
using System;
using UnityEngine.InputSystem.Controls;

public class UI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text currentThrust;

    [SerializeField]
    private PushBall pushBall;

    [SerializeField]
    private ButtonControl pushBallButton; 

    private void FixedUpdate()
    {
        ChangeShowenThrust(pushBall.CurrentThrust);
    }

    private void ChangeShowenThrust(float thrust)
    {
        currentThrust.text = Math.Round(thrust).ToString();
    }

    

}
