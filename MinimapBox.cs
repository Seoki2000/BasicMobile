using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

public class MinimapBox : MonoBehaviour     //, IPointerEnterHandler, IPointerExitHandler, IDragHandler        // 이렇게 해서 사용한 적이 있었는데 
{
    /*void OnPointerEnter(PointerEventData eventData)
    {
        // 이런식으로 사용했었다.
    }*/


    const int _minimumZoom = 30;
    const int _maximumZoom = 100;

    [SerializeField] float _zoomAimSize = 100;
    [SerializeField] float _zoomSpeed = 1;      // 이걸로 조정해나가면서 사용하자
    [SerializeField] Image _minimapBG;

    Camera _minimapCam;

    Touch _firstT;
    Touch _secondT;
    Rect _checkRange;       // 미니맵 범위를 위해서. Rect에는 Left top bottom right도 있으니 이걸 사용해서 해도 상관없다. 

    GUIStyle _style = new GUIStyle();

    float _checkCountForButton;

    void Start()
    {
        //Debug.Log(gameObject.GetComponentInParent<RectTransform>().transform.parent.GetComponent<RectTransform>());
        //Debug.Log(gameObject.GetComponentInParent<RectTransform>());

        _checkCountForButton = 0;

        InitMinimapBox();
        CheckMinimapRange();


        Debug.Log("Max");
        Debug.Log(_checkRange.max.x);
        Debug.Log(_checkRange.max.y);
        Debug.Log("Min");
        Debug.Log(_checkRange.min.x);
        Debug.Log(_checkRange.min.y);
    }

    public void InitMinimapBox()
    {
        GameObject go = GameObject.FindGameObjectWithTag("MinimapCamera");
        _minimapCam = go.GetComponent<Camera>();

        _style.normal.textColor = Color.red;
        _style.fontSize = 20;
    }

    void Update()
    {
        if (Input.touchCount > 1)        // 주의해야할점은 이렇게 할 경우 인풋 매니저는 게임 전반적인 곳에서 받아오는데 이렇게 따지면 어딜 터치해도 감지가 가능함.
        {
            // Touch 에 position은 화면 내의 있는 포지션을 가져옴
            _firstT = Input.GetTouch(0);       // 이게 첫번째 터치되는 녀석의 정보를 받아온다.
            _secondT = Input.GetTouch(1);      // 이게 두번째 터치되는 녀석을 받아옴

            // 위치 특정하여 체크 
            // 여기서 특정해야하는데 rect의 x축이나 y축 안에 들어간 경우 
            // 처음 터치한 위치에서 이전 프레임에서의 터치위치와 이번 프레임에서 터치위치의 차이를 저장.
            // 이 차이가 이전 프레임에서의 위치가 됨.

            // distance로 이렇게 비교하면 크기가 대부분 작을테니 그냥 연산이 될 거 같은데 
            // 615가 max min distance인데 이거보다 작은경우 그냥 실행되면 무조건 오류임 미니맵 범위 안에 두개 다 들어온 경우만 해야함.
            if (_firstT.position.x <= _checkRange.max.x && _firstT.position.x >= _checkRange.min.x &&
               _secondT.position.x <= _checkRange.max.x && _secondT.position.y >= _checkRange.min.y)
            {
                // 무슨 뜻이냐면 firstPrevPos가 드래그를 하더라도 위치가 바뀌지 않는다.
                // 근데 deltaPos는 이전프레임과 현재 프레임사이의 위치 즉 이동위치가 된다.

                Vector2 firstPrevPos = _firstT.position - _firstT.deltaPosition;
                Vector2 scondPrevPos = _secondT.position - _secondT.deltaPosition;

                // 각 프레임에서 터치 사이의 벡터 거리 계산.
                float prevTouchDeltaMag = (firstPrevPos - scondPrevPos).magnitude;
                float currTouchDeltaMag = (_firstT.position - _secondT.position).magnitude;

                // 이전 거리와 지금 거리의 차를 계산해서 음수(마이너스)가 되면 손가락이 멀어진것임. 양수(플러스)가 되면 손가락이 가까워진것 
                float deltaMagnitudeDiff = prevTouchDeltaMag - currTouchDeltaMag;

                // 줌스피드가 따로 있을테니 그걸 0.5f에 넣어주면 된다. 일단은 예시로 0.5f 넣어둠.
                _minimapCam.orthographicSize += deltaMagnitudeDiff * _zoomSpeed;        //아까 최소 30 최대 100 인 사이즈가 이 orthographicSize이다

                if (_minimapCam.orthographicSize < _minimumZoom)
                {
                    _minimapCam.orthographicSize = _minimumZoom;
                }
                else if (_minimapCam.orthographicSize > _maximumZoom)
                {
                    _minimapCam.orthographicSize = _maximumZoom;
                }
            }
        }
    }

