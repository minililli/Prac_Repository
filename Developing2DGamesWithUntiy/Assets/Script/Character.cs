using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터가 공통으로 지녀야 하는 기능과 속성을 담은 클래스
public abstract class Character : MonoBehaviour
{
    //C# abstract 한정자를 사용해 인스턴스화 할 수 없는 클래스고 하위 클래스에서 상속해야하는 클래스

    /// <summary>
    /// 현재 체력 값 설정
    /// </summary>
    public HitPoints hitPoints;
    /// <summary>
    /// 최대체력 설정
    /// </summary>
    public float maxHitPoints;
    /// <summary>
    /// 최초체력 설정
    /// </summary>
    public float startingHitPoints;
}
