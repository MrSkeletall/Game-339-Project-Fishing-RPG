using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private InputAction moveAction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        // Ensure gravity is disabled for top-down 2D
        _rb.gravityScale = 0f;
        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        // Capture input in Update
       
        _moveInput = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Apply movement in FixedUpdate for physics consistency
        _rb.MovePosition(_rb.position + ((_moveInput * moveSpeed) * Time.fixedDeltaTime));
    }
}
