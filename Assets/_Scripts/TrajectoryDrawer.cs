using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryDrawer : MonoBehaviour
{
    [SerializeField, Tooltip(" время ")]
    private float _timeStep = 0.1f;

    [SerializeField, Tooltip(" линия ")]
    private LineRenderer _trajectoryLine;

    [Header("Настройка дуги")]

    [SerializeField, Tooltip("Кол-во точек дуги")]
    private int _trajectoryDotCount = 30;
    
    [SerializeField, Tooltip("Материал дуги")]
    private Material _lineMaterial;

    [SerializeField, Tooltip("Цвет дуги")]
    private Color _lineColor;

    private Vector3[] _trajectoryDots;

    private bool _isVisible = false;

    private void Start()
    {
        InitializeLineRender();
    }

    private void InitializeLineRender()
    {
        _trajectoryLine = GetComponent<LineRenderer>();

        _trajectoryLine.positionCount = _trajectoryDotCount;
        _trajectoryLine.startWidth = 0.1f;
        _trajectoryLine.endWidth = 0.05f;
        _trajectoryLine.material = _lineMaterial;
        _trajectoryLine.startColor = _lineColor;
        _trajectoryLine.endColor = _lineColor;
        _trajectoryLine.enabled = false;

        _trajectoryDots = new Vector3[_trajectoryDotCount];
    }

    public void DrawTrajectory(Vector3 startPosition, Vector3 startVelocity, float currentThrust, float minThrust, float maxThrust)
    {
        if(!_isVisible)
            return;

        for (int i = 0; i < _trajectoryDotCount; i++)
        {
            float time = i * _timeStep;
            Vector3 dot = startPosition + startVelocity * time + 0.5f * Physics.gravity * time * time;

            _trajectoryDots[i] = dot;
        }

        _trajectoryLine.SetPositions(_trajectoryDots);
    }

    public void ShowTrajectory(bool show) 
    {
        _isVisible = show;
        _trajectoryLine.enabled = show;
    }
}
