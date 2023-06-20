using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public HealthBar healthBarPrefab;
    HealthBar healthbar;

    public Inventory inventoryPrefab;
    Inventory inventory;


    public float moveSpeed = 3.0f;
    Vector2 input = new Vector2();

    Rigidbody2D rigid;

    Animator anim;
    string state = "animState";

    enum CharStates
    {
        Idle = 1,
        walkEast = 2,
        walkWest = 3,
        walkNorth = 4,
        walkSouth = 5,

    }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        healthbar = Instantiate(healthBarPrefab);
        healthbar.character = this;
        hitPoints.value = startingHitPoints;

        inventory = Instantiate(inventoryPrefab);
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
        if (input.x > 0)
        {
            anim.SetInteger(state, (int)CharStates.walkEast);
        }
        else if (input.x < 0)
        {
            anim.SetInteger(state, (int)CharStates.walkWest);
        }
        else if (input.y > 0)
        {
            anim.SetInteger(state, (int)CharStates.walkNorth);
        }
        else if (input.y < 0) { anim.SetInteger(state, (int)(CharStates.walkSouth)); }

        else { anim.SetInteger(state, (int)CharStates.Idle); }
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
}
