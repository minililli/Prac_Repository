using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    PlayerInputActions inputActions;
    Rigidbody rigid;
    Runner runner;
    Animator anim;
    Vector3 inputDir;
    public GameObject finishLine;
    public GameObject stopSign;
    public GameObject clearPanel;
    public GameObject GameOverPanel;
    bool onPlay = true;
    bool onMove = false;
    bool onJump = false;

    float count = 0;
    float Count
    {
        get => count;
        set
        {
            count = value;
            Math.Clamp(count, 0, 1);
        }
    }
    public float moveForce = 10f;
    public float rotateSpeed = 5f;
    public float jumpForce = 1.0f;

    // -----장착관련 변수
    bool onEquip = false;
    public Transform GunPivot;
    public Transform LHandMount;
    public Transform RHandMount;



    private void Awake()
    {
        inputActions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        runner = GetComponent<Runner>();
    }
    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Equip.performed += OnEquip;
    }



    private void OnDisable()
    {
        inputActions.Player.Equip.performed -= OnEquip;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Move.canceled -= OnStop;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
        inputActions.Disable();
    }



    private void Start()
    {
        clearPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        anim.SetLayerWeight(1, 0);
        GunPivot.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (onPlay)
        {
            rigid.AddForceAtPosition(moveForce * inputDir, transform.position, ForceMode.Acceleration);
            if (!runner.onInvincible)
            {
                Count += 0.1f;
                anim.SetFloat("Speed", Count);
            }
            if (stopSign.transform.position.z < transform.position.z)
            {
                Count = 0;
                anim.SetFloat("Speed", 0);
            }
            if (runner.currenthp < 0)
            {
                GameOverPanel.SetActive(true);
                onPlay = false;
                anim.SetTrigger("OnDie");
            }
        }
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        if (!runner.onInvincible)
        {
            inputDir.x = input.y;
            inputDir.z = -input.x;
            stateMove();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputDir), Time.fixedDeltaTime * rotateSpeed);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (onJump && collision.gameObject.layer == 3)
        {
            onJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == finishLine)
        {
            anim.SetFloat("Speed", 0);
            anim.SetBool("Move", false);
            onPlay = false;
            OnDisable();
            StartCoroutine(End());
        }
    }
    private void OnJump(InputAction.CallbackContext _)
    {
        if (!onJump)
        {
            onJump = true;
            rigid.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
        }
    }
    private void OnStop(InputAction.CallbackContext obj)
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.inertiaTensor = Vector3.zero;
    }

    private void OnEquip(InputAction.CallbackContext _)
    {
        if (!onEquip)
        {
            onEquip = true;
            anim.SetLayerWeight(1, 0.5f);
            //anim.SetLayerWeight(1, 1f);

        }
        else
        {
            onEquip = false;
            anim.SetLayerWeight(1, 0);
            GunPivot.parent.gameObject.SetActive(false);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (onEquip)
        {
            GunPivot.parent.gameObject.transform.position = transform.position + transform.forward * 0.2f + transform.up * 1.2f ;
            GunPivot.parent.gameObject.transform.forward = transform.forward;
            GunPivot.parent.gameObject.SetActive(true);
            GunPivot.position = anim.GetIKHintPosition(AvatarIKHint.RightElbow);

            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, LHandMount.position);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, LHandMount.rotation);

            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKPosition(AvatarIKGoal.RightHand, RHandMount.position);
            anim.SetIKRotation(AvatarIKGoal.RightHand, RHandMount.rotation);
        }
    }

    void stateMove()
    {
        onMove = true;
        anim.SetBool("Move", true);
    }

    IEnumerator End()
    {
        while (transform.position.x < 3.2f)
        {
            transform.rotation *= Quaternion.LookRotation(-transform.right);
            anim.GetLayerIndex("Base Layer");
            anim.SetBool("Move", true);
            anim.SetFloat("Speed", 0);
            transform.Translate(Time.fixedDeltaTime * Vector3.forward);
            yield return null;
        }
        gameObject.SetActive(false);
        clearPanel.SetActive(true);
        StopCoroutine(End());
    }


}
