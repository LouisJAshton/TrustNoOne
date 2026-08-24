using Cysharp.Threading.Tasks;

public interface IScoringStrategy
{
    public UniTask<int> Score();
}

public class WeightedScoringStrategy : IScoringStrategy
{
    private readonly PlayerData _p1;
    private readonly PlayerData _p2;
    private readonly PlayerData _dealer;
    
    public WeightedScoringStrategy(PlayerData p1, PlayerData p2, PlayerData dealer)
    {
        _p1 = p1;
        _p2 = p2;
        _dealer = dealer;
    }

    public UniTask<int> Score()
    {
        int dealerScore = BlackjackManager.CalculateScore(_dealer);
        int p1Score = BlackjackManager.CalculateScore(_p1);
        int p2Score = BlackjackManager.CalculateScore(_p2);

        if (dealerScore > BlackjackManager.MAX)
            dealerScore = 0;
        
        if (p1Score > BlackjackManager.MAX)
            p1Score = 0;
        
        if (p2Score > BlackjackManager.MAX)
            p2Score = 0;

        int overallScore = 0;
        
        //Neither player beats the dealer
        if (dealerScore >= p2Score && dealerScore >= p1Score) {
            if (p1Score > p2Score) {
                overallScore = 1;
            }
            else if (p2Score > p1Score) {
                overallScore = -1;
            }
        }
        
        //Both players beat the dealer
        else if (dealerScore < p2Score && dealerScore < p1Score) {
            if (p1Score > p2Score) {
                overallScore = 2;
            }
            else if (p2Score > p1Score) {
                overallScore = -2;
            }
        }
        
        //Only 1 player beats the dealer
        else if ((dealerScore >= p2Score && dealerScore < p1Score) || (dealerScore < p2Score && dealerScore >= p1Score)) {
            if (p1Score > p2Score) {
                overallScore = 3;
            }
            else if (p2Score > p1Score) {
                overallScore = -3;
            }
        }
        
        return UniTask.FromResult(overallScore);
    }
}