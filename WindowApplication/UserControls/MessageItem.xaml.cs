using WindowApplication.ServiceReference1;
using System.Windows.Controls;
using System.Threading.Tasks;
using ClassLibrary.Classes;
using System;
using System.Windows.Documents;
using System.Windows.Media;

namespace WindowApplication.UserControls
{
    public partial class MessageItem : UserControl
    {
        public Message Message { get; set; }

        public MessageItem(Message Message, ServiceChatClient client)
        {
            InitializeComponent();
            LoadMessageAsync(Message, client);
        }

        public async Task LoadMessageAsync(Message Message, ServiceChatClient client)
        {
            this.Message = Message;
            MessageContentTextBlock.Text = Message.Content;
            DateTextBlock.Text = Message.Date.ToShortTimeString();

            UsernameTextBlock.Text = await Task.Run(() => client.GetUsernameByIdAsync(Message.Sender));
            
            if (Message.Receiver != null)
            {
                //UsernameTextBlock.Text = UsernameTextBlock.Text + " -> " +
                //    await Task.Run(() => client.GetUsernameById((int)Message.Receiver));

                Run arrow = new Run(" -> ")
                {
                    Foreground = new SolidColorBrush(Colors.Yellow)
                };

                UsernameTextBlock.Inlines.Add(arrow);

                Run reciever_name = new Run(await Task.Run(() => client.GetUsernameById((int)Message.Receiver)))
                { 
                    Foreground = new SolidColorBrush(Colors.Blue),
                };
                UsernameTextBlock.Inlines.Add(reciever_name);
                
            }
        }
    }
}
