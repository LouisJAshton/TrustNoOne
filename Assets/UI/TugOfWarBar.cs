using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TugOfWarBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text numberText;

    private void Start()
    {
        GameManager.Instance.blackjackManager.OnScoreChange.AddListener(UpdateSlider);
    }

    private void UpdateSlider(int arg0)
    {
        slider.value = (float)arg0 / (float)BlackjackManager.MAX_SCORE;
        numberText.text = arg0.ToString();
    }
}
