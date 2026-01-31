using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _turnTime;

    [SerializeField] float _speed;
    [SerializeField] float _strafe;
    [SerializeField] float _accleration;
    [SerializeField] float _deccleration;

    [Header("Gravity")]
    [SerializeField] public float _gravityMultiplier;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckHeight = 0.3f;
    public bool isGrounded;
    float velocity;


    [Header("Stats")]
    public Vector3 CharVelocity;
    public float CharVelocityf;



    [Header("Referances")]
    [SerializeField] CharacterController _characterController;
    [SerializeField] Animator _animatorController;

    private float _currentSpeed;
    private float _turnSpeed;

    private void Awake()
    {
    }

    void Start()
    {
        groundCheck = transform;
    }


    void Update()
    {
        GroundCheck();
        Gravity();
    }

    private void LateUpdate()
    {
        CharVelocity = _characterController.velocity;
        CharVelocityf = CharVelocity.magnitude;
    }

    Vector3 _LastDirection = Vector3.zero;
    Vector2 _LastMoveInput = Vector2.zero;
    Vector3 oldDir = Vector3.zero;
    public void Move(Vector2 moveInput, Vector2 lookInput)
    {
        Vector3 dir;
        if (lookInput.magnitude > 0f && moveInput.magnitude > 0f)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, _strafe * moveInput.magnitude, Time.deltaTime * _accleration);
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

            float targetAngle = Mathf.Atan2(lookInput.x, lookInput.y) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSpeed, _turnTime * Time.timeScale);
            transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);

            _LastMoveInput = Vector2.Lerp(_LastMoveInput, moveInput, Time.deltaTime);
            _characterController.Move(_currentSpeed * Time.deltaTime * moveDirection);
            dir = transform.InverseTransformDirection(moveDirection);


        }
        else if (moveInput.magnitude > 0f)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, _speed * moveInput.magnitude, Time.deltaTime * _accleration);

            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSpeed, _turnTime * Time.timeScale);
            transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);
            Vector3 moveDirection = Quaternion.Euler(0, smoothedAngle, 0) * Vector3.forward;
            _LastDirection = moveDirection;

            _characterController.Move(_currentSpeed * Time.deltaTime * moveDirection);
            _animatorController.SetFloat("Speed", _currentSpeed / _speed);
            dir = transform.InverseTransformDirection(moveDirection);


        }
        else
        {
            if (lookInput.magnitude > 0f)
            {
                float targetAngle = Mathf.Atan2(lookInput.x, lookInput.y) * Mathf.Rad2Deg;
                float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSpeed, _turnTime * Time.timeScale);
                transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);

            }
            if (_currentSpeed < 0.01f)
            {
                _currentSpeed = 0;
            }
            else
            {
                _currentSpeed = Mathf.Lerp(_currentSpeed, 0, Time.deltaTime * _deccleration);
                _characterController.Move(_currentSpeed * Time.deltaTime * _LastDirection);
            }
            _animatorController.SetFloat("Speed", _currentSpeed / _speed);
            dir = transform.InverseTransformDirection(Vector3.zero);

        }
        oldDir = Vector3.Lerp(oldDir, dir, 3 * Time.deltaTime);
        _animatorController.SetFloat("MoveX", oldDir.x * (_currentSpeed / _speed));
        _animatorController.SetFloat("MoveY", _currentSpeed / _speed * dir.z);
    }

    void Gravity()
    {
        if (isGrounded && velocity < 0)
        {
            velocity = -5f;
        }
        else if (!isGrounded && velocity < 0)
        {
            velocity += _gravity * Time.deltaTime * _gravityMultiplier;
        }
        else if (!isGrounded && velocity > -5f)
        {
            velocity += _gravity * Time.deltaTime * _gravityMultiplier;
        }

        _characterController.Move(Time.unscaledDeltaTime * velocity * Vector3.up);
    }


    void GroundCheck()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckHeight, groundLayer);
    }

}