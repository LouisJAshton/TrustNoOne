using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueHandler : MonoBehaviour
{
    private int[] BarButtons = new int[6] {2, 2, 2, 2, 2, 2};

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
        TextCanv.gameObject.SetActive(false);
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

        }
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
