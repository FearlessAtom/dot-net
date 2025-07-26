using ClassLibrary.Classes;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WindowApplication.ServiceReference1;
using WindowApplication.UserControls;

namespace WindowApplication
{
    public partial class MainWindow : Window, IServiceChatCallback
    {
        public ServiceChatClient client { get; set; }
        public GlobalChat global_chat = new GlobalChat();
        public int? Id { get; set; }

        public UserItem active_user { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ConnectButton.Click += (sender, e) => Connect();
            Closing += (sender, e) => Disconnect();

            global_chat.MessageTextBox.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter) SendMessage();
            };

            global_chat.SendButton.Click += (sender, e) => SendMessage();
        }

        public void Connect()
        {
            if (UsernameTextBox.Text.Length > 16 || UsernameTextBox.Text.Length < 4)
            {
                MessageBox.Show("Username must be in range of 4 and 16 characters!");
                return;
            }

            try
            {
                client = new ServiceChatClient(new InstanceContext(this));

                UserSerialized[] users = client.GetUsers();

                for (int index = 0; index < users.Length; index++)
                {
                    if (users[index].Username == UsernameTextBox.Text)
                    {
                        MessageBox.Show("Username is taken!");
                        return;
                    }
                }
                
                Id = client.Connect(UsernameTextBox.Text);
                MainGrid.Children.Add(global_chat);

                Title = UsernameTextBox.Text;
                client.UpdateUsers();
            }

            catch
            {
                MessageBox.Show("Unable to connect to the server!");
            }
        }

        public void Disconnect()
        {
            if (!Id.HasValue)
            {
                return;
            }
            
            client.Disconnect((int)Id);
            client.UpdateUsers();
            client = null;
        }

        public void SendMessage()
        {
            string content = global_chat.MessageTextBox.Text;

            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            Message Message = new Message()
            {
                Content = content,
                Sender = (int)Id,
                Receiver = active_user == null ? null : (int?)active_user.User.Id,
            };

            client.SendMessageGlobal(Message);

            global_chat.MessageTextBox.Text = string.Empty;
        }

        public void SendMessageGlobalCallback(Message Message)
        {
            MessageItem message_item = new MessageItem(Message, client);

            if (Message.Sender == Id)
            {
                message_item.HorizontalAlignment = HorizontalAlignment.Right;
                message_item.MainBorder.Background = new SolidColorBrush(Colors.LightSteelBlue);
                message_item.MainBorder.CornerRadius = new CornerRadius(15, 15, 0, 15);
            }

            else
            {
                message_item.FlowDirection = FlowDirection.RightToLeft;
                message_item.HorizontalAlignment = HorizontalAlignment.Left;
                message_item.MainBorder.Background = new SolidColorBrush(Colors.LightGray);
                message_item.MainBorder.CornerRadius = new CornerRadius(15, 15, 0, 15);
            }

            global_chat.MessagesStackPanel.Children.Add(message_item);
        }

        public void UpdateUsersCallback(UserSerialized[] users)
        {
            global_chat.UsersStackPanel.Children.Clear();

            for (int index = 0; index < users.Length; index++)
            {
                global_chat.UsersStackPanel.Children.Add(new UserItem(users[index], users[index].Id == Id));
            }
        }
    }
}
