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
    // �ϴ� �׳� �����̶�� �����ϰ�
    // Item Label�� �ؽ�Ʈ���� ���� �ٲ�����ϰ� �̰� �����ϸ� ResolutiuonList�� �ִ� Label �ؽ�Ʈ�� �ٲ���Ѵ�.
    // Ȯ���� ���� ��� ���� Label �ؽ�Ʈ�� �������� �װſ� �°� �ػ󵵸� �����ؾ���.

    // üũ�ڽ��� Ǯ��ũ�� ��� ���� ������. �������� ������ �͵��� ���� �����ϰ� ū ���� ���� List�� �־��ָ� ���� �����ϴ�. 
    // Ǯ ��ũ�� ��带 �־��ٷ��� üũǥ�ø� �Ѱ� �� ����� üũ�� �ߴ����� ���� bool�������� �Ѱ� �����´�.
    // üũ�� �ι� Ŭ���ؼ� �ٽ� ��Ҹ� �� �� ������ �̹��� ������ �ϴ�
    // bool�� �ΰ��� �����;��ϰ� ... ��.. 



    [SerializeField] GameObject _resolutionList;        // Dropdown �������� ���ؼ� 
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _effSlider;
    Dropdown _options;

    List<string> _optionList = new List<string>();

    string _dropdownKey = "DropDownKey";
    int _nowOption;

    // GameInfoDisplay���� �߾���ߴµ�
    public int _bgm;
    public int _effect;
    public int _saveresolx;
    public int _saveresoly;

    public bool _isOpen = false;

    void Awake()
    {
        Instance = this;
        _options = _resolutionList.GetComponent<Dropdown>();
        // ���� �ʱⰪ ����.
        /* SetSliderValue();

         //Debug.Log(File.Exists(Application.dataPath + "/Resources/Option.txt"));     // .txt���� �ؾ��� ã������. �ٵ� �о���°� �̻��ϰ� �ػ� �����ͼ� �ϴ°� �ȵȴ�.
         //Debug.Log(Application.dataPath + "/Resources/Option");
         //Debug.Log(Application.dataPath);
         //Debug.Log(Resources.Load("Option") != null);
         *//*string str = Application.dataPath + "/Resources/Option.txt";
         Debug.Log(str);*/
        // GameInfo���� �ػ� �������°� �����ߴµ� �� �����̴� ����� �־��ִ°� ��������.
        if (File.Exists(Application.dataPath + "/Resources/Option.txt"))     // ������ �ִ� ��� 
        {
            ReadTxtFile();      // �ִ� ��� �о�ͼ� ������ ���ش�. ���⼭ int���ٰ� ���� ���� �־��ְ� 
            SetSliderValue();   // BGM �о�°����� ����. 
            //Screen.SetResolution(_saveresolx, _saveresoly, false);      // ���Ͽ��� ���������� ����.
            //ScreenSizeChange(); // �о�� ������ �ػ� ������ �Ǿ���ϴµ� 
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
        // Ŭ���ؼ� ������� �����ϰ� ���� �� Ȯ���� ������ �� �ػ󵵷� ����Ǿ�� �ϴµ�. Label�� �ִ� �ؽ�Ʈ ���� �����ͼ� �״�� �ٲ��ش�.

        _options.onValueChanged.AddListener(delegate { SetDropDown(_options.value); });
        SetDropDown(1);
    }

    void SetDropDown(int option)
    {
        PlayerPrefs.SetInt(_dropdownKey, option);
        _nowOption = option;    // option ��ȣ ���� 
        Debug.Log("current option : " + option);        // �����ϱ� ���⿡ button�� Ȯ�� ��Ҹ� �־�ּ� Ȯ���� ���  �ٲٰ� ����� ��� �������
    }

    public void ClickYesButton()
    {
        SaveSoundVal();     // ���� ���� 
        // ������ ��ȣ�� GameInfoDisplay�� �˷���� �Ѵ�.
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

    public void SetSliderValue()        // �ɼǹ�ư Ŭ�� �� ���
    {
        _bgmSlider.value = _bgm;
        _effSlider.value = _effect;
    }
    public void ClickYNoButton()
    {
        SaveSoundVal();     // ���� ���� 
        gameObject.SetActive(false);
        _isOpen = false;
    }
    public void SaveSoundVal()
    {
        _bgm = (int)_bgmSlider.value;       // ����� ����
        _effect = (int)_effSlider.value;    // ����� ����.
    }
    public void CreateFile()        // ���� ��� ���ʷ� �����Ѵ�. 
    {
        string str = "Assets/Resources/Option";         // ���� ���  Application.dataPath �̰� ���� // D:/Unity/Myproject/Assets�̷�����
        StreamWriter sw;
        /*if (File.Exists(str))          // ���� �ִ��� Ȯ�� 
        {
            //File.Create("Assets/Resources/Option.txt");     // �ؽ�Ʈ ���� ����
            sw = new StreamWriter(str + ".txt");        // �ؽ�Ʈ ���� ���� 
            sw.WriteLine(sw);
            sw.Flush();
            sw.Close();                               
        }
        else
        {*/
        // �ٵ� �̷��� �ҰŸ� �� if else�� ���� �� �ᵵ �ֵ� ���� �� üũ�� �� �� ������ 
        //sw = new StreamWriter(File.Open("Option", FileMode.Open));
        sw = new StreamWriter(File.Create(str + ".txt"));
        //sw.write(File.WriteAllText(str + ".txt", string.Empty));  // �ؽ�Ʈ ���� ��ü ����
        sw.WriteLine("Resolusions({0}, {1})", _saveresolx, _saveresoly);        // �ػ� ����
        sw.WriteLine("BGMVolume({0})", _bgm);       // ����� ����   
        sw.WriteLine("EFFVolume({0})", _effect);    // ȿ���� ����
        sw.Close();                                 // ���� �ݱ�
                                                    // ��������� �������� �ߴµ� �̰� �����Ѱ� �����;���. �װ� ã�ƾ��ϴµ� 
                                                    //}


        //TextAsset text = Resources.Load("Option") as TextAsset;
        //StringReader str = new StringReader(text.text);
        // �̷��� �Ǹ� ������ �ִٸ� stringReader�� null �� �ƴϰ� �ǰ�
        // for, while���� �����鼭 str.ReadLine(); ���� ���پ� ��Ʈ���� ���� �� �ִ�. 
    }

    public void ReadTxtFile()       // ������ �̹� �ִ� ��� ����ؼ� ��������. 
    {
        List<string> _textNum = new List<string>();     // ����Ʈ�� �����ϱ� ���ؼ�
        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/Option.txt");
        for (int n = 0; n <= sr.Peek(); n++)
        {
            string s = sr.ReadLine().ToString();    // ù��° ������ �о ���ڿ��� ��ȯ
            //string[] ss = s.Split("()", System.StringSplitOptions.None); // "" �������� ���ڿ� �и�
            _textNum.Add(s);        // ss[n]�ϸ� ������
            //Debug.Log(s);
        }
        // �ػ� �������� 
        string[] num1 = _textNum[0].Split("("); // num1�� ù��°�� ��������
        //num1[1].TrimEnd('(', ',');      // 640,480) �̰ſ��� Trim���� ) , �̰� �ΰ��� �߶��༭ 640480�̷��Ը� ������ ��. �̰� �ƴ� Replace�� �ؾ���
        string temp = "[),]";
        num1[1] = Regex.Replace(num1[1], temp, "");     // num1[1]�� �ִ� temp������ ���� �������� �ٲ���
        temp = "[ ]";
        num1[1] = Regex.Replace(num1[1], temp, "");     // ���� �����ϱ� 
        Debug.Log(num1[1]);
        Debug.Log(num1[1].Length);          // 6�ڸ�
        Debug.Log(num1[1].Substring(0, 3));  // 640
        Debug.Log(num1[1].Substring(3, 3));  // 480
        if (num1[1].Length == 6)     // 6�ڸ��� ���
        {
            // ���⿡ 
            _saveresolx = int.Parse(num1[1].Substring(0, 3));
            _saveresoly = int.Parse(num1[1].Substring(3, 3));
        }
        if (num1[1].Length == 7)    // 7�ڸ��� ���        
        {
            _saveresolx = int.Parse(num1[1].Substring(0, 4));
            _saveresoly = int.Parse(num1[1].Substring(4, 3));
        }
        if (num1[1].Length == 8)    // 8�ڸ��� ���
        {
            _saveresolx = int.Parse(num1[1].Substring(0, 4));
            _saveresoly = int.Parse(num1[1].Substring(4, 4));
        }

        // BGM ��������
        num1 = _textNum[1].Split("(", System.StringSplitOptions.None);
        temp = "[)]";
        num1[1] = Regex.Replace(num1[1], temp, "");     // num1[1]�� �ִ� temp������ ���� �������� �ٲ���
        temp = "[ ]";
        num1[1] = Regex.Replace(num1[1], temp, "");
        _bgm = int.Parse(num1[1]);
        Debug.Log(_bgm);

        // ȿ���� ��������
        num1 = _textNum[2].Split("(", System.StringSplitOptions.None);
        temp = "[)]";
        num1[1] = Regex.Replace(num1[1], temp, "");     // ���� �����
        temp = "[ ]";
        num1[1] = Regex.Replace(num1[1], temp, "");     // ���� �����
        _effect = int.Parse(num1[1]);
        Debug.Log(_effect);
        // Split���� "(" �� ���� �ڸ��� ù���� ���� ��� [0] Resolutions( [1]640,480) �̷��� ������ 
        //string.TrimEnd('����'); �̰ɷ� , ) �̰� �ΰ� �ڸ��� string �� ���̰� 567(0���ʹϱ�) �߿� �Ѱ��ϱ� 5�ΰ�� 
        // 0~2���� width�� �ϰ� 3~5���� height�� �ϰ� 6�ΰ�� width�� 0~3 4~6�� height �̷������� �ɰ��� �� �־��ָ� ���� �� ����.
        // 
        //_textNum[0].LastIndexOf(1.ToString());
        /* Debug.Log(_textNum.Count);      // �̰Ŵ� ���������� 3�� �����ߴٰ� ����
         //Debug.Log(_textNum[0].Substring(_textNum[0].IndexOf('(') + 1, _textNum[0].LastIndexOf(')', _textNum[0].Length)));
         Debug.Log(_textNum[0].Substring(_textNum[0].IndexOf('(') + 1, 3));      // 640
         string[] s1234 = _textNum[0].Split("(", System.StringSplitOptions.None);
         Debug.Log(s1234[1]);        // �̷��� �ϸ� 640,480) �̷��� ���� ���⼭ TrimEnd(')', ','); ���༭ ,�� )����� 
         // ���� ���� ���� ���� �����ؼ� �̰� ���̰� �� 5,6,7�� ���� �ٸ��� ������ ���ڳ� �� ? ���� �� �ߴµ� ���� 
         //string num = _textNum[0].Substring(_textNum[0].IndexOf('(') + 1, _textNum[0].Length);
         //Debug.Log(num);
         Debug.Log(_textNum[0].Substring(_textNum[0].IndexOf(',') + 1, 4));      // 480
         // �̷��� ������ �ϴµ� �̰� ���� 4�� ��쵵 �ڿ������� ���;� �ϴµ� �װ� �ذ��� ���ع���... �̷��� 
         Debug.Log(_textNum[1].Substring(_textNum[1].IndexOf('(') + 1, 1));      // �̷��� �о���µ� ������ 
         Debug.Log(_textNum[1].Substring(_textNum[2].IndexOf('(') + 1, 1));*/      // �ڿ� 1�� �ϰų� 3, 4�� ���ϰ� ������ ����ϴ°� �´µ� �̰� ���ϰڳ�

        //Debug.Log(_textNum[1].Substring(_textNum[1].IndexOf('(') + 1, _textNum[0].IndexOf(')') - 1));
        // ��� �̰� 3~4 �ҰŸ� �� int num2 = IndexOf('('); int num1 = IndexOf(')'); �̷��� �ϰ� �̰� �ΰ� ���� �ؼ� 
        // �������� 3 Ȥ�� 4�̸� �װſ� �°� 3,4�� �־��ָ� ���� �ʳ� ��� �׷����ѵ� ����
        //Debug.Log(_textNum[0].Substring(0, _textNum[0].Length));  �̰� Resolusions(720, 480)�� ���µ�
        //�̰� ���ٴ°� ���⼭ �и��� ���ָ� ���ε� �� 
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
        // �̷��� s�� ���پ� ����� �߰� 
        // �ϴ� 3�ٱ��� ���״ϱ� ������ �� ���� �ٵ� 
        sr.Close();
    }
}