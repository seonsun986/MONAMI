using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5;
    public float gravity = -9.81f;
    float yVelocity;
    CharacterController cc;
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        yVelocity += gravity * Time.deltaTime;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = v * transform.up + h * transform.right;
        dir.Normalize();
        dir.y = yVelocity;
        cc.Move(dir * speed * Time.deltaTime);
    }
}
