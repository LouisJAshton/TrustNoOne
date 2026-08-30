using System;
using Eflatun.SceneReference;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private SceneReference gameWonScene;
    
    private int[] BarButtons = new int[16] {2, 2, 2, 2, 2, 3, 1, 3, 3, 1, 2, 1, 1, 3, 2, 1};

    private int[] AgaButtons = new int[16] {3, 3, 1, 2, 1, 1, 2, 1, 3, 1, 2, 2, 2, 1, 1, 1};

    private int[] LanButtons = new int[12] {2, 3, 3, 2, 1, 1, 3, 1, 1, 2, 1, 1};
    
    [SerializeField] private CombatTrigger combatTrigger;

    [SerializeField] private InputActionAsset inputActions;

    [SerializeField] private Canvas TextCanv;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject LanceBlock;
    [SerializeField] private SpriteRenderer AgaliaSprite;
    [SerializeField] private Collider AgaliaCollider;

    [SerializeField] private GameObject Button1;
    [SerializeField] private GameObject Button2;
    [SerializeField] private GameObject Button3;

    [SerializeField] private CharacterName CharName;
    [SerializeField, TextArea(5, 100)] private string DisplayText;
    [SerializeField, TextArea(5, 100)] private string ButtonText;
    [SerializeField] private TMP_Text TextOBJ;

    private int TextNum;
    private int ButtNum;
    private int TextCharCount;
    private int TextLength;

    private string SubText;
    private string PartText;

    private bool doingText;
    private bool tutorial;

    [SerializeField] private GameObject BarMusicOBJ;
    [SerializeField] private GameObject BattleMusicOBJ;

    [SerializeField] private AudioClip OtisBattleTheme;
    [SerializeField] private AudioClip AgaliaBattleTheme;
    [SerializeField] private AudioClip LanceBattleTheme;

    [SerializeField] private AudioClip WinTheme;
    [SerializeField] private AudioClip LossTheme;

    #region Round Over Event Handling

    [SerializeField] private RoundOverEvent roundOverEvent;
    
    private void OnEnable() => roundOverEvent?.Subscribe(OnRoundWon);
    private void OnDisable()
    {
        Cursor.visible = false;
        roundOverEvent?.Unsubscribe(OnRoundWon);
    }

    private void OnRoundWon(RoundOverEventData data)
    {
        //If the round was not played against me, ignore this callback
        if (data.CharacterName != CharName)
            return;

        BarMusicOBJ.SetActive(true);
        BattleMusicOBJ.SetActive(false);

        if (data.WasWon) {
            MainAudio.instance.PlaySFXClip(WinTheme, transform, 1);
            print($"Player won against {CharName}");
            if (CharName == CharacterName.Bar && tutorial)
            {
                ToggleCanvas();
                ProgressText();
            }
            else if (CharName == CharacterName.Bar && !tutorial)
            {
                ButtNum = 18;
                TextNum = 9;
                
                AgaliaCollider.enabled = true;
                AgaliaSprite.enabled = true;
                
                ToggleCanvas();
                ProgressText();
            }

            else if (CharName == CharacterName.Agalia)
            {
                ButtNum = 24;
                TextNum = 12;
                ToggleCanvas();
                ProgressText();
            }
            else if (CharName == CharacterName.Lance)
            {
                ButtNum = 11;
                TextNum = 4;
                ToggleCanvas();
                ProgressText();

                SceneManager.LoadScene(gameWonScene.BuildIndex, LoadSceneMode.Single);
            }
        }
        else {
            print($"{CharName} won");
            MainAudio.instance.PlaySFXClip(LossTheme, transform, 1);
            if (CharName == CharacterName.Bar && tutorial) 
            {
                ToggleCanvas();
                ProgressText();
            }
            else if (CharName == CharacterName.Bar && !tutorial)
            {
                ButtNum = 27;
                TextNum = 14;
                ToggleCanvas();
                ProgressText();
            }

            else if (CharName == CharacterName.Agalia)
            {
                ButtNum = 26;
                TextNum = 14;
                ToggleCanvas();
                ProgressText();
            }
            else if (CharName == CharacterName.Lance)
            {
                ButtNum = 20;
                TextNum = 10;
                ToggleCanvas();
                ProgressText();
            }
        }
    }
    #endregion

    public enum CharacterName
    {
        Bar,
        Agalia,
        Lance
    }

    private void Start()
    {
        BarMusicOBJ?.SetActive(true);
        BattleMusicOBJ?.SetActive(false);
        ResetButtonClick();
        TextCanv.gameObject.SetActive(false);
    }

    private void ResetButtonClick()
    {
        Button1.TryGetComponent<Button>(out Button butt1);
        butt1.onClick.RemoveAllListeners();
        butt1.onClick.AddListener(gameObject.GetComponent<DialogueHandler>().ProgressText);
        Button2.TryGetComponent<Button>(out Button butt2);
        butt2.onClick.RemoveAllListeners();
        butt2.onClick.AddListener(gameObject.GetComponent<DialogueHandler>().ProgressText);
        Button3.TryGetComponent<Button>(out Button butt3);
        butt3.onClick.RemoveAllListeners();
        butt3.onClick.AddListener(gameObject.GetComponent<DialogueHandler>().ProgressText);
    }

    private void UpdateMusic()
    {
        if (CharName == CharacterName.Bar)
        {
            BattleMusicOBJ.GetComponent<AudioSource>().generator = OtisBattleTheme;
        }
        if (CharName == CharacterName.Agalia)
        {
            BattleMusicOBJ.GetComponent<AudioSource>().generator = AgaliaBattleTheme;
        }
        if (CharName == CharacterName.Lance)
        {
            BattleMusicOBJ.GetComponent<AudioSource>().generator = LanceBattleTheme;
        }

        BattleMusicOBJ.SetActive(true);
        BarMusicOBJ.SetActive(false);
    }

    public void StartTalk()
    {
        ResetButtonClick();
        TextNum = 0;
        ButtNum = 0;
        Debug.Log("YAP YAP");        
        UpdateText();
    }

    public void ProgressText()
    {
        Debug.Log(gameObject);
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
        string[] SplitTex = DisplayText.Split(';');
        if (TextNum < SplitTex.Length)
        {
            SubText = SplitTex[TextNum];
            TextLength = SubText.Length;
            TextCharCount = 0;
            
            TextCanv.gameObject.SetActive(true);
        }
        else 
        {
            Debug.Log("OUT OF RANGE/NO MORE TEXT");
            CloseText();
        }

        string[] SplitButt = ButtonText.Split(";");
        int ButtCount = 0;
        if(ButtNum < SplitButt.Length && CharName == CharacterName.Bar)
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
                butt2.onClick.AddListener(Skip);
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
            else if (ButtNum == 27)//count number of button texts starting from 1 to get this accurate
            {
                Button1.TryGetComponent<Button>(out Button butt1);
                butt1.onClick.RemoveAllListeners();
                butt1.onClick.AddListener(Skip);
                Button2.TryGetComponent<Button>(out Button butt2);
                butt2.onClick.RemoveAllListeners();
                butt2.onClick.AddListener(Skip);
            }
            else if (ButtNum == 28)//count number of button texts starting from 1 to get this accurate
            {
                Button1.TryGetComponent<Button>(out Button butt1);
                butt1.onClick.RemoveAllListeners();
                butt1.onClick.AddListener(StartBattleOtis);
            }
        }



        else if (ButtNum < SplitButt.Length && CharName == CharacterName.Agalia)
        {
            Button1.GetComponentInChildren<TMP_Text>().SetText(SplitButt[ButtNum]);
            ButtNum++;
            ButtCount++;
            if (ButtCount < AgaButtons[TextNum] && ButtNum < SplitButt.Length)
            {
                Button2.GetComponentInChildren<TMP_Text>().SetText(SplitButt[ButtNum]);
                ButtCount++;
                ButtNum++;
            }
            else
            {
                Debug.Log("No button 2, skipping");
            }
            if (ButtCount < AgaButtons[TextNum] && ButtNum < SplitButt.Length)
            {
                Button3.GetComponentInChildren<TMP_Text>().SetText(SplitButt[ButtNum]);
                ButtCount++;
                ButtNum++;
            }
            else
            {
                Debug.Log("No button 3, skipping");
            }
            if (ButtNum == 24)//count number of button texts starting from 1 to get this accurate
            {
                Button1.TryGetComponent<Button>(out Button butt1);
                butt1.onClick.RemoveAllListeners();
                butt1.onClick.AddListener(StartBattleAgalia);
                Button2.TryGetComponent<Button>(out Button butt2);
                butt2.onClick.RemoveAllListeners();
                butt2.onClick.AddListener(StartBattleAgalia);
            }

            if (ButtNum == 26)//count number of button texts starting from 1 to get this accurate
            {
                Button1.TryGetComponent<Button>(out Button butt1);
                butt1.onClick.RemoveAllListeners();
                butt1.onClick.AddListener(Skip);
                LanceBlock.SetActive(false);
            }

            if (ButtNum == 27)//count number of button texts starting from 1 to get this accurate
            {
                Button1.TryGetComponent<Button>(out Button butt1);
                butt1.onClick.RemoveAllListeners();
                butt1.onClick.AddListener(StartBattleAgalia);
            }

        }



        else if (ButtNum < SplitButt.Length && CharName == CharacterName.Lance)
        {
            Button1.GetComponentInChildren<TMP_Text>().SetText(SplitButt[ButtNum]);
            ButtNum++;
            ButtCount++;
            if (ButtCount < LanButtons[TextNum] && ButtNum < SplitButt.Length)
            {
                Button2.GetComponentInChildren<TMP_Text>().SetText(SplitButt[ButtNum]);
                ButtCount++;
                ButtNum++;
            }
            else
            {
                Debug.Log("No button 2, skipping");
            }
            if (ButtCount < LanButtons[TextNum] && ButtNum < SplitButt.Length)
            {
                Button3.GetComponentInChildren<TMP_Text>().SetText(SplitButt[ButtNum]);
                ButtCount++;
                ButtNum++;
            }
            else
            {
                Debug.Log("No button 3, skipping");
            }

            if (ButtNum == 11)//count number of button texts starting from 1 to get this accurate
            {
                Button1.TryGetComponent<Button>(out Button butt1);
                butt1.onClick.RemoveAllListeners();
                butt1.onClick.AddListener(StartBattleLance);
            }

            if (ButtNum == 20)//count number of button texts starting from 1 to get this accurate
            {
                Button1.TryGetComponent<Button>(out Button butt1);
                butt1.onClick.RemoveAllListeners();
                butt1.onClick.AddListener(StartBattleAgalia);
            }

            if (ButtNum == 21)//count number of button texts starting from 1 to get this accurate
            {
                Button1.TryGetComponent<Button>(out Button butt1);
                butt1.onClick.RemoveAllListeners();
                butt1.onClick.AddListener(StartBattleLance);
            }

        }
        doingText = true;
    }

    private void ToggleCanvas()
    {
        bool active = TextCanv.gameObject.activeSelf;
        TextCanv.gameObject.SetActive(!active);
    }
    private void Skip()
    {
        Debug.Log("SKIPPED");
        ProgressText();
        ProgressText();
    }
    private void RetryDeal()
    {
        Debug.Log("WRONG!");
        ButtNum = 14;
        TextNum = 7;
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
        Button1.TryGetComponent<Button>(out Button butt1);
        butt1.onClick.RemoveAllListeners();
        butt1.onClick.AddListener(StartBattleOtis);
    }
    private void StartTutorial()
    {
        tutorial = true;
        UpdateMusic();
        combatTrigger.Trigger();
        Debug.Log("DOING TUTORIAL AAAAAA");
        ButtNum = 8;
        TextNum = 3;
        ToggleCanvas();
    }
    private void StartBattleOtis()
    {
        tutorial = false;
        UpdateMusic();
        ToggleCanvas();
        combatTrigger.Trigger();
        Debug.Log("BATTLE WITH OTIS");
        ResetButtonClick();
    }
    private void StartBattleAgalia()
    {
        ToggleCanvas();
        UpdateMusic();
        combatTrigger.Trigger();
        Debug.Log("BATTLE WITH AGALIA");
        ResetButtonClick();
    }
    private void StartBattleLance()
    {
        ToggleCanvas();
        UpdateMusic();
        combatTrigger.Trigger();
        Debug.Log("BATTLE WITH LANCE");
        ResetButtonClick();
    }

    private void CloseText()
    {
        doingText = false;
        Player.GetComponent<PlayerInteract>().istalking = false;
        inputActions.FindActionMap("Player").Enable();
        inputActions.FindActionMap("UI").Disable();
        TextCanv.gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }

    private void Update()
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
                int ButtonSpawnCount = CharName switch
                {
                    CharacterName.Bar => BarButtons[TextNum],
                    CharacterName.Agalia => AgaButtons[TextNum],
                    CharacterName.Lance => LanButtons[TextNum],
                    _ => 0
                };

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
