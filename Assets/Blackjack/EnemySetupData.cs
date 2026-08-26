using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create EnemySetupData", fileName = "EnemySetupData", order = 0)]
public class EnemySetupData : ScriptableObject
{
    public string enemyName;
    public List<Sprite> sprites;
    public DeckBase deck;
    public DeckBase dealerDeck;
}