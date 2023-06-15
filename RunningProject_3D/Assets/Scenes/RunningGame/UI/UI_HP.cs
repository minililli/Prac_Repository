using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HP : MonoBehaviour
{
    Slider slider;
    Runner runner;
    private void Awake()
    {
        runner = FindObjectOfType<Runner>();
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        slider.value = runner.currenthp / runner.maxHp;
    }

    private void Update()
    {
        slider.value = runner.currenthp / runner.maxHp;
      
    }
}
