using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HandData", menuName = "Scriptable Objects/HandData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private List<CardInfo> hand;
    
    public List<CardInfo> Hand => hand;
}
