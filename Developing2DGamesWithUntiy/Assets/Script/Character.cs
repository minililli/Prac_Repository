using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터가 공통으로 지녀야 하는 기능과 속성을 담은 클래스
public abstract class Character : MonoBehaviour
{
    /// <summary>
    /// 캐릭터를 다시 사용할 수 있게 원래 시작 상태로 되돌린다.
    /// </summary>
    public abstract void ResetCharacter();
    /// <summary>
    /// 현재 캐릭터에게 피해를 주려고 다른 캐릭터가 호출하는 메서드
    /// </summary>
    /// <param name="damage">피해량</param>
    /// <param name="interval"> 피해 간격</param>
    /// <returns></returns>
    public abstract IEnumerator DamageCharacter(int damage, float interval);
  
    /// <summary>
    /// 최대체력 설정
    /// </summary>
    public float maxHitPoints;
    /// <summary>
    /// 최초체력 설정
    /// </summary>
    public float startingHitPoints;

    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }
}
