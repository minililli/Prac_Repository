using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Runner : MonoBehaviour
{
    CapsuleCollider capsule;
    Animator anim;

    public float invincibleTime=2.0f;

    WaitForSeconds ivInterval;
    public bool onInvincible = false;
    public float maxHp = 100;
    public float currenthp;

    private void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
        anim= GetComponent<Animator>(); 
        ivInterval = new WaitForSeconds(invincibleTime);
    }

    private void Start()
    {
        currenthp = maxHp;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")&& !onInvincible)
        {
            Debug.Log("아야");
            OnInvincibleMode();
            transform.forward = Vector3.forward;
            GetComponent<Rigidbody>().AddForce(0, 0, 15f,ForceMode.Impulse);
            currenthp -= 0.2f * maxHp;
        }
       
    }

    void OnInvincibleMode()
    {
        onInvincible = true;
        StartCoroutine(OnInvincible());
    }

    IEnumerator OnInvincible()
    {
        while (onInvincible)
        {
            //capsule.enabled = false;

            this.gameObject.layer = 8;
            anim.SetBool("Invincible", true);
            anim.SetFloat("Speed", 0.1f);
            yield return ivInterval;

            onInvincible = false;
        }
        //Debug.Log("끝");
        anim.SetBool("Invincible", false);
        anim.SetFloat("Speed", 1);
        this.gameObject.layer = 0;

        StopCoroutine(OnInvincible());
    }
}
