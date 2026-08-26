using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    private int[] BarButtons = new int[15] {2, 2, 2, 2, 2, 3, 1, 3, 3, 1, 2, 1, 1, 3, 2};

    [SerializeField] private InputActionAsset inputActions;

    [SerializeField] private Canvas TextCanv;

    [SerializeField] private GameObject Button1;
    [SerializeField] private GameObject Button2;
    [SerializeField] private GameObject Button3;

    [SerializeField] private string CharName;
    [SerializeField] private string DisplayText;
    [SerializeField] private string ButtonText;
    [SerializeField] private TMP_Text TextOBJ;

    private int TextNum;
    private int ButtNum;
    private int TextCharCount;
    private int TextLength;

    private string SubText;
    private string PartText;

    private bool doingText;

    private void Start()
    {
        ResetButtonClick();
        TextCanv.gameObject.SetActive(false);
    }

    private void ResetButtonClick()
    {
        Button1.TryGetComponent<Button>(out Button butt1);
        butt1.onClick.RemoveAllListeners();
        butt1.onClick.AddListener(ProgressText);
        Button2.TryGetComponent<Button>(out Button butt2);
        butt2.onClick.RemoveAllListeners();
        butt2.onClick.AddListener(ProgressText);
        Button3.TryGetComponent<Button>(out Button butt3);
        butt3.onClick.RemoveAllListeners();
        butt3.onClick.AddListener(ProgressText);
    }

    public void StartTalk()
    {
        TextNum = 0;
        ButtNum = 0;
        Debug.Log("YAP YAP");        
        UpdateText();
    }

    public void ProgressText()
    {
        TextCharCount = 0;
        TextNum++;
        PartText = "";
        UpdateText();
    }

    private void UpdateText()
    {
        Button1.SetActive(false);
        Button2.SetActive(false);
        Button3.SetActive(false);
        string[] SplitTex = DisplayText.Split(':');
        if (TextNum < SplitTex.Length)
        {
            SubText = SplitTex[TextNum];
            TextLength = SubText.Length;
            TextCharCount = 0;
            doingText = true;
            TextCanv.gameObject.SetActive(true);
        }
        else 
        {
            Debug.Log("OUT OF RANGE/NO MORE TEXT");
            CloseText();
        }

        string[] SplitButt = ButtonText.Split(":");
        int ButtCount = 0;
        if(ButtNum < SplitButt.Length && CharName == "Bar")
        {
            Button1.GetComponentInChildren<TMP_Text>().SetText(SplitButt[ButtNum]);
            ButtNum++;
            ButtCount++;
            if (ButtCount < BarButtons[TextNum] && ButtNum < SplitButt.Length)
            {
                Button2.GetComponentInChildren<TMP_Text>().SetText(SplitButt[ButtNum]);
                ButtCount++;
                ButtNum++;
            }
            else
            {
                Debug.Log("No button 2, skipping");
            }
            if (ButtCount < BarButtons[TextNum] && ButtNum < SplitButt.Length)
            {
                Button3.GetComponentInChildren<TMP_Text>().SetText(SplitButt[ButtNum]);
                ButtCount++;
                ButtNum++;
            }
            else
            {
                Debug.Log("No button 3, skipping");
            }

            if (ButtNum == 8)//count number of button texts starting from 1 to get this accurate
            {
                Button1.TryGetComponent<Button>(out Button butt1);
                butt1.onClick.RemoveAllListeners();
                butt1.onClick.AddListener(StartTutorial);
                Button2.TryGetComponent<Button>(out Button butt2);
                butt2.onClick.RemoveAllListeners();
                butt2.onClick.AddListener(SkipTutorial);
            }
            else if (ButtNum == 10)//count number of button texts starting from 1 to get this accurate
            {
                Button2.TryGetComponent<Button>(out Button butt2);
                butt2.onClick.RemoveAllListeners();
                butt2.onClick.AddListener(ProgressText);
            }
            else if (ButtNum == 13)//count number of button texts starting from 1 to get this accurate
            {
                ResetButtonClick(); 
            }
            else if (ButtNum == 17)//count number of button texts starting from 1 to get this accurate
            {
                Button1.TryGetComponent<Button>(out Button butt1);
                butt1.onClick.RemoveAllListeners();
                butt1.onClick.AddListener(RetryDeal);
                Button2.TryGetComponent<Button>(out Button butt2);
                butt2.onClick.RemoveAllListeners();
                butt2.onClick.AddListener(RetryDeal);
                Button3.TryGetComponent<Button>(out Button butt3);
                butt3.onClick.RemoveAllListeners();
                butt3.onClick.AddListener(DoDeal);
            }
        }
    }

    private void StartTutorial()
    {
        Debug.Log("DOING TUTORIAL AAAAAA");
        ButtNum = 8;
        TextNum = 3;
        ProgressText();
    }
    private void RetryDeal()
    {
        Debug.Log("WRONG!");
        ButtNum = 14;
        TextNum = 7;
        ProgressText();
    }

    private void SkipTutorial()
    {
        Debug.Log("SKIPPED");
        ProgressText();
        ProgressText();
    }
    private void DoDeal()
    {
        ProgressText();
        if (TextNum != 9)
        {
            ButtNum = ButtNum - 3;
            ProgressText();
        }
        ResetButtonClick();
    }

    private void CloseText()
    {
        doingText = false;
        inputActions.FindActionMap("Player").Enable();
        inputActions.FindActionMap("UI").Disable();
        TextCanv.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (doingText)
        {
            if (TextCharCount < TextLength)
            {
                //Debug.Log(TextLength);
                char newchar = SubText[TextCharCount];
                PartText += newchar;

                TextOBJ.SetText(PartText);
                TextCharCount++;
            }
            else if (TextCharCount == TextLength)
            {
                int ButtonSpawnCount = 0;
                if (CharName =="Bar")
                {
                    ButtonSpawnCount = BarButtons[TextNum];
                }

                Button1.SetActive(true);
                if (ButtonSpawnCount == 3)
                {
                    Button2.SetActive(true);
                    Button3.SetActive(true);
                }
                if (ButtonSpawnCount == 2)
                {
                    Button2.SetActive(true);
                }
                TextCharCount++;

            }
        }
    }

}
