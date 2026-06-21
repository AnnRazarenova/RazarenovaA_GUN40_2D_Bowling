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
    //[SerializeField, Tooltip("Префаб для точек")]
    //private GameObject dotPrefab;
    
    [SerializeField, Tooltip("Кол-во точек дуги")]
    private int trajectoryDotCount = 30;
    [SerializeField, Tooltip("Расстояние между точками")]
    private float dotSpacing = 0.5f;
    
    [SerializeField, Tooltip("Материал дуги")]
    private Material lineMaterial;
    [SerializeField, Tooltip("Цвет дуги")]
    private Color lineColor;

    private Vector3[] trajectoryDots;
    private List<GameObject> dots = new List<GameObject>();

    private bool isVisible = false;

    private void Start()
    {
        InitializeLineRender();
        //InitializeDots();
    }

    private void InitializeLineRender()
    {
        trajectoryLine = GetComponent<LineRenderer>();

        trajectoryLine.positionCount = trajectoryDotCount;
        trajectoryLine.startWidth = 0.1f;
        trajectoryLine.endWidth = 0.05f;
        trajectoryLine.material = lineMaterial;
        trajectoryLine.startColor = lineColor;
        trajectoryLine.endColor = lineColor;
        trajectoryLine.enabled = false;

        trajectoryDots = new Vector3[trajectoryDotCount];
    }

    //private void InitializeDots()
    //{
    //    if (!showDots || dotPrefab == null)
    //        return;

    //    for (int i = 0; i < trajectoryDotCount; i++)
    //    {
    //        GameObject dot = Instantiate(dotPrefab, transform);
    //        dot.SetActive(false);
    //        dots.Add(dot);
    //    }
    //}

    public void DrawTrajectory(Vector3 startPosition, Vector3 startVelocity, float currentThrust, float minThrust, float maxThrust)
    {
        if(!isVisible)
            return;

        //int currentDotCount = trajectoryDotCount;

        for (int i = 0; i < trajectoryDotCount; i++)
        {
            float time = i * timeStep;
            Vector3 dot = startPosition + startVelocity * time + 0.5f * Physics.gravity * time * time;

            //RaycastHit hit;
            //if (Physics.Raycast(startPosition, dot - startPosition, out hit, Vector3.Distance(startPosition, dot)))
            //{
            //    trajectoryDots[i] = hit.point;
            //    currentDotCount = +1;
            //    break;
            //}

            trajectoryDots[i] = dot;
        }

        trajectoryLine.SetPositions(trajectoryDots);
        //UpdateDots(trajectoryDotCount);
    }

    //private void UpdateTrajectory(int dotCount)
    //{
    //    trajectoryLine.positionCount = dotCount;
    //    trajectoryLine.SetPositions(trajectoryDots);
    //}

    //private void UpdateDots(int dotCount)
    //{
    //    if (!showDots || dots.Count == 0) 
    //        return;

    //    for (int i = 0; i < dots.Count;i++)
    //    {
    //        if (i<do)
    //    }
    //}

    public void ShowTrajectory(bool show) 
    {
        isVisible = show;
        trajectoryLine.enabled = show;
    }
}
