using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameManager Instance;
    MapManager mapManager;
    public Player player;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(Instance);
        else Instance = this;
    }
    private void OnEnable()
    {
        if (player != null)
        {
            player = FindObjectOfType<Player>();
        }
    }
}
