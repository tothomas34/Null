using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent (typeof(Rigidbody))]
public class ControlerTopDown : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private Rigidbody _rb;


    private Vector3 _inputVector;

    // Initialisation interne + Création d'objets si prévu
    private void Awake()
    {
        if(_rb == null) { 
        _rb = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        _inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {

        // _rb.MovePosition(_rb.position + _inputVector);

        // _inputVector.Normalize();
        Vector3 boundedInputVector = Vector3.ClampMagnitude(_inputVector, 1f);

        _rb.linearVelocity = transform.TransformDirection(boundedInputVector)  * _speed;

         

    }
}
