using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryDrawer : MonoBehaviour
{
    [SerializeField, Tooltip(" время ")]
    private float timeStep = 0.1f;

    [SerializeField, Tooltip(" линия ")]
    private LineRenderer trajectoryLine;

    [Header("Настройка дуги")]

    [SerializeField, Tooltip("Кол-во точек дуги")]
    private int trajectoryDotCount = 30;
    
    [SerializeField, Tooltip("Материал дуги")]
    private Material lineMaterial;

    [SerializeField, Tooltip("Цвет дуги")]
    private Color lineColor;

    private Vector3[] trajectoryDots;

    private bool isVisible = false;

    private void Start()
    {
        InitializeLineRender();
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

    public void DrawTrajectory(Vector3 startPosition, Vector3 startVelocity, float currentThrust, float minThrust, float maxThrust)
    {
        if(!isVisible)
            return;

        for (int i = 0; i < trajectoryDotCount; i++)
        {
            float time = i * timeStep;
            Vector3 dot = startPosition + startVelocity * time + 0.5f * Physics.gravity * time * time;

            trajectoryDots[i] = dot;
        }

        trajectoryLine.SetPositions(trajectoryDots);
    }

    public void ShowTrajectory(bool show) 
    {
        isVisible = show;
        trajectoryLine.enabled = show;
    }
}
