using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    Runner runner;
    private void Awake()
    {
        runner = FindObjectOfType<Runner>();
    }

    private void Start()
    {
        this.transform.position = new Vector3(-21, 5, -37f);
    }
    private void Update()
    {
        this.transform.position = new Vector3(-21, 5, runner.transform.position.z);
    }

}
