using System;
using UnityEngine;
using UnityEngine.UI;

public class TugOfWarBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        // GameManager.Instance.blackjackManager.OnScoreChange.AddListener(arg0 => slider.value = 0.4f);
        GameManager.Instance.blackjackManager.OnScoreChange.AddListener(arg0 => slider.value = (float)arg0 / (float)BlackjackManager.MAX_SCORE);
    }
}
