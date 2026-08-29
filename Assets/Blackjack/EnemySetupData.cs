using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create EnemySetupData", fileName = "EnemySetupData", order = 0)]
public class EnemySetupData : ScriptableObject
{
    public string enemyName;
    public Sprite sprite;
    public DeckBase deck;
    public DeckBase dealerDeck;
    
    public DialogueHandler.CharacterName characterName;
}