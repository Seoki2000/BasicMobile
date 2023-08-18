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
            // 같은 해상도인 경우 더 높은 헤르츠를 가진 녀석을
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
            OptionWnd._instance.ReadTxtFile();      // 있는 경우 읽어와서 설정을 해준다. 여기서 int에다가 값을 전부 넣어주고 
            //OptionWnd._instance.SetSliderValue();   // BGM 읽어온값으로 설정.  이거는 당장 안해도 괜찮고 Wnd 킬때만 바꿔도
            OptionWnd._instance.ScreenSizeChange(); // 해상도 변경은 뜨는데 BGM값이 엄
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
                // refreshRate 가 헤르츠 라고 보면 된다. 
                str = resolutions[n].width.ToString() + "x" + resolutions[n].height.ToString() + " : "
                                                            + resolutions[n].refreshRate.ToString();   
                float y = 300 + (20 * n);
                GUI.Label(new Rect(0, y, 200, 20), str);
            }
        }
    }

    public void ClickOptionButton()
    {
        // 가끔 PC에서 제공하지 않는 해상도가 아닌 경우가 있으니 PC설정에서 사용 가능한 해상도를 써야함.
        //Screen.SetResolution(1024, 768, false); 
        //resolutions = Screen.resolutions;      // 스크린에 다 있다고 보면 된다.
        /* for(int n = 0; n < resolutions.Length; n++)
         {
             // 여기에서 뜨는거랑 빌드에서 뜨는게 또 다르다. 
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
