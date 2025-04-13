namespace NotificationEngineWorker.Core.Data.Enums;

public enum ProducerType : byte
{
    /// <summary>
    /// Producer is interested in conventional default flow
    /// </summary>
    DefaultFlow,

    /// <summary>
    /// Producer is interested in vision language model + computer vision processing flow
    /// </summary>
    VlmCvFlow
}

