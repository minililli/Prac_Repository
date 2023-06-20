using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    Vector2 input = new Vector2();

    Rigidbody2D rigid;

    Animator anim;
    string state = "animState";

    enum CharStates
    {
        Idle = 1,
        walkEast=2,
        walkWest=3,
        walkNorth=4,
        walkSouth=5,

    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

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
        if(input.x > 0)
        {
            anim.SetInteger(state, (int)CharStates.walkEast);
        }
        else if(input.x < 0)
        {
            anim.SetInteger(state, (int)CharStates.walkWest);
        }
        else if(input.y > 0)
        {
            anim.SetInteger(state, (int)CharStates.walkNorth);
        }
        else if(input.y < 0) { anim.SetInteger(state, (int)(CharStates.walkSouth));}

        else { anim.SetInteger(state, (int)CharStates.Idle);}
    }
}
