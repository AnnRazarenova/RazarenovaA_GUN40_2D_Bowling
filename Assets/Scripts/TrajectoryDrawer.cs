using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryDrawer : MonoBehaviour
{
    [SerializeField, Tooltip(" время ")]
    private float timeStep = 0.1f;
    [SerializeField, Tooltip(" линия ")]
    private LineRenderer trajectoryLine;

    [Header("Настройка дуги")]
    [SerializeField, Tooltip("Точки дуги видны")]
    private bool showDots = true;
    [SerializeField, Tooltip("Префаб для точек")]
    private GameObject dotPrefab;
    
    [SerializeField, Tooltip("Кол-во точек дуги")]
    private int trajectoryPointCount = 30;
    [SerializeField, Tooltip("Расстояние между точками")]
    private float dotSpacing = 0.5f;
    
    [SerializeField, Tooltip("Материал дуги")]
    private Material lineMaterial;
    [SerializeField, Tooltip("Цвет дуги")]
    private Color lineColor;

    private Vector3[] trajectoryPositions;
    private List<GameObject> dots = new List<GameObject>();

    private bool isVisible = false;

    private void Start()
    {
        InitializeLineRender();
        InitializeDots();
    }

    private void InitializeLineRender()
    {
        trajectoryLine = GetComponent<LineRenderer>();

        trajectoryLine.positionCount = trajectoryPointCount;
        trajectoryLine.startWidth = 0.1f;
        trajectoryLine.endWidth = 0.05f;
        trajectoryLine.material = lineMaterial;
        trajectoryLine.startColor = lineColor;
        trajectoryLine.endColor = lineColor;
        trajectoryLine.enabled = false;

        trajectoryPositions = new Vector3[trajectoryPointCount];
    }

    private void InitializeDots()
    {
        if (!showDots || dotPrefab == null)
            return;

        for (int i = 0; i < trajectoryPointCount; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
            dot.SetActive(false);
            dots.Add(dot);
        }
    }

    public void DrawTrajectory(Vector3 startPosition, Vector3 startVelocity, float currentThrust, float minThrust, float maxThrust)
    {

    }

    public void ShowTrajectory() { }

}
