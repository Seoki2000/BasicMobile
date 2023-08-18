using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class OptionWnd : MonoBehaviour
{
    private static OptionWnd Instance;

    public static OptionWnd _instance
    {
        get
        {
            if (null == Instance)
            {
                return null;
            }
            return Instance;
        }
    }
    // 일단 그냥 생성이라고 가정하고
    // Item Label에 텍스트들을 전부 바꿔줘야하고 이걸 선택하면 ResolutiuonList에 있는 Label 텍스트를 바꿔야한다.
    // 확인을 누른 경우 현재 Label 텍스트를 가져오고 그거에 맞게 해상도를 변경해야함.

    // 체크박스로 풀스크린 모드 따로 만들어보자. 이전까지 가져온 것들은 전부 가능하고 큰 문제 없이 List에 넣어주면 전부 가능하다. 
    // 풀 스크린 모드를 넣어줄려면 체크표시를 한개 더 만들고 체크를 했는지에 따라서 bool형식으로 한개 가져온다.
    // 체크를 두번 클릭해서 다시 취소를 할 수 있으니 이미지 나오게 하는
    // bool형 두개를 가져와야하고 ... 엄.. 



    [SerializeField] GameObject _resolutionList;        // Dropdown 가져오기 위해서 
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _effSlider;
    Dropdown _options;

    List<string> _optionList = new List<string>();

    string _dropdownKey = "DropDownKey";
    int _nowOption;

    // GameInfoDisplay에서 했어야했는데
    public int _bgm;
    public int _effect;
    public int _saveresolx;
    public int _saveresoly;

    public bool _isOpen = false;

    void Awake()
    {
        Instance = this;
        _options = _resolutionList.GetComponent<Dropdown>();
        // 볼륨 초기값 설정.
        /* SetSliderValue();

         //Debug.Log(File.Exists(Application.dataPath + "/Resources/Option.txt"));     // .txt까지 해야지 찾아진다. 근데 읽어오는게 이상하게 해상도 가져와서 하는게 안된다.
         //Debug.Log(Application.dataPath + "/Resources/Option");
         //Debug.Log(Application.dataPath);
         //Debug.Log(Resources.Load("Option") != null);
         *//*string str = Application.dataPath + "/Resources/Option.txt";
         Debug.Log(str);*/
        // GameInfo에서 해상도 가져오는건 성공했는데 왜 슬라이더 밸류값 넣어주는건 못했을까.
        if (File.Exists(Application.dataPath + "/Resources/Option.txt"))     // 파일이 있는 경우 
        {
            ReadTxtFile();      // 있는 경우 읽어와서 설정을 해준다. 여기서 int에다가 값을 전부 넣어주고 
            SetSliderValue();   // BGM 읽어온값으로 설정. 
            //Screen.SetResolution(_saveresolx, _saveresoly, false);      // 파일에서 읽은값으로 설정.
            //ScreenSizeChange(); // 읽어온 값으로 해상도 변경이 되어야하는데 
        }



    }

    void Start()
    {
        _options.ClearOptions();
        //640 480, 720 480, 800 600, 1024 768, 1176 664, 1280 720, 1280 960, 1280 1024
        _optionList.Add("640 x 480");
        _optionList.Add("720 x 480");
        _optionList.Add("800 x 600");
        _optionList.Add("1024 x 768");
        _optionList.Add("1176 x 664");
        _optionList.Add("1280 x 720");
        _optionList.Add("1280 x 960");
        _optionList.Add("1280 x 1024");

        _options.AddOptions(_optionList);
        // 클릭해서 변경까지 가능하고 변경 후 확인을 누르면 그 해상도로 변경되어야 하는데. Label에 있는 텍스트 값을 가져와서 그대로 바꿔준다.

        _options.onValueChanged.AddListener(delegate { SetDropDown(_options.value); });
        SetDropDown(1);
    }

    void SetDropDown(int option)
    {
        PlayerPrefs.SetInt(_dropdownKey, option);
        _nowOption = option;    // option 번호 저장 
        Debug.Log("current option : " + option);        // 나오니까 여기에 button에 확인 취소를 넣어둬서 확인인 경우  바꾸고 취소인 경우 원래대로
    }

    public void ClickYesButton()
    {
        SaveSoundVal();     // 사운드 저장 
        // 선택한 번호를 GameInfoDisplay에 알려줘야 한다.
        int a = 0;
        int b = 0;

        switch (_nowOption)
        {
            case 0:
                a = 640;
                b = 480;
                break;
            case 1:
                a = 720;
                b = 480;
                break;
            case 2:
                a = 800;
                b = 600;
                break;
            case 3:
                a = 1024;
                b = 768;
                break;
            case 4:
                a = 1176;
                b = 664;
                break;
            case 5:
                a = 1280;
                b = 720;
                break;
            case 6:
                a = 1280;
                b = 960;
                break;
            case 7:
                a = 1280;
                b = 1024;
                break;
        }
        Screen.SetResolution(a, b, false);
        gameObject.SetActive(false);
        _saveresolx = a;
        _saveresoly = b;
        CreateFile();
    }

    public void ScreenSizeChange()
    {
        Screen.SetResolution(_saveresolx, _saveresoly, false);
    }

    public void SetSliderValue()        // 옵션버튼 클릭 시 사용
    {
        _bgmSlider.value = _bgm;
        _effSlider.value = _effect;
    }
    public void ClickYNoButton()
    {
        SaveSoundVal();     // 사운드 저장 
        gameObject.SetActive(false);
        _isOpen = false;
    }
    public void SaveSoundVal()
    {
        _bgm = (int)_bgmSlider.value;       // 밸류값 저장
        _effect = (int)_effSlider.value;    // 밸류값 저장.
    }
    public void CreateFile()        // 없는 경우 최초로 생성한다. 
    {
        string str = "Assets/Resources/Option";         // 파일 경로  Application.dataPath 이거 쓰면 // D:/Unity/Myproject/Assets이런식임
        StreamWriter sw;
        /*if (File.Exists(str))          // 파일 있는지 확인 
        {
            //File.Create("Assets/Resources/Option.txt");     // 텍스트 파일 생성
            sw = new StreamWriter(str + ".txt");        // 텍스트 파일 생성 
            sw.WriteLine(sw);
            sw.Flush();
            sw.Close();                               
        }
        else
        {*/
        // 근데 이렇게 할거면 걍 if else문 없이 걍 써도 있든 없든 걍 체크가 될 것 같은데 
        //sw = new StreamWriter(File.Open("Option", FileMode.Open));
        sw = new StreamWriter(File.Create(str + ".txt"));
        //sw.write(File.WriteAllText(str + ".txt", string.Empty));  // 텍스트 내용 전체 삭제
        sw.WriteLine("Resolusions({0}, {1})", _saveresolx, _saveresoly);        // 해상도 저장
        sw.WriteLine("BGMVolume({0})", _bgm);       // 배경음 저장   
        sw.WriteLine("EFFVolume({0})", _effect);    // 효과음 저장
        sw.Close();                                 // 파일 닫기
                                                    // 저장까지는 어찌저찌 했는데 이거 저장한거 가져와야함. 그걸 찾아야하는데 
                                                    //}


        //TextAsset text = Resources.Load("Option") as TextAsset;
        //StringReader str = new StringReader(text.text);
        // 이렇게 되면 파일이 있다면 stringReader가 null 이 아니게 되고
        // for, while문을 돌리면서 str.ReadLine(); 으로 한줄씩 스트링을 받을 수 있다. 
    }

    public void ReadTxtFile()       // 파일이 이미 있는 경우 사용해서 가져오기. 
    {
        List<string> _textNum = new List<string>();     // 리스트에 저장하기 위해서
        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/Option.txt");
        for (int n = 0; n <= sr.Peek(); n++)
        {
            string s = sr.ReadLine().ToString();    // 첫번째 라인을 읽어서 문자열로 변환
            //string[] ss = s.Split("()", System.StringSplitOptions.None); // "" 기준으로 문자열 분리
            _textNum.Add(s);        // ss[n]하면 오류뜸
            //Debug.Log(s);
        }
        // 해상도 가져오기 
        string[] num1 = _textNum[0].Split("("); // num1로 첫번째줄 가져오기
        //num1[1].TrimEnd('(', ',');      // 640,480) 이거에서 Trim으로 ) , 이거 두가지 잘라줘서 640480이렇게만 나오게 함. 이거 아님 Replace로 해야함
        string temp = "[),]";
        num1[1] = Regex.Replace(num1[1], temp, "");     // num1[1]에 있는 temp값들을 전부 공백으로 바꿔줌
        temp = "[ ]";
        num1[1] = Regex.Replace(num1[1], temp, "");     // 공백 제거하기 
        Debug.Log(num1[1]);
        Debug.Log(num1[1].Length);          // 6자리
        Debug.Log(num1[1].Substring(0, 3));  // 640
        Debug.Log(num1[1].Substring(3, 3));  // 480
        if (num1[1].Length == 6)     // 6자리인 경우
        {
            // 여기에 
            _saveresolx = int.Parse(num1[1].Substring(0, 3));
            _saveresoly = int.Parse(num1[1].Substring(3, 3));
        }
        if (num1[1].Length == 7)    // 7자리인 경우        
        {
            _saveresolx = int.Parse(num1[1].Substring(0, 4));
            _saveresoly = int.Parse(num1[1].Substring(4, 3));
        }
        if (num1[1].Length == 8)    // 8자리인 경우
        {
            _saveresolx = int.Parse(num1[1].Substring(0, 4));
            _saveresoly = int.Parse(num1[1].Substring(4, 4));
        }

        // BGM 가져오기
        num1 = _textNum[1].Split("(", System.StringSplitOptions.None);
        temp = "[)]";
        num1[1] = Regex.Replace(num1[1], temp, "");     // num1[1]에 있는 temp값들을 전부 공백으로 바꿔줌
        temp = "[ ]";
        num1[1] = Regex.Replace(num1[1], temp, "");
        _bgm = int.Parse(num1[1]);
        Debug.Log(_bgm);

        // 효과음 가져오기
        num1 = _textNum[2].Split("(", System.StringSplitOptions.None);
        temp = "[)]";
        num1[1] = Regex.Replace(num1[1], temp, "");     // 가로 지우기
        temp = "[ ]";
        num1[1] = Regex.Replace(num1[1], temp, "");     // 공백 지우기
        _effect = int.Parse(num1[1]);
        Debug.Log(_effect);
        // Split으로 "(" 로 먼저 자르면 첫번쨰 줄인 경우 [0] Resolutions( [1]640,480) 이렇게 나오나 
        //string.TrimEnd('내용'); 이걸로 , ) 이거 두개 자르고 string 총 길이가 567(0부터니까) 중에 한개니까 5인경우 
        // 0~2까지 width로 하고 3~5까지 height로 하고 6인경우 width가 0~3 4~6이 height 이런식으로 쪼개준 후 넣어주면 맞을 것 같다.
        // 
        //_textNum[0].LastIndexOf(1.ToString());
        /* Debug.Log(_textNum.Count);      // 이거는 정상적으로 3개 저장했다고 나옴
         //Debug.Log(_textNum[0].Substring(_textNum[0].IndexOf('(') + 1, _textNum[0].LastIndexOf(')', _textNum[0].Length)));
         Debug.Log(_textNum[0].Substring(_textNum[0].IndexOf('(') + 1, 3));      // 640
         string[] s1234 = _textNum[0].Split("(", System.StringSplitOptions.None);
         Debug.Log(s1234[1]);        // 이렇게 하면 640,480) 이렇게 나옴 여기서 TrimEnd(')', ','); 해줘서 ,랑 )지우고 
         // 위에 지운 값을 따로 저장해서 이게 길이가 총 5,6,7에 따라서 다르게 받으면 되자너 어 ? 거의 다 했는데 ㄹㅇ 
         //string num = _textNum[0].Substring(_textNum[0].IndexOf('(') + 1, _textNum[0].Length);
         //Debug.Log(num);
         Debug.Log(_textNum[0].Substring(_textNum[0].IndexOf(',') + 1, 4));      // 480
         // 이렇게 나오긴 하는데 이게 만약 4인 경우도 자연스럽게 나와야 하는데 그걸 해결을 못해버린... 이러면 
         Debug.Log(_textNum[1].Substring(_textNum[1].IndexOf('(') + 1, 1));      // 이렇게 읽어오는데 문제는 
         Debug.Log(_textNum[1].Substring(_textNum[2].IndexOf('(') + 1, 1));*/      // 뒤에 1을 하거나 3, 4를 안하고 변수를 써야하는게 맞는데 이걸 못하겠네

        //Debug.Log(_textNum[1].Substring(_textNum[1].IndexOf('(') + 1, _textNum[0].IndexOf(')') - 1));
        // 잠깐만 이거 3~4 할거면 걍 int num2 = IndexOf('('); int num1 = IndexOf(')'); 이렇게 하고 이거 두개 빼기 해서 
        // 나머지가 3 혹은 4이면 그거에 맞게 3,4를 넣어주면 되지 않나 어라 그럴듯한데 ㄹㅇ
        //Debug.Log(_textNum[0].Substring(0, _textNum[0].Length));  이건 Resolusions(720, 480)이 들어갔는데
        //이게 들어갔다는건 여기서 분리만 해주면 끝인데 왜 
        /*for(int n = 0; n < _textNum.Count; n++)
        {
            Debug.Log(_textNum[n].Substring(_textNum[n].IndexOf("("), _textNum[n].IndexOf(9.ToString())));
        }*/
        /*
         {
             string s = sr.ReadLine().ToString();
             _textNum.Add(s);
             string[] ss = s.Split(",", System.StringSplitOptions.None);
         }
         for(int n = 0; n < _textNum.Count; n++)
         {
             //_textNum[n].Split(",", System.StringSplitOptions.None);

         }*/
        // 이렇게 s로 한줄씩 출력은 했고 
        // 일단 3줄까지 할테니까 나오긴 다 나옴 근데 
        sr.Close();
    }
}