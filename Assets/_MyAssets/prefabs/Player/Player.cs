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
    
    [Header("Camera")]
    [SerializeField] Transform cameraGlobalTransform;
    [SerializeField] Transform cameraParentTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform tableCameraTransform;
    [SerializeField] float cameraTransSpeed = 1f;
    private Vector3 _ogCameraGlobalPos;
    private Quaternion _ogCameraGlobalRot;

    private Card _highlightedCard;
    private bool _isCardSelected;
    private Vector3 _rayCastHitPos;

    private bool _isLeftMouseButtonHeld;
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
        _playerInput.Gameplay.LeftButton.performed += OnLeftClickHeldUpdated;
        _playerInput.Gameplay.LeftButton.canceled += OnLeftClickHeldUpdated;
    }

    private void OnLeftClickHeldUpdated(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        float buttonHeld = obj.ReadValue<float>();
        if(buttonHeld == 1)
        {
            _isLeftMouseButtonHeld = true;
        }
        else
        {
            _isLeftMouseButtonHeld = false;
        }
        
    }

    private void OnMouseMoveUpdated(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _playerInputValue = obj.ReadValue<Vector2>();
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 2f))
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * hit.distance, Color.blue);
        }
    }

    void Start()
    {
        _ogCameraGlobalPos = cameraGlobalTransform.position;
        _ogCameraGlobalRot = cameraGlobalTransform.rotation;
        _isCardSelected = false;

        SetupPlayerInputs();
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        UpdateCameraRotation();

        CardSelected();

        if (!_isCardSelected)
        {
            HighLightCard();
        }
    }

    private void CardSelected()
    {
        if (_isLeftMouseButtonHeld && _highlightedCard != null)
        {
            _isCardSelected = true;
            _highlightedCard.SetIsHighlighted(false);

            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 10f, LayerMask.GetMask("PlayerArea")))
            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * hit.distance, Color.red);
                _highlightedCard.transform.position = hit.point + new Vector3(0, .1f, 0);

                //not done need to fix rotation
                Quaternion rotToLerp = Quaternion.FromToRotation(transform.right, hit.normal);
                _highlightedCard.SetDesiredRotation(rotToLerp);
            }


            //CAMERA
            cameraGlobalTransform.position = Vector3.Lerp(cameraGlobalTransform.position, tableCameraTransform.position, cameraTransSpeed * Time.deltaTime);
            cameraGlobalTransform.rotation = Quaternion.Lerp(cameraGlobalTransform.rotation, tableCameraTransform.rotation, cameraTransSpeed * Time.deltaTime);

        }
        else
        {
            _isCardSelected = false;

            if (_highlightedCard != null)
            {
                _highlightedCard.SetToOGTransform();
            }

            //CAMERA
            cameraGlobalTransform.position = Vector3.Lerp(cameraGlobalTransform.position, _ogCameraGlobalPos, cameraTransSpeed * Time.deltaTime);
            cameraGlobalTransform.rotation = Quaternion.Lerp(cameraGlobalTransform.rotation, _ogCameraGlobalRot, cameraTransSpeed * Time.deltaTime);
        }
    }

    private void UpdateCameraRotation()
    {
        float mouseX = _playerInputValue.x * mouseSensitivity * Time.deltaTime;
        float mouseY = _playerInputValue.y * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 45f);
        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        _yRotation += mouseX;
        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);
        cameraParentTransform.localRotation = Quaternion.Euler(0f, _yRotation, 0f);

    }

    private void HighLightCard()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, .5f,LayerMask.GetMask("Card")))
        {
            if (_highlightedCard != null)
            {
                _highlightedCard.SetIsHighlighted(false);
            }

            _highlightedCard = hit.collider.gameObject.GetComponent<Card>();

            _highlightedCard.SetIsHighlighted(true);

            _rayCastHitPos = hit.point;
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * hit.distance, Color.blue);
        }
        else
        {
            if (_highlightedCard != null)
                _highlightedCard.SetIsHighlighted(false);

            _highlightedCard = null;
        }
    }
}
