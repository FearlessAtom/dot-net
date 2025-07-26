using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary.Classes;
using System;
using System.Runtime.Remoting.Channels;

namespace ClassLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ServiceChat : IServiceChat
    {
        List<User> users = new List<User>();

        int next_id = 1;

        public int Connect(string Username)
        {
            User user = new User()
            {
                Id = next_id,
                Username = Username,
                operation_context = OperationContext.Current,
            };

            users.Add(user);

            next_id = next_id + 1;
            return user.Id;
        }

        public void Disconnect(int Id)
        {
            User user = users.FirstOrDefault(u => u.Id == Id);

            if (user == null)
            {
                return;
            }

            users.Remove(user);
        }

        public string GetUsernameById(int Id) => users.FirstOrDefault(u => u.Id == Id).Username;

        public List<UserSerialized> GetUsers()
        {   
            List<UserSerialized> serialized_users = new List<UserSerialized>();

            for (int index = 0; index < users.Count(); index++)
            {
                UserSerialized user = new UserSerialized();
                
                user.Id = users[index].Id;
                user.Username = users[index].Username;

                serialized_users.Add(user);
            }

            return serialized_users;
        }

        public void SendMessageGlobal(Message Message)
        {
            Message.Date = DateTime.Now;

            if (Message.Receiver != null)
            {
                User sender = users.FirstOrDefault(u => u.Id == Message.Sender);

                if (sender != null)
                {
                    sender.operation_context.GetCallbackChannel<IServiceChatCallback>().
                    SendMessageGlobalCallback(Message);
                }

                User receiver = users.FirstOrDefault(u => u.Id == Message.Receiver);

                if (receiver != null)
                {
                    receiver.operation_context.GetCallbackChannel<IServiceChatCallback>().
                       SendMessageGlobalCallback(Message);
                }
            }

            else
            {
                for (int index = 0; index < users.Count(); index++)
                {
                    users[index].operation_context.GetCallbackChannel<IServiceChatCallback>().
                        SendMessageGlobalCallback(Message);
                }
            }
        }

        public void UpdateUsers()
        {
            if (users.Count() == 0)
            {
                return;
            }

            for (int index = 0; index < users.Count(); index++)
            {
                users[index].operation_context.GetCallbackChannel<IServiceChatCallback>().
                    UpdateUsersCallback(GetUsers().ToArray());
            }
        }
    }
}
