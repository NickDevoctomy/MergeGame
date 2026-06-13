namespace MergeGame.Infrastructure.Config;

/// <summary>Grid dimensions loaded from configuration.</summary>
public sealed class GridConfig
{
    /// <summary>Gets or sets the number of columns.</summary>
    public int Columns { get; set; }

    /// <summary>Gets or sets the number of rows.</summary>
    public int Rows { get; set; }
}
