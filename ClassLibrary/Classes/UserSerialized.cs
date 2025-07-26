using System.Runtime.Serialization;

namespace ClassLibrary.Classes
{
    [DataContract]
    public class UserSerialized
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Username { get; set; }
    }
}