    public void ClickAimButton()
    {
        // 최대줌으로 변경하고 에임버튼, 발사버튼이 그대로 있음.
        // 만약 에임 버튼을 한번 더 클릭 시 원상복구, 발사버튼 클릭 시 벽이 있는경우 파편 튀는 이펙트 1~2초간 생성
        // 에임버튼 처음에 클릭하면 확대 및 Aim이 생겨야함.
        _checkCountForButton++;     // 버튼 연속 체크인지 확인을 위해서

        GameObject go = GameObject.FindGameObjectWithTag("Cross");
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(_zoomAimSize, _zoomAimSize);        // 사이즈 크게 

        go = GameObject.FindGameObjectWithTag("MainCamera");
        Camera main = go.GetComponent<Camera>();
        main.fieldOfView = 4;       // 카메라 줌

        if (_checkCountForButton >= 2)       // 두번 이상 클릭 시 
        {
            main.fieldOfView = 60;      // 다시 초기화 시키기.
            _checkCountForButton = 0;   // 초기화
        }
    }

    public void ClickShotButton()
    {
        // 버튼 컴포넌트 가져와서 에임을 줌 상태만 Color.Tint로 클릭 이미지 가능하게 해줌.
        GameObject shotB = GameObject.FindGameObjectWithTag("ShotButton");
        Button button = shotB.GetComponent<Button>();
        //button.interactable = false; 고민중임 이거를 뭘 어떻게 할지 None으로 해도 일단은 활성화는 되어있는 상태라고 해서 음.. 걍 항상 켜두는걸로 일단 하고 

        //Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 100000, Color.red);        // 100은 임시로 설정
        shotB = GameObject.FindGameObjectWithTag("MainCamera");

        // 줌인 상태에서만 할 예정이면 _checkCountForButton == 1인 경우에만 
        if (_checkCountForButton == 1)       // 줌인 상태에서만 가능으로 일단 설정 
        {
            Debug.Log(2);
            //button.transition = Selectable.Transition.ColorTint;        // 한번 바꾸고 나서 이후에는 이상하게 오류인지 뭔지 모르겠는데 안바뀜 
            int lMask = 1 << LayerMask.NameToLayer("Water"); // | 1 << LayerMask.NameToLayer("Floor");
            RaycastHit hit;

            // 이게 현재 위치에서 앞으로 _zoomAimSize만큼 Ray길이, lMask
            if (Physics.Raycast(shotB.transform.position, shotB.transform.forward, out hit, 1000000, lMask))
            {
                Debug.Log(1);
                GameObject go = Resources.Load("Effect") as GameObject;
                GameObject.Instantiate(go, hit.transform);        // 생성 오브젝트, 트랜스폼, rotation, parent를 할 예정이다.
              /*go.transform.position = hit.transform.position;
                go.transform.rotation = hit.transform.rotation;*/
            }
        }
        //button.transition = Selectable.Transition.None;
    }

