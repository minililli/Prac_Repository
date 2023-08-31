using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Environment : Test_Base
{

    En_Chest chest;
    private void Start()
    {
        chest = FindObjectOfType<En_Chest>();
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        chest.Open();
    }
    protected override void Test2(InputAction.CallbackContext _)
    {
        chest.Close();
    }
}
