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
        float scroll = Input.GetAxis("Mouse ScrollWheel") * _wheelSpeed; // Edit에 Project Setting에 Inuput Manager안에 있는 마우스 휠을 가져온다.
        Debug.Log(scroll);

        // 조건문에 제한을 거는 경우가 많다. 이 경우는 조건을 기준으로 하는게 아니라 한계에 집중해서 그걸 기준으로 해야한다. 
        if(_myCam.fieldOfView < _miniFOV)
        {
            _myCam.fieldOfView = _miniFOV;
        }
        if (_myCam.fieldOfView > _maxiFOV)
        {
            _myCam.fieldOfView = _maxiFOV;
        }
        _myCam.fieldOfView += scroll;       // 돌릴때 0.1씩 바뀌는데 너무 적으니 값을 추가로 연산해주는게 좋다고 생각한다.
        // 이걸 모바일로 넘어가면 확대하는 드래그로 할 수 있는데 이거는 나중에 활용을 해봐야 한다. 


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
        // 여기까지 강사님 답안 

       /* Vector3 num = _playerObj.position + _offSet;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_playerObj.position), _followRot);
        transform.position = Vector3.MoveTowards(transform.position, num, _followSpeed);
        transform.LookAt(_playerObj);*/
        // 뒤로 돌아가서 걸어가면 따라가지 않고 즉시 바뀐다. 

        /*float currAngle = Mathf.LerpAngle(transform.eulerAngles.y, _playerObj.eulerAngles.y,
            _followRot * Time.deltaTime);

        Quaternion rot = Quaternion.Euler(0, currAngle, 0);

        transform.position = _playerObj.position - (rot * Vector3.forward * _offSet.z)
            + (Vector3.up * _offSet.y);

        transform.LookAt(_playerObj); 이게 찾은 내용인데 여기서 골포스 추가하고 마지막 바꿔주면 완성인데 다시 한번 생각해보자 */

    }


}