    void OnGUI()        // 모바일에서 투터치를 위해서는 준비를 해야함. 
    {
        if (Input.touchCount > 1)
        {
            string ex = string.Format("FirstTouch Pos : {0}", _firstT.position);
            GUI.Label(new Rect(0, Screen.height - 120, 500, 20), ex, _style);
            ex = string.Format("SecondTouch Pos : {0}", _secondT.position);
            GUI.Label(new Rect(0, Screen.height - 100, 500, 20), ex, _style);

            ex = string.Format("Rect : {0}", _minimapBG.rectTransform.rect);
            GUI.Label(new Rect(Screen.width / 2 - 100, 0, 200, 20), ex, _style);


            ex = string.Format("maxX : {0}", _checkRange.max.x);        // 2870  즉 x는 440  나옴
            GUI.Label(new Rect(Screen.width / 2 - 120, 320, 200, 20), ex, _style);
            ex = string.Format("minX : {0}", _checkRange.min.x);        // 2430   
            GUI.Label(new Rect(Screen.width / 2 - 120, 340, 200, 20), ex, _style);

            // 근데 이게 왜 3810대로 나오는거지 
            // 일단은 해결했는데 밑에 적혀있는 CheckMinimapRange에서 수정이 약간 필요함. 임시로 해둔거라서.

            ex = string.Format("maxY : {0}", _checkRange.max.y);        // 325
            GUI.Label(new Rect(Screen.width / 2 - 120, 380, 200, 20), ex, _style);
            ex = string.Format("minY : {0}", _checkRange.min.y);        // 105
            GUI.Label(new Rect(Screen.width / 2 - 120, 400, 200, 20), ex, _style);

            // 2400 1040 이렇게 나오는거는 실제 모바일 플랫폼에서 해상도가 똑같이 2960 1440이 아닐수도 있기 때문에 플랫폼에 무조건 맞춰야함
            // 즉 확인한 2400 1040이 맞는거여서 이걸 기준으로 값을 다시 계산해줘야함.

            // 스크린 widht가 현재 2960 height 1440 이니까  width는 560 마이너스 해주고 height 400마이너스 해주자.
        }
    }

    public void CheckMinimapRange()
    {
        //_checkRange = new Rect();
        //x , y, width, height 인데 
        // _minimapBG.rectTransform을 가져와서 그 값을 
        //_checkRange.left = _minimapBG.rectTransform.rect.width 스크린사이즈 기준으로 마이너스인가 

        // 2,3번 타서 가져온다. p는 boxBG고 pp는 box이다.
        RectTransform rectP = gameObject.transform.parent.GetComponent<RectTransform>().transform.parent.GetComponent<RectTransform>();
        RectTransform rectPP = gameObject.transform.parent.GetComponent<RectTransform>().transform.parent.GetComponent<RectTransform>().transform.parent.GetComponent<RectTransform>();
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        // 2번타는거나 3번타는거나 둘 다 xy 가져와서 
        // 3번타고 들어가서 x,y,width,height 가져와서 이값 계산하고 그 값에 대한걸로 다시 계산을 해야함.
        //float left = Screen.width - (rectPP.rect.x - rectP.rect.x- (rect.rect.width / 2));        // 이게 즉 -20 하고 -250 하고 - (440/2)값을 해준다는거 아니여
        float right = Screen.width + 50 + (rectPP.rect.x - ((rectPP.rect.width - rect.rect.width) / 2));     // 2960 + ( -20 - ((500 - 440) / 2)) =  2960 -20 -30 이 right
        float left = right + rect.rect.width;       // 왼쪽 계산한것에 +를 해주면 오른쪽크기가 나온다.
        //float bottom = Screen.height + 200 + (rectPP.rect.y * 1) + rectP.rect.y + rectP.rect.height;       // 즉 0 + (-20 * 1) + 25 + 430 한 값이 bottom 미니맵 가장 밑에 쪽
        float top = (Screen.height - 100) + (rectPP.rect.y * 1) + rectP.rect.y;
        // scrren width height 좌표 및 크기가 top까지 계산해야하지않나 최대가 1440 에서 bottom이 -480이면 980일텐데 여기에서 430을 더한 값이 1390. 즉 이게 top 이여야 한다.
        // 근데 대체 왜 800을 더해야하지 흠.. . 일단은 800을 더해서 만들긴했는데
        // 2960 1440이 아니라 2440 1080느낌으로 핸드폰 좌표는 또 다른거 같아서 미니맵 근처쪽에서 줌인아웃 하면 가능해짐... 일단 범위는 줄여서 이거는 
        // 추가적으로 수정하면 미니맵까지 가능할거같긴하고 일단 800이 어캐 해야 맞는지부터 생각해야할듯.

        _checkRange = new Rect(right, top, rect.rect.width, rect.rect.height);        // 크기는 현재 오브젝트 recttransform에 맞춘다.
    }
}
