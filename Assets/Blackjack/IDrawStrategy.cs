using System.Collections.Generic;

public interface IDrawStrategy
{
    public CardInfo Draw(List<CardInfo> deck);
}