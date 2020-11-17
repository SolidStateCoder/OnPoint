namespace OnPoint.Universal
{
    public interface IIsChanged
    {
        /// <summary>
        /// Indicates the item has changed since its creation.
        /// </summary>
        bool IsChanged { get; set; }
    }
}