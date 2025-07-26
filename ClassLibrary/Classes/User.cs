using System.ServiceModel;
using System.Runtime.Serialization;

namespace ClassLibrary
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public OperationContext  operation_context { get; set; }
    }
}
