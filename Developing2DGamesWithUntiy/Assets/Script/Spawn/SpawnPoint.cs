using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    /// <summary>
    /// 소환할 프리팹
    /// </summary>
    public GameObject prefabToSpawn;
    /// <summary>
    /// 소환 간격 초
    /// </summary>
    public float repeatInterval;
    public void Start()
    {
        if (repeatInterval > 0)
        {
            InvokeRepeating("SpawnObject", 0.0f, repeatInterval);
        }
    }
    /// <summary>
    /// 게임오브젝트 생성하는 함수
    /// </summary>
    /// <returns></returns>
    public GameObject SpawnObject()
    {
        if(prefabToSpawn != null)
        {
            return Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }

        return null;
    }
}
