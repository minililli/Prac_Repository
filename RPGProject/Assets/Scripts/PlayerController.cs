using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class PlayerController : MonoBehaviour
{
    PlayerInputActions inputActions;
    Rigidbody rigid;
    Animator anim;


    public float moveSpeed = 2f;
    public float runningSpeed = 4f;
    public float rotateSpeed = 180f;
    public float jumpForce = 1f;


    float moveDir;
    float rotateDir;
    [SerializeField]
    float currentSpeed;
    public float CurrentSpeed
    {
        get
        {
            if (bMove && bRun && moveDir > 0) currentSpeed = runningSpeed;
            else if (bMove && !bRun || bMove && bRun && moveDir < 0) currentSpeed = moveSpeed;
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
        inputActions.Player.Move.canceled += InputMove;
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
        inputActions.Player.Move.canceled -= InputMove;
        inputActions.Player.Move.performed -= InputMove;
        inputActions.Player.Disable();
    }

    private void Start()
    {
        ResetStartState();
        // 가상 스틱 연결
        VirtualStick stick = FindObjectOfType<VirtualStick>();
        if (stick != null)
        {
            stick.onMoveInput += (input) => SetInput(input, input != Vector2.zero); // 가상 스틱의 입력이 있으면 이동 처리
        }

        // 가상 버튼 연결
        VirtualButton button = FindObjectOfType<VirtualButton>();
        if (button != null)
        {
            button.onClick += Jump;
            //onJumpCoolTimeChange += button.RefreshCoolTime;     // 점프 쿨타임이 변하면 버튼의 쿨타임 표시 변경
        }
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

        if (context.canceled)
        {
            bMove = false;
            moveDir = 0;
            rotateDir = 0;
            anim.SetBool("bmove", false);
            anim.SetFloat("moveSpeed", 0);
        }
        else
        {
            bMove = true;
            SetInput(inputDir, bMove);
        }

    }
    /// <summary>
    /// 입력에 따라 이동 처리용 변수에 값을 설정하는 함수
    /// </summary>
    /// <param name="input">이동 방향</param>
    /// <param name="isMove">이동 중인지 아닌지(true면 이동중, false면 정지 중)</param>
    private void SetInput(Vector2 inputDir, bool isMove)
    {
        moveDir = inputDir.y;
        rotateDir = inputDir.x;

            if (!bCrouch)
            {
                if (inputDir != Vector2.zero) bMove = true;
                else bMove = false;
            }
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


    private void Rotate()
    {
        Quaternion rotate = Quaternion.AngleAxis(Time.deltaTime * rotateDir * rotateSpeed, transform.up);
        rigid.MoveRotation(transform.rotation * rotate);
    }

    private void InputJump(InputAction.CallbackContext _)
    {

        Jump();

    }

    void Jump()
    {
        if (!bJump)
        {
            bJump = true;
            anim.SetTrigger("tjump");
            rigid.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Land"))
        {
            bJump = false;
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
        }
        else bMove = true;

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

