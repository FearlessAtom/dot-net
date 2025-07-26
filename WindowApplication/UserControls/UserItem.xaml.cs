using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WindowApplication.ServiceReference1;
using ClassLibrary.Classes;

namespace WindowApplication.UserControls
{
    public partial class UserItem : UserControl
    {
        MainWindow main_window = (MainWindow)Application.Current.MainWindow;
        public UserSerialized User { get; set; }
        bool IsMe { get; set; }

        public UserItem(UserSerialized User, bool IsMe)
        {
            InitializeComponent();

            this.User = User;
            this.IsMe = IsMe;

            if (this.IsMe)
            {
                UsernameTextBlock.Foreground = new SolidColorBrush(Colors.Blue);
            }

            UsernameTextBlock.Text = User.Username;

            if (!IsMe)
            {
                MainGrid.MouseDown += (sender, e) =>
                {
                    if (main_window.active_user == this)
                    {
                        main_window.active_user.MainGrid.Background = new SolidColorBrush(Colors.Transparent);
                        main_window.active_user = null;
                        return;
                    }
                    
                    if (main_window.active_user != null)
                    {
                        main_window.active_user.MainGrid.Background = new SolidColorBrush(Colors.Transparent);
                    }
                    
                    MainGrid.Background = new SolidColorBrush(Colors.LightSteelBlue);
                    main_window.active_user = this;
                };
            }
        }
    }
}
