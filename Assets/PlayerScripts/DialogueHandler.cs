using TMPro;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private string CharName;
    [SerializeField] private TMP_Text DisplayText;
    private int TextNum;

    

    public void StartTalk()
    {
        TextNum = 0;
        Debug.Log("YAP YAP");
    }

    private void UpdateText(TMP_Text text, string character)
    {

    }

}
