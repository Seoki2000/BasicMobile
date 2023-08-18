using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameInfoDisplay : MonoBehaviour
{
    Vector2Int _resolutionBasic = new Vector2Int(1024, 764);

    [SerializeField] GameObject _prefabUIOptionWnd;
    GameObject _opWnd;
    Resolution[] resolutions;

    bool _isOpen = false;


    public int _selectResolutionNumber
    {
        get;set;
    }

    void Awake()
    {
        /*int index = 0;
        Screen.SetResolution(_resolutionBasic.x, _resolutionBasic.y, false);

        List<Resolution> _resol = new List<Resolution>();
        foreach(Resolution resol in Screen.resolutions)
        {
            // ���� �ػ��� ��� �� ���� �츣���� ���� �༮��
            if(resol.refreshRate >= 59)
            {
                index++;
                if(resol.width == _resolutionBasic.x && resol.height == _resolutionBasic.y)
                {
                    _selectResolutionNumber = index;
                }
                _resol.Add(resol);
            }
        }*/

        //OptionWnd._instance._bgm = 30;
        //OptionWnd._instance._effect = 30;

        //OptionWnd._instance._saveresolx = 720;
        //OptionWnd._instance._saveresoly = 480;

        if (File.Exists(Application.dataPath + "/Resources/Option.txt"))   
        {
            OptionWnd._instance.ReadTxtFile();      // �ִ� ��� �о�ͼ� ������ ���ش�. ���⼭ int���ٰ� ���� ���� �־��ְ� 
            //OptionWnd._instance.SetSliderValue();   // BGM �о�°����� ����.  �̰Ŵ� ���� ���ص� ������ Wnd ų���� �ٲ㵵
            OptionWnd._instance.ScreenSizeChange(); // �ػ� ������ �ߴµ� BGM���� ��
        }
        
    }

    void OnGUI()
    {
        string str = string.Format("Resolusion : {0}, {1}", Screen.width, Screen.height);
        GUI.Label(new Rect(Screen.width / 2 - 100, 0, 200, 50), str);
        if(resolutions != null)
        {
            for(int n = 0; n < resolutions.Length; n++)
            {
                // refreshRate �� �츣�� ��� ���� �ȴ�. 
                str = resolutions[n].width.ToString() + "x" + resolutions[n].height.ToString() + " : "
                                                            + resolutions[n].refreshRate.ToString();   
                float y = 300 + (20 * n);
                GUI.Label(new Rect(0, y, 200, 20), str);
            }
        }
    }

    public void ClickOptionButton()
    {
        // ���� PC���� �������� �ʴ� �ػ󵵰� �ƴ� ��찡 ������ PC�������� ��� ������ �ػ󵵸� �����.
        //Screen.SetResolution(1024, 768, false); 
        //resolutions = Screen.resolutions;      // ��ũ���� �� �ִٰ� ���� �ȴ�.
        /* for(int n = 0; n < resolutions.Length; n++)
         {
             // ���⿡�� �ߴ°Ŷ� ���忡�� �ߴ°� �� �ٸ���. 
             print(resolutions[n].width + "x" + resolutions[n].height + " : " + resolutions[n].refreshRate);
         }*/
        GameObject go = GameObject.FindGameObjectWithTag("OptionWnd");
        if (_isOpen)
        {
            _opWnd.SetActive(true);
        }
        else
        {
            go = GameObject.Instantiate(_prefabUIOptionWnd);
            _opWnd = go;
            _isOpen = true;
        }

    }

   

}
