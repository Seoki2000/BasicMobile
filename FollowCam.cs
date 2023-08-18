using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] Vector3 _offSet;
    [SerializeField] float _angleX = 35;
    [SerializeField] float _followSpeed = 3;
    [SerializeField] float _followRot = 30;

    const int _miniFOV = 10;
    const int _maxiFOV = 150;

    Camera _myCam;
    Transform _playerObj;
    Vector3 _goalPos;

    float _wheelSpeed = 10;
    

    public static bool _isAppearance
    {
        get;set;
    }

    private void Awake()
    {
        _myCam = GetComponent<Camera>();
        _isAppearance = true;
        
    }
    public void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * _wheelSpeed; // Edit�� Project Setting�� Inuput Manager�ȿ� �ִ� ���콺 ���� �����´�.
        Debug.Log(scroll);

        // ���ǹ��� ������ �Ŵ� ��찡 ����. �� ���� ������ �������� �ϴ°� �ƴ϶� �Ѱ迡 �����ؼ� �װ� �������� �ؾ��Ѵ�. 
        if(_myCam.fieldOfView < _miniFOV)
        {
            _myCam.fieldOfView = _miniFOV;
        }
        if (_myCam.fieldOfView > _maxiFOV)
        {
            _myCam.fieldOfView = _maxiFOV;
        }
        _myCam.fieldOfView += scroll;       // ������ 0.1�� �ٲ�µ� �ʹ� ������ ���� �߰��� �������ִ°� ���ٰ� �����Ѵ�.
        // �̰� ����Ϸ� �Ѿ�� Ȯ���ϴ� �巡�׷� �� �� �ִµ� �̰Ŵ� ���߿� Ȱ���� �غ��� �Ѵ�. 


        if (_isAppearance)
        {
            if (_playerObj == null)
            {
                _playerObj = GameObject.FindGameObjectWithTag("Player").transform;
            }
            else
            {
                MoveCam();
            } 
        }
    }
    public void MoveCam()
    {
        /*Vector3 pos = _playerObj.transform.position + _offSet;
        Vector3 temp = Vector3.Lerp(transform.position, pos, _followSpeed);
        transform.position = temp;
        transform.LookAt(_playerObj.transform);*/

        Quaternion lookTargett = _playerObj.rotation;
        float currYAngle = Mathf.LerpAngle(transform.eulerAngles.y, _playerObj.eulerAngles.y, _followRot * Time.deltaTime);

        Quaternion rot = Quaternion.Euler(lookTargett.eulerAngles.x, currYAngle, 0);
        _goalPos = _playerObj.position - (rot * Vector3.forward * _offSet.z)
            + (Vector3.up * _offSet.y);
        transform.position = Vector3.Lerp(transform.position, _goalPos, _followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(rot.eulerAngles);
        // ������� ����� ��� 

       /* Vector3 num = _playerObj.position + _offSet;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_playerObj.position), _followRot);
        transform.position = Vector3.MoveTowards(transform.position, num, _followSpeed);
        transform.LookAt(_playerObj);*/
        // �ڷ� ���ư��� �ɾ�� ������ �ʰ� ��� �ٲ��. 

        /*float currAngle = Mathf.LerpAngle(transform.eulerAngles.y, _playerObj.eulerAngles.y,
            _followRot * Time.deltaTime);

        Quaternion rot = Quaternion.Euler(0, currAngle, 0);

        transform.position = _playerObj.position - (rot * Vector3.forward * _offSet.z)
            + (Vector3.up * _offSet.y);

        transform.LookAt(_playerObj); �̰� ã�� �����ε� ���⼭ ������ �߰��ϰ� ������ �ٲ��ָ� �ϼ��ε� �ٽ� �ѹ� �����غ��� */

    }


}
