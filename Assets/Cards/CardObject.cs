using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class CardObject : MonoBehaviour
{
    [SerializeField] private BaseCardInfo baseCardInfo;

    [SerializeField] private RawImage image;
    [SerializeField] private TMP_Text[] ranks;

    public void SetCardInfo(CardInfo cardInfo)
    {
        image.texture = cardInfo.texture;

        foreach (var rank in ranks) {
            rank.color = cardInfo.GetColour();
            rank.text = cardInfo.rankName;
        }
    }
}
