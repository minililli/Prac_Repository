using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    Runner runner;
    public float pushForce = 500f;
   // BoxCollider[] colliders;
    private void Awake()
    {
        runner = FindObjectOfType<Runner>();
        //colliders = GetComponentsInChildren<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            runner.GetComponent<Rigidbody>().AddForce(pushForce, 0, 0, ForceMode.Impulse);
        }
    }
}
