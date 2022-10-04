using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_rigidbody : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 20, ForceMode.Impulse);
    }

    float currentTime;
    public float offTime = 1;
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime>offTime)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
    }
}
