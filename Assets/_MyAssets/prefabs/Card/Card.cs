using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] float posSpeed;
    [SerializeField] float YOffset;
    private Vector3 _desiredPos;
    private Vector3 _transformYOffest;
    private Vector3 _ogPos;

    [Header("Rotation")]
    private Quaternion _desiredRot;
    private Quaternion _ogRotation;

    [Header("Scale")]
    [SerializeField] float scaleSpeed;
    private Vector3 _desiredScale;
    private Vector3 _ogScale;


    private bool _isHighlighted;

    public void SetToOGTransform()
    {
        if(!_isHighlighted)
        {
            _desiredPos = _ogPos;
            _desiredScale = _ogScale;
            _desiredRot = _ogRotation;
        }
    }

    public void SetDesiredRotation(Quaternion newRot)
    {
        _desiredRot = newRot;
    }

    public void SetIsHighlighted(bool isHightlighted)
    {
        _isHighlighted = isHightlighted;

        if (_isHighlighted)
        {
            _desiredPos = _transformYOffest;
            _desiredScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else
        {
            _desiredPos = _ogPos;
            _desiredScale = _ogScale;
        }
    }

    private void Start()
    {
        _ogPos = transform.position;
        _transformYOffest = _ogPos + new Vector3(0,YOffset,0);
        _ogScale = transform.localScale;
        _ogRotation = transform.rotation;

        _desiredPos = _ogPos;
        _desiredScale = _ogScale;
    }

    private void Update()
    {
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, _desiredScale, scaleSpeed * Time.deltaTime);

        this.transform.position = Vector3.Lerp(this.transform.position, _desiredPos, posSpeed * Time.deltaTime);

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _desiredRot, posSpeed * Time.deltaTime);
    }
}
