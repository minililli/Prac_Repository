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
            if (isMoving && isRunning && moveDir > 0) currentSpeed = runningSpeed;
            else if (isMoving && !isRunning || isMoving && isRunning && moveDir < 0) currentSpeed = moveSpeed;
            else currentSpeed = 0;
            return currentSpeed;
        }
    }

    bool isCrouching = false;
    bool isJumping = false;
    bool isMoving = false;
    bool isRunning = false;
    bool isAttacking = false;
    bool isComboAttacking = false;

    float maxClickSecond = 1f;
    float clickSecond = 0;
    public float ClickSecond
    {
        get => clickSecond;
        private set
        {
            clickSecond = Mathf.Clamp(value, 0, 2);
        }
    }
    int maxComboCount = 2;
    int comboInx = 0;
    public int ComboInx
    {
        get => comboInx;
        private set
        {
            comboInx = Mathf.Clamp(value, 0, maxComboCount);
        }
    }

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
        clickSecond = 0;
    }

    void ResetStartState()
    {
        isCrouching = false;
        isJumping = false;
        isAttacking = false;
        isMoving = false;
        isRunning = false;
    }

    private void InputMove(InputAction.CallbackContext context)
    {
        Vector2 inputDir = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            isMoving = false;
            moveDir = 0;
            rotateDir = 0;
            anim.SetBool("bmove", false);
            anim.SetFloat("moveSpeed", 0);
        }
        else
        {
            isMoving = true;
            if(!isCrouching) SetInput(inputDir, isMoving);
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

        anim.SetBool("bmove", isMoving);
        anim.SetFloat("moveSpeed", CurrentSpeed);
    }

    private void Update()
    {
        if (!isAttacking)
        {
            ClickSecond += Time.deltaTime;
        }

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
        if (!isJumping)
        {
            isJumping = true;
            anim.SetTrigger("tjump");
            rigid.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Land"))
        {
            isJumping = false;
        }
    }

    private void InputRun(InputAction.CallbackContext _)
    {
        isRunning = !isRunning;
        anim.SetFloat("moveSpeed", CurrentSpeed);
    }
    private void InputCrouch(InputAction.CallbackContext _)
    {
        isCrouching = !isCrouching;
        moveDir = 0;
        if (isCrouching)
        {
            anim.SetLayerWeight(2, 1);
            isMoving = false;
        }
        else isMoving = true;
        anim.SetBool("bcrouch", isCrouching);
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
        isAttacking = true;
        anim.SetLayerWeight(3, 1);
        anim.SetBool("battack", isAttacking);
        anim.SetInteger("comboInx", ComboInx);
        ClickSecond = 0f;
        if (ComboInx > 0 && clickSecond < maxClickSecond)
        {
            isComboAttacking = true;
            Debug.Log("Combo");
            anim.SetInteger("comboInx", ComboInx);
            anim.SetBool("isComboAttacking", isAttacking);
            Debug.Log(ComboInx);

        }
        ComboInx++;
    }

    private void EndAttack(InputAction.CallbackContext _)
    {
        isAttacking = false;
        anim.SetBool("battack", isAttacking);
        if (ClickSecond > maxClickSecond || ComboInx > maxComboCount)
        {
            ComboInx = 0;
            isComboAttacking = false;
            anim.SetLayerWeight(3, 0);
        }
    }
    private void InputSkill(InputAction.CallbackContext _)
    {
        isAttacking = true;
        Debug.Log("Skill");
    }
}





