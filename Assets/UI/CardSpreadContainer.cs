using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CardSpreadContainer : MonoBehaviour
{
    private Dictionary<CardObject, Vector3> _cardPositions = new Dictionary<CardObject, Vector3>();

    private const float MAX_SPREAD_ANGLE = 135f;
    private const float MAX_SPREAD_DELTA = 15f;
    
    public void RefreshSpread()
    {
        var updatedCards = new List<CardObject>();
        _cardPositions.Clear();
        foreach (var c in GetComponentsInChildren<CardObject>()) {
            updatedCards.Add(c);
        }

        float spreadDelta = MAX_SPREAD_DELTA;

        if (updatedCards.Count * MAX_SPREAD_DELTA > MAX_SPREAD_ANGLE) {
            spreadDelta = MAX_SPREAD_ANGLE / updatedCards.Count;
        }

        for (var i = 0; i < updatedCards.Count; i++) {
            var c = updatedCards[i];

            var col = i - updatedCards.Count / 2.0f + 0.5f;
            _cardPositions.Add(c, Quaternion.AngleAxis(col * spreadDelta, Vector3.back) * Vector3.up);
        }
    }

    private void Update()
    {
        RefreshSpread();
        
        foreach (var kvp in _cardPositions) {
            kvp.Key.transform.localPosition = Vector3.Lerp(kvp.Key.transform.localPosition, kvp.Value, 0.3f);
        }
    }
}
