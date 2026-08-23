using Cysharp.Threading.Tasks;

public interface ITurnStrategy
{
    public UniTask TakeTurn();
}