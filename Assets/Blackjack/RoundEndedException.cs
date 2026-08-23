using System;

public class RoundEndedException : Exception { }

public class BothStandingException : RoundEndedException { }

public class BustException : RoundEndedException
{
    public readonly PlayerData BustPlayer;
    private string _message;

    public BustException(PlayerData bustPlayer)
    {
        this.BustPlayer = bustPlayer;
    }
}

public class BlackjackException : RoundEndedException { }