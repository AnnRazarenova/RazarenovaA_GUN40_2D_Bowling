using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(TrajectoryDrawer))]
public class PushBall : MonoBehaviour
{
    public static event Action OnBallThrown;
    public static event Action<float> OnThrustChange;

    [Header("Настройка силы броска шара")]

    [SerializeField, Tooltip("Минимальная сила броска")]
    private float _minThrust = 20f;

    [SerializeField, Tooltip("Максимальная сила броска")]
    private float _maxThrust = 40f;

    [SerializeField, Tooltip("Текущая сила броска")]
    private float _currentThrust = 20f;

    [SerializeField, Tooltip("Скорость изменения силы броска")]
    private float _thrustSpeedChange = 7f;


    [Header("Настройка поворота")]

    [SerializeField]
    private float _rotationSpeed = 3f;

    [SerializeField]
    private float _minRotationAngle = -45f;

    [SerializeField]
    private float _maxRotationAngle = 45f;    

    private Rigidbody _rb;
    private TrajectoryDrawer _trajectoryDrawer;

    private BallControls _ballControls;

    private bool _isChanging = false;
    private bool _increasing = true;

    private float _currentRotationY = 0f;
    private float _mausePositionX = 0f;
    private Vector3 _startForward;


    public static PushBall PushBallInstance { get; private set; }

    public float CurrentThrust { get { return _currentThrust; }}

    private void Awake()
    {
        _ballControls = new BallControls();

        _ballControls.Ball.ThrowBall.started += OnThrowBall;
        _ballControls.Ball.ThrowBall.canceled += OnThrowBall;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _trajectoryDrawer = GetComponent<TrajectoryDrawer>();

        _startForward = transform.forward;
    }

    private void FixedUpdate()
    {
        if (_isChanging)
        {
            UpdateThrust();

            RotateBall();

            _trajectoryDrawer.DrawTrajectory(transform.position, transform.forward* _currentThrust, _currentThrust, _minThrust, _maxThrust);
        }
    }

    public void OnThrowBall(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartChanging();
        }
        else if (context.canceled)
        {
            ThrowBall();
        }
    }

    private  void RotateBall()
    {
        _mausePositionX = Input.GetAxis("Mouse X");

        _currentRotationY += _mausePositionX * _rotationSpeed;

        _currentRotationY = Mathf.Clamp(_currentRotationY, _minRotationAngle, _maxRotationAngle);

        Quaternion rotation = Quaternion.Euler(0, _currentRotationY, 0);
        transform.rotation = Quaternion.LookRotation(rotation * _startForward, Vector3.up);
    }

    private void StartChanging()
    {
        _isChanging = true;
        _currentThrust = _minThrust;
        _increasing = true;

        _trajectoryDrawer.ShowTrajectory(true);

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    private void UpdateThrust()
    {
        float oldThrust = _currentThrust;

        if (_increasing)
        {
            _currentThrust += _thrustSpeedChange * Time.fixedDeltaTime;

            if (_currentThrust >= _maxThrust)
            {
                _currentThrust = _maxThrust;
                _increasing = false;
            }

            if (Mathf.Abs(_currentThrust - oldThrust) > 0.001f)
            {
                OnThrustChange?.Invoke(_currentThrust);
            }
        }
        else
        {
            _currentThrust -= _thrustSpeedChange * Time.fixedDeltaTime;

            if (_currentThrust < _minThrust)
            {
                _currentThrust = _minThrust;
                _increasing = true;
            }
            if (Mathf.Abs(_currentThrust - oldThrust) > 0.001f )
            {
                OnThrustChange?.Invoke(_currentThrust);
            }
        }

        
    }

    private void ThrowBall()
    {
        _isChanging = false;

        _rb.useGravity = true;

        _trajectoryDrawer.ShowTrajectory(false);

        Vector3 direction = transform.forward;

        _rb.AddForce(direction * _currentThrust, ForceMode.Impulse);
        OnBallThrown?.Invoke();
    }    
    
    private void OnEnable()
    {
        _ballControls.Ball.Enable();
    }

    private void OnDisable()
    {
        _ballControls.Ball.ThrowBall.started -= OnThrowBall;
        _ballControls.Ball.ThrowBall.canceled -= OnThrowBall;
        _ballControls.Ball.Disable();
    }
}
