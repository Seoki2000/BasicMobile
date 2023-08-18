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

    [Header("�⺻ �������ͽ�")]
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
        FollowCam._isAppearance = true;     // �̰� �� ������ ����.
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
        _aniControl.SetInteger("AniType", (int)type);       // int.Parse�� �ȸ��� 
        _currAniType = type;
    }

    // ���ڸ����� ȸ���� ��ũ, ������ ���°� ��, �ڷ� ���°� ��ũ ���� ������ Ű���带 ������ ��찡 

    public void MoveAnimation()
    {
        // �̰ɷ� �޾ƿͼ� ����ؾ��ϳ� 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))     // D Ȥ�� ������Ű�� ���� ���
        {
            ChangeAnimationFromType(eAniType.Walk);
            //gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 180);
            //gameObject.transform.Rotate(Vector3.right, 180);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), _rotSpeed);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))     // A Ȥ�� ����Ű�� ���� ���
        {
            ChangeAnimationFromType(eAniType.Walk);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), _rotSpeed);
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))      // W Ȥ�� ����Ű�� ���� ���
        {
            ChangeAnimationFromType(eAniType.Run);
            //gameObject.transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, (transform.position.z + _runSpeed)),_runSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), _rotSpeed);
            gameObject.transform.Translate(Vector3.forward *_runSpeed);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))    // S Ȥ�� �Ʒ�Ű�� ���� ���
        {
            ChangeAnimationFromType(eAniType.Walk);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), _rotSpeed);
            //_cController.Move(Vector3.back * _moveSpeed); �����̵��� 
            gameObject.transform.Translate(Vector3.forward * _walkSpeed);
        }
    }
}
