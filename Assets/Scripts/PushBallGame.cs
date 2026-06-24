using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RestartPushBall : MonoBehaviour
{
    [SerializeField]
    private GameObject ballInGame;

    [SerializeField]
    private GameObject pinsInGame;

    [SerializeField]
    private GameObject ballPrefab;

    [SerializeField]
    private GameObject pinsPrefab;

    [SerializeField]
    private Vector3 ballPoint;

    [SerializeField]
    private Vector3 pinsPoint;

    private void RestartGame()
    {
        if (ballInGame != null && pinsInGame != null)
        {
            Destroy(ballInGame);
            Destroy(pinsInGame);

            ballInGame = Instantiate(ballPrefab, ballPoint, pinsPrefab.transform.rotation);
            pinsInGame = Instantiate(pinsPrefab, pinsPoint, ballPrefab.transform.rotation);
        }
    }
}
