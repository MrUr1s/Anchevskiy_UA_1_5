using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl_new : MonoBehaviour
{
    private PlayerControls _control;
    [SerializeField] float _speed = 5f;
    [SerializeField] float _speed_look = 60f;
    [SerializeField] float _speed_bullet = 100f;
    float angle = 0f;
    [SerializeField]
    bool isGround=true;

    void Awake()
    {
        _control = new PlayerControls();
    }

    void OnEnable()
    {
        _control.PlayerControl.Enable();
        _control.PlayerControl.F_and_CTRL.performed += F_and_CTRL_performed;
        _control.PlayerControl.Fire.performed += Fire_performed;
        _control.PlayerControl.Jump.performed += Jump_performed;
    }
    void onDisable()
    {
        _control.PlayerControl.Disable();
        _control.PlayerControl.F_and_CTRL.performed -= F_and_CTRL_performed;
        _control.PlayerControl.Fire.performed -= Fire_performed;
        _control.PlayerControl.Jump.performed -= Jump_performed;
    }


    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(isGround)
        StartCoroutine(Jump());
    }

    IEnumerator Jump()
    {
        var t = 1;
        isGround = false;
        do
        {
            if (transform.position.y >= 1f)
            {
                t = -1;
            }
            transform.position += t * Vector3.MoveTowards(Vector3.zero, Vector3.up, Time.deltaTime);
            yield return 0;
        } 
        while (transform.position.y >= 0);
        isGround = true;
    }

    private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StartCoroutine(Shot());
    }

    IEnumerator Shot()
    {
        var bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.transform.position=transform.position;
        bullet.transform.rotation=transform.rotation;
        Vector3 targetDirection = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f) * Vector3.forward;
        while (true)
        {
            bullet.transform.position += targetDirection*Time.deltaTime* _speed_bullet;
            yield return 0;
        }
    }

    private void F_and_CTRL_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("F_and_CTRL");
    }

    void Update()
    {
        Move();
        Look();
    }

    private void Look()
    {
        var input = _control.PlayerControl.Look.ReadValue<float>();
        if (input != 0f)
        {
            angle += input * _speed_look * Time.deltaTime;
            transform.rotation=Quaternion.Euler(0, angle, 0);
        }
    }

    private void Move()
    {
        var input = _control.PlayerControl.Move.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            var _targetRotation = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            transform.position += targetDirection * _speed*Time.deltaTime;
        }
    }


}
