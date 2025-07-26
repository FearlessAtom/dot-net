using System.ServiceModel;
using System.Collections.Generic;
using ClassLibrary.Classes;

namespace ClassLibrary
{
    [ServiceContract(CallbackContract = typeof(IServiceChatCallback))]
    public interface IServiceChat
    {
        [OperationContract]
        int Connect(string Username);

        [OperationContract]
        void Disconnect(int Id);

        [OperationContract(IsOneWay = true)]
        void SendMessageGlobal(Message Message);

        [OperationContract(IsOneWay = true)]
        void UpdateUsers();

        [OperationContract]
        string GetUsernameById(int Id);

        [OperationContract]
        List<UserSerialized> GetUsers();
    }

    public interface IServiceChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendMessageGlobalCallback(Message Message);

        [OperationContract(IsOneWay = true)]
        void UpdateUsersCallback(UserSerialized[] Users);
    }
}
