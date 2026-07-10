using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CenterOfMassController : MonoBehaviour
{
    [Header("Настройки центра масс")]
    [SerializeField]
    private Vector3 _customCenterOfMass = new Vector3(0, -0.3f, 0);

    [SerializeField]
    private float _size = 0.02f;

    [SerializeField] 
    private bool _applyOnStart = true;

    [SerializeField] 
    private bool _applyInEditor = true;

    private Rigidbody _rb;

    private void Start()
    {
        if (_applyOnStart)
        {
            ApplyCenterOfMass();
        }
    }

    private void ApplyCenterOfMass()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb != null)
        {
            _rb.centerOfMass = _customCenterOfMass;
            Debug.Log($"Center of Mass установлен: {_customCenterOfMass}");
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_applyInEditor)
        {
            _rb = GetComponent<Rigidbody>();
            if (_rb != null)
            {
                _rb.centerOfMass = _customCenterOfMass;
            }
        }
    }

    private void OnDrawGizmos()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) return;

        Vector3 worldCenter = transform.TransformPoint(_rb.centerOfMass);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(worldCenter, _size);

        Gizmos.color = Color.yellow;
    }
#endif
}