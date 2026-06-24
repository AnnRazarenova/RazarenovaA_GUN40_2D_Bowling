using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(TrajectoryDrawer))]
public class PushBall : MonoBehaviour
{
    [Header("Настройка силы броска шара")]

    [SerializeField, Tooltip("Минимальная сила броска")]
    private float minThrust = 5f;

    [SerializeField, Tooltip("Максимальная сила броска")]
    private float maxThrust = 20f;

    [SerializeField, Tooltip("Текущая сила броска")]
    private float currentThrust = 5f;

    [SerializeField, Tooltip("Скорость изменения силы броска")]
    private float thrustSpeedChange = 1f;
    
    private Rigidbody rb;
    private TrajectoryDrawer trajectoryDrawer;

    private bool isChanging = false;
    private bool increasing = true;

    private BallControls ballControls;
    public float CurrentThrust { get { return currentThrust; }}

    private void Awake()
    {
        ballControls = new BallControls();

        ballControls.Ball.ThrowBall.started += OnThrowBall;
        ballControls.Ball.ThrowBall.canceled += OnThrowBall;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        trajectoryDrawer = GetComponent<TrajectoryDrawer>();
    }

    private void FixedUpdate()
    {
        if (isChanging)
        {
            UpdateThrust();

            trajectoryDrawer.DrawTrajectory(transform.position, transform.forward*currentThrust, currentThrust, minThrust, maxThrust);
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

    private void StartChanging()
    {
        isChanging = true;
        currentThrust = minThrust;
        increasing = true;

        trajectoryDrawer.ShowTrajectory(true);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void UpdateThrust()
    {
        if (increasing)
        {
            currentThrust += thrustSpeedChange * Time.fixedDeltaTime;

            if (currentThrust >= maxThrust)
            {
                currentThrust = maxThrust;
                increasing = false;
            }
        }
        else
        {
            currentThrust -= thrustSpeedChange * Time.fixedDeltaTime;

            if (currentThrust < minThrust)
            {
                currentThrust = minThrust;
                increasing = true;
            }
        }
    }

    private void ThrowBall()
    {
        isChanging = false;

        rb.useGravity = true;

        trajectoryDrawer.ShowTrajectory(false);

        Vector3 direction = transform.forward;

        rb.AddForce(direction *  currentThrust, ForceMode.Impulse);
    }    
    
    private void OnEnable()
    {
        ballControls.Ball.Enable();
    }

    private void OnDisable()
    {
        ballControls.Ball.Disable();
    }
}
