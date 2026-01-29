namespace AbsoluteCinema.Requests
{
    /// <summary>
    /// Request body for attaching existing media
    /// </summary>
    /// <param name="MediaId"></param>
    public record AttachMediaRequest(Guid mediaId);
}
