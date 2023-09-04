using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    //module의 갯수
    public GameObject[] modules;

    //map구성 module 의 수
    public int module = 3;

    //map의 크기(전체 모듈의 갯수, module*module)
    int mapSize;
    int[,] map;
    


    private void Awake()
    {
        mapSize = module * module; //모듈의 총 갯수
        map = new int[module, module];
        modules = new GameObject[6];
    }

    private void OnEnable()
    {
        ResetMapStartPoint(map);
    }


    /// <summary>
    /// map 원점을 0,0으로 만들기 위해 좌측하단으로 고정
    /// 0 1 2 >> 6 7 8
    /// 3 4 5 >> 3 4 5
    /// 5 7 8 >> 0 1 2 
    /// </summary>
    /// <param name="map"></param>
    private void ResetMapStartPoint(int[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = (module * (module - 1)) - (module * i) + j; // 3*2 - 3*i + j = 6 - 3i + j
            }
        }
    }

    private void Search(int[,] map, int num)
    {
        for(int i=0; i<map.GetLength(0); i++)
        {
            for(int j=0;j<map.GetLength(1);j++)
            {
                if (map[i, j] == num)
                {
                    int x = (module * (module - 1)) - (module * i);
                    int y = j;
                    Instantiate(x, y);
                }
            }
        }
    }

    public void SetModuleToMap(GameObject[] modules)
    {

        
    }

    public GameObject Instantiate(int x, int y)
    {
        int r = Random.Range(0, modules.Length - 1);
        GameObject obj = GameObject.Instantiate(modules[r]);
        obj.transform.position = new Vector3(x, 0, y);

        return obj;
    }

}
