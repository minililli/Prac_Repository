using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class En_Chest : MonoBehaviour
{
    Transform closeState;
    Transform openState;

    private void Awake()
    {
        closeState = transform.GetChild(0);
        openState = transform.GetChild(1);
    }
    private void Start()
    {
        Close();
    }
    public void Open()
    {
        closeState.gameObject.SetActive(false);
        for (int i = 0; i < openState.childCount; i++)
        {
            openState.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void Close()
    {
        closeState.gameObject.SetActive(true);
        for (int i = 0; i < openState.childCount; i++)
        {
            openState.GetChild(i).gameObject.SetActive(false);
        }
    }
}
