namespace OnPoint.Universal
{
    public enum ExecutionResult
    {
        NoOp = 0,
        Success = 1,
        Failure = 2,
        NoResult = 3,
    }


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


    public enum IconFlipOrientation
    {
        Normal = 0,
        Horizontal = 1,
        Vertical = 2,
        Both = 3
    }
}