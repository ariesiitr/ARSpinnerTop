using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{

    private Rigidbody rb;
    public bool doSpin = false;
    public float spinSpeed = 3600f;
    public GameObject playerGraphics;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (doSpin)
        { playerGraphics.transform.Rotate(new Vector3(0 , spinSpeed *Time.deltaTime , 0)); }
    }
}
