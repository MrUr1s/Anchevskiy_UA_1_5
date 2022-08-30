using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl_old : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float _speed_look = 60f;
    [SerializeField] float _speed_bullet = 100f;
    float angle = 0f;
    [SerializeField]
    bool isGround = true;


    void Update()
    {
        Move();
        Look();
        Fire();
        F_and_CTRL();
        Jumping();
    }
    void Jumping()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            if (isGround)
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


    void F_and_CTRL()
    {
        if (Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.LeftControl))
            Debug.Log("F_and_CTRL");
    }

    void Move()
    {
        Vector3 input = new Vector3(0,0,0);
        if (Input.GetKey(KeyCode.W))
        {
            input.y = 1;
        }
        else
        if (Input.GetKey(KeyCode.S))
        {
            input.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            input.x = -1;
        }
        else
        if (Input.GetKey(KeyCode.D))
        {
            input.x = 1;
        }
        if (input != Vector3.zero)
        {
            var _targetRotation = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            transform.position += targetDirection * _speed * Time.deltaTime;
        }
    }

    void Look()
    {
        var input = 0;
        if(Input.GetKey(KeyCode.Q))
        {
            input = -1;
        }
        else if(Input.GetKey(KeyCode.E))
        {
            input = 1;
        }
        if (input != 0f)
        {
            angle += input * _speed_look * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
    void Fire()
    {
        if(Input.GetMouseButtonDown(0))
            StartCoroutine(Shot());
    }
    IEnumerator Shot()
    {
        var bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        Vector3 targetDirection = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f) * Vector3.forward;
        while (true)
        {
            bullet.transform.position += targetDirection * Time.deltaTime * _speed_bullet;
            yield return 0;
        }
    }
}
