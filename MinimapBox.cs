using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

public class MinimapBox : MonoBehaviour     //, IPointerEnterHandler, IPointerExitHandler, IDragHandler        // �̷��� �ؼ� ����� ���� �־��µ� 
{
    /*void OnPointerEnter(PointerEventData eventData)
    {
        // �̷������� ����߾���.
    }*/


    const int _minimumZoom = 30;
    const int _maximumZoom = 100;

    [SerializeField] float _zoomAimSize = 100;
    [SerializeField] float _zoomSpeed = 1;      // �̰ɷ� �����س����鼭 �������
    [SerializeField] Image _minimapBG;

    Camera _minimapCam;

    Touch _firstT;
    Touch _secondT;
    Rect _checkRange;       // �̴ϸ� ������ ���ؼ�. Rect���� Left top bottom right�� ������ �̰� ����ؼ� �ص� �������. 

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
        if (Input.touchCount > 1)        // �����ؾ������� �̷��� �� ��� ��ǲ �Ŵ����� ���� �������� ������ �޾ƿ��µ� �̷��� ������ ��� ��ġ�ص� ������ ������.
        {
            // Touch �� position�� ȭ�� ���� �ִ� �������� ������
            _firstT = Input.GetTouch(0);       // �̰� ù��° ��ġ�Ǵ� �༮�� ������ �޾ƿ´�.
            _secondT = Input.GetTouch(1);      // �̰� �ι�° ��ġ�Ǵ� �༮�� �޾ƿ�

            // ��ġ Ư���Ͽ� üũ 
            // ���⼭ Ư���ؾ��ϴµ� rect�� x���̳� y�� �ȿ� �� ��� 
            // ó�� ��ġ�� ��ġ���� ���� �����ӿ����� ��ġ��ġ�� �̹� �����ӿ��� ��ġ��ġ�� ���̸� ����.
            // �� ���̰� ���� �����ӿ����� ��ġ�� ��.

            // distance�� �̷��� ���ϸ� ũ�Ⱑ ��κ� �����״� �׳� ������ �� �� ������ 
            // 615�� max min distance�ε� �̰ź��� ������� �׳� ����Ǹ� ������ ������ �̴ϸ� ���� �ȿ� �ΰ� �� ���� ��츸 �ؾ���.
            if (_firstT.position.x <= _checkRange.max.x && _firstT.position.x >= _checkRange.min.x &&
               _secondT.position.x <= _checkRange.max.x && _secondT.position.y >= _checkRange.min.y)
            {
                // ���� ���̳ĸ� firstPrevPos�� �巡�׸� �ϴ��� ��ġ�� �ٲ��� �ʴ´�.
                // �ٵ� deltaPos�� ���������Ӱ� ���� �����ӻ����� ��ġ �� �̵���ġ�� �ȴ�.

                Vector2 firstPrevPos = _firstT.position - _firstT.deltaPosition;
                Vector2 scondPrevPos = _secondT.position - _secondT.deltaPosition;

                // �� �����ӿ��� ��ġ ������ ���� �Ÿ� ���.
                float prevTouchDeltaMag = (firstPrevPos - scondPrevPos).magnitude;
                float currTouchDeltaMag = (_firstT.position - _secondT.position).magnitude;

                // ���� �Ÿ��� ���� �Ÿ��� ���� ����ؼ� ����(���̳ʽ�)�� �Ǹ� �հ����� �־�������. ���(�÷���)�� �Ǹ� �հ����� ��������� 
                float deltaMagnitudeDiff = prevTouchDeltaMag - currTouchDeltaMag;

                // �ܽ��ǵ尡 ���� �����״� �װ� 0.5f�� �־��ָ� �ȴ�. �ϴ��� ���÷� 0.5f �־��.
                _minimapCam.orthographicSize += deltaMagnitudeDiff * _zoomSpeed;        //�Ʊ� �ּ� 30 �ִ� 100 �� ����� �� orthographicSize�̴�

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
        // �ִ������� �����ϰ� ���ӹ�ư, �߻��ư�� �״�� ����.
        // ���� ���� ��ư�� �ѹ� �� Ŭ�� �� ���󺹱�, �߻��ư Ŭ�� �� ���� �ִ°�� ���� Ƣ�� ����Ʈ 1~2�ʰ� ����
        // ���ӹ�ư ó���� Ŭ���ϸ� Ȯ�� �� Aim�� ���ܾ���.
        _checkCountForButton++;     // ��ư ���� üũ���� Ȯ���� ���ؼ�

        GameObject go = GameObject.FindGameObjectWithTag("Cross");
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(_zoomAimSize, _zoomAimSize);        // ������ ũ�� 

        go = GameObject.FindGameObjectWithTag("MainCamera");
        Camera main = go.GetComponent<Camera>();
        main.fieldOfView = 4;       // ī�޶� ��

        if (_checkCountForButton >= 2)       // �ι� �̻� Ŭ�� �� 
        {
            main.fieldOfView = 60;      // �ٽ� �ʱ�ȭ ��Ű��.
            _checkCountForButton = 0;   // �ʱ�ȭ
        }
    }

    public void ClickShotButton()
    {
        // ��ư ������Ʈ �����ͼ� ������ �� ���¸� Color.Tint�� Ŭ�� �̹��� �����ϰ� ����.
        GameObject shotB = GameObject.FindGameObjectWithTag("ShotButton");
        Button button = shotB.GetComponent<Button>();
        //button.interactable = false; ������� �̰Ÿ� �� ��� ���� None���� �ص� �ϴ��� Ȱ��ȭ�� �Ǿ��ִ� ���¶�� �ؼ� ��.. �� �׻� �ѵδ°ɷ� �ϴ� �ϰ� 

        //Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 100000, Color.red);        // 100�� �ӽ÷� ����
        shotB = GameObject.FindGameObjectWithTag("MainCamera");

        // ���� ���¿����� �� �����̸� _checkCountForButton == 1�� ��쿡�� 
        if (_checkCountForButton == 1)       // ���� ���¿����� �������� �ϴ� ���� 
        {
            Debug.Log(2);
            //button.transition = Selectable.Transition.ColorTint;        // �ѹ� �ٲٰ� ���� ���Ŀ��� �̻��ϰ� �������� ���� �𸣰ڴµ� �ȹٲ� 
            int lMask = 1 << LayerMask.NameToLayer("Water"); // | 1 << LayerMask.NameToLayer("Floor");
            RaycastHit hit;

            // �̰� ���� ��ġ���� ������ _zoomAimSize��ŭ Ray����, lMask
            if (Physics.Raycast(shotB.transform.position, shotB.transform.forward, out hit, 1000000, lMask))
            {
                Debug.Log(1);
                GameObject go = Resources.Load("Effect") as GameObject;
                GameObject.Instantiate(go, hit.transform);        // ���� ������Ʈ, Ʈ������, rotation, parent�� �� �����̴�.
              /*go.transform.position = hit.transform.position;
                go.transform.rotation = hit.transform.rotation;*/
            }
        }
        //button.transition = Selectable.Transition.None;
    }

    void OnGUI()        // ����Ͽ��� ����ġ�� ���ؼ��� �غ� �ؾ���. 
    {
        if (Input.touchCount > 1)
        {
            string ex = string.Format("FirstTouch Pos : {0}", _firstT.position);
            GUI.Label(new Rect(0, Screen.height - 120, 500, 20), ex, _style);
            ex = string.Format("SecondTouch Pos : {0}", _secondT.position);
            GUI.Label(new Rect(0, Screen.height - 100, 500, 20), ex, _style);

            ex = string.Format("Rect : {0}", _minimapBG.rectTransform.rect);
            GUI.Label(new Rect(Screen.width / 2 - 100, 0, 200, 20), ex, _style);


            ex = string.Format("maxX : {0}", _checkRange.max.x);        // 2870  �� x�� 440  ����
            GUI.Label(new Rect(Screen.width / 2 - 120, 320, 200, 20), ex, _style);
            ex = string.Format("minX : {0}", _checkRange.min.x);        // 2430   
            GUI.Label(new Rect(Screen.width / 2 - 120, 340, 200, 20), ex, _style);

            // �ٵ� �̰� �� 3810��� �����°��� 
            // �ϴ��� �ذ��ߴµ� �ؿ� �����ִ� CheckMinimapRange���� ������ �ణ �ʿ���. �ӽ÷� �صаŶ�.

            ex = string.Format("maxY : {0}", _checkRange.max.y);        // 325
            GUI.Label(new Rect(Screen.width / 2 - 120, 380, 200, 20), ex, _style);
            ex = string.Format("minY : {0}", _checkRange.min.y);        // 105
            GUI.Label(new Rect(Screen.width / 2 - 120, 400, 200, 20), ex, _style);

            // 2400 1040 �̷��� �����°Ŵ� ���� ����� �÷������� �ػ󵵰� �Ȱ��� 2960 1440�� �ƴҼ��� �ֱ� ������ �÷����� ������ �������
            // �� Ȯ���� 2400 1040�� �´°ſ��� �̰� �������� ���� �ٽ� ����������.

            // ��ũ�� widht�� ���� 2960 height 1440 �̴ϱ�  width�� 560 ���̳ʽ� ���ְ� height 400���̳ʽ� ������.
        }
    }

    public void CheckMinimapRange()
    {
        //_checkRange = new Rect();
        //x , y, width, height �ε� 
        // _minimapBG.rectTransform�� �����ͼ� �� ���� 
        //_checkRange.left = _minimapBG.rectTransform.rect.width ��ũ�������� �������� ���̳ʽ��ΰ� 

        // 2,3�� Ÿ�� �����´�. p�� boxBG�� pp�� box�̴�.
        RectTransform rectP = gameObject.transform.parent.GetComponent<RectTransform>().transform.parent.GetComponent<RectTransform>();
        RectTransform rectPP = gameObject.transform.parent.GetComponent<RectTransform>().transform.parent.GetComponent<RectTransform>().transform.parent.GetComponent<RectTransform>();
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        // 2��Ÿ�°ų� 3��Ÿ�°ų� �� �� xy �����ͼ� 
        // 3��Ÿ�� ���� x,y,width,height �����ͼ� �̰� ����ϰ� �� ���� ���Ѱɷ� �ٽ� ����� �ؾ���.
        //float left = Screen.width - (rectPP.rect.x - rectP.rect.x- (rect.rect.width / 2));        // �̰� �� -20 �ϰ� -250 �ϰ� - (440/2)���� ���شٴ°� �ƴϿ�
        float right = Screen.width + 50 + (rectPP.rect.x - ((rectPP.rect.width - rect.rect.width) / 2));     // 2960 + ( -20 - ((500 - 440) / 2)) =  2960 -20 -30 �� right
        float left = right + rect.rect.width;       // ���� ����ѰͿ� +�� ���ָ� ������ũ�Ⱑ ���´�.
        //float bottom = Screen.height + 200 + (rectPP.rect.y * 1) + rectP.rect.y + rectP.rect.height;       // �� 0 + (-20 * 1) + 25 + 430 �� ���� bottom �̴ϸ� ���� �ؿ� ��
        float top = (Screen.height - 100) + (rectPP.rect.y * 1) + rectP.rect.y;
        // scrren width height ��ǥ �� ũ�Ⱑ top���� ����ؾ������ʳ� �ִ밡 1440 ���� bottom�� -480�̸� 980���ٵ� ���⿡�� 430�� ���� ���� 1390. �� �̰� top �̿��� �Ѵ�.
        // �ٵ� ��ü �� 800�� ���ؾ����� ��.. . �ϴ��� 800�� ���ؼ� ������ߴµ�
        // 2960 1440�� �ƴ϶� 2440 1080�������� �ڵ��� ��ǥ�� �� �ٸ��� ���Ƽ� �̴ϸ� ��ó�ʿ��� ���ξƿ� �ϸ� ��������... �ϴ� ������ �ٿ��� �̰Ŵ� 
        // �߰������� �����ϸ� �̴ϸʱ��� �����ҰŰ����ϰ� �ϴ� 800�� ��ĳ �ؾ� �´������� �����ؾ��ҵ�.

        _checkRange = new Rect(right, top, rect.rect.width, rect.rect.height);        // ũ��� ���� ������Ʈ recttransform�� �����.
    }
}
