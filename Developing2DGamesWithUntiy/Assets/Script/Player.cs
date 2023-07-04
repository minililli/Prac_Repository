using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public HealthBar healthBarPrefab;
    HealthBar healthBar;

    public Inventory inventoryPrefab;
    Inventory inventory;

    /// <summary>
    /// 현재 체력 값 설정
    /// </summary>
    public HitPoints hitPoints;

    public float moveSpeed = 3.0f;
    Vector2 input = new Vector2();

    Rigidbody2D rigid;

    Animator anim;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ResetCharacter();
    }

    private void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    void OnMove()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input.Normalize();

        rigid.velocity = moveSpeed * input;
    }

    void UpdateState()
    {
        if(Mathf.Approximately(input.x,0) && Mathf.Approximately(input.y,0))
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }

        anim.SetFloat("DirX", input.x);
        anim.SetFloat("DirY", input.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            if (hitObject != null)
            {
                bool shouldDisappear = false;

                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        shouldDisappear = inventory.AddItem(hitObject);
                        break;
                    case Item.ItemType.HEALTH:
                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;
                    case Item.ItemType.FOOD:
                        shouldDisappear = inventory.AddItem(hitObject);
                        break;
                    default: break;

                }
                if (shouldDisappear)
                {
                    collision.gameObject.SetActive(false);
                }

            }
        }
    }

    public bool AdjustHitPoints(int amount)
    {
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value += amount;
            print("Adjusted hitpoints by: " + amount + ".New value: " + hitPoints);
            return true;
        }
        return false;
    }

    public override void ResetCharacter()
    {
        healthBar = Instantiate(healthBarPrefab);
        inventory = Instantiate(inventoryPrefab);
        healthBar.character = this;
        hitPoints.value = startingHitPoints;

    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            hitPoints.value = hitPoints.value - damage;
            if(hitPoints.value <= float.Epsilon)
            {
                KillCharacter();
                break;
            }
            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);

            }
            else break;
        }
    }

    public override void KillCharacter()
    {
        base.KillCharacter();
        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
    }
}
