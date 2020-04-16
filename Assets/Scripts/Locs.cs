using System;

public interface ILoc
{
    ILoc[] NextLocs { get; }
}

public class EntranceLoc : ILoc
{
    public ILoc[] NextLocs { get; private set; }

    public EntranceLoc(params ILoc[] nextLocs)
    {
        NextLocs = nextLocs;
    }
}

public class ExitLoc : ILoc
{
    public ILoc[] NextLocs => throw new InvalidOperationException();

    public ExitLoc() { }
}

public class MonsterLoc : ILoc
{
    public ILoc[] NextLocs { get; private set; }

    public MonsterLoc(params ILoc[] nextLocs)
    {
        NextLocs = nextLocs;
    }
}

public class CampLoc : ILoc
{
    public ILoc[] NextLocs { get; private set; }

    public CampLoc(params ILoc[] nextLocs)
    {
        NextLocs = nextLocs;
    }
}
