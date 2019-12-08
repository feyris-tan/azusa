namespace moe.yo3explorer.azusa.dex.Schema.Enums
{
    public enum SessionState : byte
    {
        Removed = 1, 
        Expired = 2, 
        ResidualDeviation = 3,
        CountsDeviation = 4, 
        SecondSession = 5, 
        OffTimeLoss = 6,
        Started = 7, 
        BadTransmitter = 8,
        ManufacturingMode = 9
    }
}