using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Character
{
    float hitPoints;

    /// <summary>
    /// 플레이어와 충돌했을 때 적이 입힐 피해량
    /// </summary>
    public int damageStrength;
    Coroutine damageCoroutine;

    private void OnEnable()
    {
        ResetCharacter();
    }
    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while(true)
        {
            hitPoints = hitPoints - damage;
            if (hitPoints <= float.Epsilon)
            {
                KillCharacter();
                break;
            }
            if(interval > float.Epsilon)
            {
                yield  return new WaitForSeconds(interval);
            }
            else //interval < float.Epsilon ==> interval = 0 이면 damage 한번만 입힘.
            {
                break;
            }
        }
    }

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if(damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 1.0f));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }
}
