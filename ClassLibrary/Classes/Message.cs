using System;

namespace ClassLibrary.Classes
{
    public class Message
    {
        public int Sender { get; set; }
        public int? Receiver { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
    }
}
