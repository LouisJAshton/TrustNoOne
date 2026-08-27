using Combat.UI;
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
                LogManager.Instance.Log(new LogData("Neither player has bested me, but you have the higher score. 1 point.", "Dealer"));
            }
            else if (p2Score > p1Score) {
                LogManager.Instance.Log(new LogData("Neither player has bested me, and you have the lower score. -1 point.", "Dealer"));
                overallScore = -1;
            }
        }
        
        //Both players beat the dealer
        else if (dealerScore < p2Score && dealerScore < p1Score) {
            if (p1Score > p2Score) {
                LogManager.Instance.Log(new LogData("You've both bested me this round but it is you who takes this round. 2 points.", "Dealer"));
                overallScore = 2;
            }
            else if (p2Score > p1Score) {
                LogManager.Instance.Log(new LogData("You've both exceeded my score, but your opponent exceeds yours. -2 points.", "Dealer"));
                overallScore = -2;
            }
        }
        
        //Only 1 player beats the dealer
        else if ((dealerScore >= p2Score && dealerScore < p1Score) || (dealerScore < p2Score && dealerScore >= p1Score)) {
            if (p1Score > p2Score) {
                LogManager.Instance.Log(new LogData("Only you had the hand to beat mine. 3 points.", "Dealer"));
                overallScore = 3;
            }
            else if (p2Score > p1Score) {
                LogManager.Instance.Log(new LogData("Your opponent trumps my hand and yours. -3 points.", "Dealer"));
                overallScore = -3;
            }
        }
        else if (overallScore == 0){
            LogManager.Instance.Log(new LogData("A tie. No points.", "Dealer"));
        }

        return UniTask.FromResult(overallScore);
    }
}