using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public enum eAniType
    {
        Idle        = 0,
        Walk,
        Run,
        Fire,
        Dead        = 99
    }

    [Header("기본 스테이터스")]
    [SerializeField] float _walkSpeed = 0.5f;
    [SerializeField] float _runSpeed = 4;

    eAniType _currAniType;
    float _moveSpeed = 0;
    float _rotSpeed = 60;
    bool _isDeath = false;


    CharacterController _cController;      
    Animator _aniControl;

    private void Awake()
    {
        _cController = GetComponent<CharacterController>();
        _aniControl = GetComponent<Animator>();

        _moveSpeed = _walkSpeed;
    }
    private void Start()
    {
        FollowCam._isAppearance = true;     // 이게 왜 문제가 있지.
    }
    private void Update()
    {
        MoveAnimation();
    }

    public void ChangeAnimationFromType(eAniType type)
    {
        if (_isDeath)
            return;

        switch (type)
        {
            case eAniType.Fire:
                _aniControl.SetTrigger("Fire");
                break;
            case eAniType.Dead:
                _aniControl.SetTrigger("Dead");
                break;
        }
        _aniControl.SetInteger("AniType", (int)type);       // int.Parse는 안먹음 
        _currAniType = type;
    }

    // 제자리에서 회전도 워크, 앞으로 가는거 런, 뒤로 가는건 워크 왼쪽 오른쪽 키보드를 누르는 경우가 

    public void MoveAnimation()
    {
        // 이걸로 받아와서 사용해야하나 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))     // D 혹은 오른쪽키가 눌린 경우
        {
            ChangeAnimationFromType(eAniType.Walk);
            //gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 180);
            //gameObject.transform.Rotate(Vector3.right, 180);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), _rotSpeed);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))     // A 혹은 왼쪽키가 눌린 경우
        {
            ChangeAnimationFromType(eAniType.Walk);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), _rotSpeed);
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))      // W 혹은 위에키가 눌린 경우
        {
            ChangeAnimationFromType(eAniType.Run);
            //gameObject.transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, (transform.position.z + _runSpeed)),_runSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), _rotSpeed);
            gameObject.transform.Translate(Vector3.forward *_runSpeed);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))    // S 혹은 아래키가 눌린 경우
        {
            ChangeAnimationFromType(eAniType.Walk);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), _rotSpeed);
            //_cController.Move(Vector3.back * _moveSpeed); 순간이동임 
            gameObject.transform.Translate(Vector3.forward * _walkSpeed);
        }
    }
}
