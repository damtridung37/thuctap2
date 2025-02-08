namespace WFC
{
    public enum EdgeType
    {
        Wall,   // Solid block, can't connect
        Door,   // Can connect to another Door
        Open,   // Can connect to anything (used for gaps or outdoor areas)
        Empty   // Placeholder for missing data (optional)
    }
}
