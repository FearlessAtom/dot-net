namespace CLassLibrary;

public class AccountEventArgs
{
    public AccountEventArgs(string Message, decimal Sum, string Receiver, object Object)
    {
        this.Message = Message;
        this.Sum = Sum;
        this.Receiver = Receiver;
        this.Object = Object;
    }

    public string? Message { get; set; } = string.Empty;
    public decimal? Sum { get; set; }
    public string? Receiver { get; set; } = string.Empty;
    public object? Object{ get; set; }
}