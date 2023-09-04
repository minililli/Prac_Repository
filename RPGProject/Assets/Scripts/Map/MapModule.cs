using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModule : MonoBehaviour
{
    public Vector3 GetModuleCenter(Vector3 modPos, int modLength)
    {
        Vector3 changePos = new Vector3();
        changePos.x = modPos.x + modLength * 0.5f;
        changePos.y = modPos.y;
        changePos.z = modPos.z + modLength * 0.5f;
        return changePos;
    }
}
