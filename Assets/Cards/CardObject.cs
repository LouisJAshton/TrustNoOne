using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class CardObject : MonoBehaviour
{
    [SerializeField] private BaseCardInfo baseCardInfo;

    [SerializeField] private RawImage image;
    private TMP_Text[] _ranks;

    public void SetCardInfo(CardInfo cardInfo)
    {
        image.texture = cardInfo.texture;

        foreach (var rank in _ranks) {
            rank.color = cardInfo.GetColour();
            rank.text = cardInfo.rankName;
        }
    }
}
