namespace OnPoint.Universal
{
    /// <summary>
    /// Models the result of an action.
    /// </summary>
    public enum ExecutionResult
    {
        NoOp = 0,
        Success = 1,
        Failure = 2,
        NoResult = 3,
    }


    /// <summary>
    /// Models the position of something in a 3x3 grid.
    /// </summary>
    public enum GridPosition : uint
    {
        Unknown = 0,
        TopLeft = 1,
        TopCenter = 2,
        TopRight = 3,
        MidLeft = 4,
        MidCenter = 5,
        MidRight = 6,
        BottomLeft = 7,
        BottomCenter = 8,
        BottomRight = 9,
        TopRow = 10,
        MiddleRow = 11,
        BottomRow = 12,
        LeftColumn = 13,
        MiddleColumn = 14,
        RightColumn = 15,
        Fill = 16,
    }


    /// <summary>
    /// Models the mirroring of an image.
    /// </summary>
    public enum IconFlipOrientation
    {
        Normal = 0,
        Horizontal = 1,
        Vertical = 2,
        Both = 3
    }


    /// <summary>
    /// Models the current state of the app, or window, on the device.
    /// </summary>
    public enum AppDisplayState
    {
        Normal = 0,
        Minimized = 1,
        Maximized = 2,
    }
}