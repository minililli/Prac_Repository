using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting.FullSerializer;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions inputActions;
    Rigidbody rigid;
    Animator anim;


    public float moveSpeed = 2f;
    public float runningSpeed = 4f;
    public float rotateSpeed = 180f;
    public float jumpForce = 5f;


    float moveDir;
    float rotateDir;
    [SerializeField]
    float currentSpeed;
    public float CurrentSpeed
    {
        get
        {
            if (bMove && bRun && moveDir>0) currentSpeed = runningSpeed;
            else if (bMove && !bRun || bMove&&bRun&&moveDir<0) currentSpeed = moveSpeed;
            else currentSpeed = 0;
            return currentSpeed;
        }
    }

    bool bCrouch = false;
    bool bJump = false;
    bool bAttack = false;
    bool bMove = false;
    bool bRun = false;


    private void Awake()
    {
        inputActions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += InputMove;
        inputActions.Player.Move.canceled += InputStop;
        inputActions.Player.Jump.performed += InputJump;
        inputActions.Player.Run.performed += InputRun;
        inputActions.Player.Interaction.performed += InputInteraction;
        inputActions.Player.Attack.performed += InputAttack;
        inputActions.Player.Attack.canceled += EndAttack;
        inputActions.Player.Crouch.performed += InputCrouch;
        inputActions.Player.Skill.performed += InputSkill;
    }

   
    private void OnDisable()
    {
        inputActions.Player.Skill.performed -= InputSkill;
        inputActions.Player.Crouch.performed -= InputCrouch;
        inputActions.Player.Attack.canceled -= EndAttack;
        inputActions.Player.Attack.performed -= InputAttack;
        inputActions.Player.Interaction.performed += InputInteraction;
        inputActions.Player.Run.performed -= InputRun;
        inputActions.Player.Jump.performed -= InputJump;
        inputActions.Player.Move.canceled -= InputStop;
        inputActions.Player.Move.performed -= InputMove;
        inputActions.Player.Disable();
    }

    private void Start()
    {
        ResetStartState();
    }

    void ResetStartState()
    {
        bCrouch = false;
        bJump = false;
        bAttack = false;
        bMove = false;
        bRun = false;
    }

    private void InputMove(InputAction.CallbackContext context)
    {
        Vector2 inputDir = context.ReadValue<Vector2>();
        moveDir = inputDir.y;
        rotateDir = inputDir.x;
        bMove = true;
        anim.SetBool("bmove", bMove);
        anim.SetFloat("moveSpeed", CurrentSpeed);
    }
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }
    private void Move()
    {
        rigid.MovePosition(transform.position + Time.fixedDeltaTime * moveDir * CurrentSpeed * transform.forward);
    }
    private void InputStop(InputAction.CallbackContext _)
    {
        moveDir = 0;
        rotateDir = 0;
        bMove = false;
        anim.SetBool("bmove", false);
        anim.SetFloat("moveSpeed", 0);
    }
  
    private void Rotate()
    {
        Quaternion rotate = Quaternion.AngleAxis(Time.deltaTime * rotateDir * rotateSpeed, transform.up);
        rigid.MoveRotation(transform.rotation * rotate);
    }

    private void InputJump(InputAction.CallbackContext _)
    {
        if (!bJump)
        {
            bJump = true;
            if (bMove) anim.SetTrigger("tjump");
            else anim.SetBool("bjump", true);
            rigid.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Land"))
        {
            bJump = false;
            if (!bMove) anim.SetBool("bjump", false);
        }
    }

    private void InputRun(InputAction.CallbackContext _)
    {
        bRun = !bRun;
        anim.SetFloat("moveSpeed", CurrentSpeed);
    }
    private void InputCrouch(InputAction.CallbackContext _)
    {
        bCrouch = !bCrouch;
        moveDir = 0;
        if (bCrouch)
        {
            Debug.Log("crouch");
            anim.SetLayerWeight(2, 1);
            bMove = false;
            bCrouch = true;
        }
        else
        {
            bCrouch = false;
        }
        anim.SetBool("bcrouch", bCrouch);
    }
    /// <summary>
    /// 상호작용 함수
    /// </summary>
    /// <param name="_">키보드 F키</param>
    private void InputInteraction(InputAction.CallbackContext _)
    {
        Debug.Log("interaction(F)");
        anim.SetTrigger("interaction");
    }


    private void InputAttack(InputAction.CallbackContext _)
    {
        bAttack = true;
        Debug.Log("attack");
    }
    private void EndAttack(InputAction.CallbackContext _)
    {
        bAttack = false;
        Debug.Log("!attack");
    }

    private void InputSkill(InputAction.CallbackContext _)
    {
        bAttack = true;
    }


}

