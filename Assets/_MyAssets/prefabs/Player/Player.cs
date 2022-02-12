using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("FP Values")]
    [SerializeField] float mouseSensitivity = 100f;
    private float _xRotation = 0f;
    private float _yRotation = 0f;
    [Header("Transforms")]
    [SerializeField] Transform[] playerCardsTransforms;
    [SerializeField] Transform cameraParentTransform;
    [SerializeField] Transform cameraTransform;

    private Vector2 _playerInputValue;
    private PlayerInput _playerInput;
    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }
    private void OnDisable()
    {
        _playerInput.Disable();
    }
    private void SetupPlayerInputs()
    {
        _playerInput.Gameplay.MouseMove.performed += OnMouseMoveUpdated;
        _playerInput.Gameplay.MouseMove.canceled += OnMouseMoveUpdated;
    }

    private void OnMouseMoveUpdated(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _playerInputValue = obj.ReadValue<Vector2>();
    }

    private void SelectCard(float playerInputValue)
    {
        //PlayerCardsTransforms[]
    }

    void Start()
    {
        SetupPlayerInputs();
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        UpdateCameraRotation();
    }


    private void UpdateCameraRotation()
    {
        float mouseX = _playerInputValue.x * mouseSensitivity * Time.deltaTime;
        float mouseY = _playerInputValue.y * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation,-90f,45f);
        cameraTransform.localRotation = Quaternion.Euler(_xRotation,0f,0f);

        _yRotation += mouseX;
        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);
        cameraParentTransform.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
    }

}
