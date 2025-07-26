using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using ClassLibrary.Data.Models;
using EncryptionClassLibrary;
using System.Text;
using System;
using System.IO;

namespace EncryptionWindowApplication.UserControls;

public partial class EncryptionMenu : UserControl
{
    private BackgroundWorker background_worker = new BackgroundWorker();
    private MainWindow main_window = (MainWindow)Application.Current.MainWindow;
    private Stopwatch stop_watch = new Stopwatch();
    private System.Timers.Timer timer = new System.Timers.Timer(10);

    private bool InvalidKey = false;

    public event Action OnConfirmEvent;
    
    public EncryptionMenu(bool Mode)
    {
        InitializeComponent();
        TitleTextBlock.Text = (Mode ? "Encryption": "Decryption") + " Menu";

        OnConfirmEvent += (Mode ? Encrypt : Decrypt);

        background_worker.WorkerReportsProgress = true;
        background_worker.WorkerSupportsCancellation = true;
        
        background_worker.DoWork += (object sender, DoWorkEventArgs e) =>
        {
            stop_watch.Start();
            OnConfirmEvent();
            stop_watch.Stop();
        };

        background_worker.ProgressChanged += (object sender, ProgressChangedEventArgs e) => ProgressChanged(e);
        background_worker.RunWorkerCompleted += (object sender,  RunWorkerCompletedEventArgs e) => 
            WorkerCompleted();
        
        CancelButton.Click += (object sender, RoutedEventArgs e) => Close();
        ConfirmButton.Click += (object sender, RoutedEventArgs e) => ConfirmButtonClick();
    }

    private void WorkerCompleted()
    {
        timer.Stop();

        if (InvalidKey)
        {
            MessageBox.Show("Invalid key!");
            return;
        }

        main_window.UpdateButton(null, null);

        if (Parent != null)
        {
            Close();
        }

        if (main_window.active_file == null)
        {
            MessageBox.Show("Incorrect key!");
            return;
        }
        
        MessageBox.Show(main_window.active_file.File.Path +
                $"\nTime taken: {stop_watch.Elapsed.ToString()}" +
                $"\nFile size: {new FileInfo(main_window.active_file.File.Path).Length} bytes");
    }

    private void ConfirmButtonClick()
    {
        if (KeyPasswordBox.Password.Length != 16)
        {
            MessageBox.Show("Key must be 16 characters long!");
            return;
        }

        KeyPasswordBox.IsEnabled = false;

        stop_watch.Reset();

        timer.AutoReset = true;

        DateTime initial_time = DateTime.Now;

        timer.Elapsed += (sender, e) =>
        {
            TimeSpan time_difference = e.SignalTime - initial_time;

            string time_taken = $"Time taken: {time_difference.Hours:D2}:{time_difference.Minutes:D2}:" +
                $"{time_difference.Seconds:D2}.{time_difference.Milliseconds:D3}";
        
            Dispatcher.Invoke(() =>
            {
                TimerTextBlock.Text = time_taken;
            }); 
        };

        timer.Start();

        background_worker.RunWorkerAsync();
    }

    private void Encrypt()
    {
        Encryption.EncryptFile(main_window.active_file.File.Path,
                Encoding.UTF8.GetBytes(KeyPasswordBox.Password), new byte[16], background_worker);

        string new_path = main_window.active_file.File.Path +"." + Encryption.encrypted_extension;

        main_window.active_file.File.Path = new_path;
        main_window.active_file.File.EditingDate = DateTime.Now;

        main_window.context.SaveChanges();
        main_window.UpdateButton(null, null);
    }

    private void Decrypt()
    {
        bool result = Encryption.DecryptFile(main_window.active_file.File.Path,
                Encoding.UTF8.GetBytes(KeyPasswordBox.Password), new byte[16], background_worker);

        string new_path = main_window.active_file.File.Path.Replace("." + Encryption.encrypted_extension, "");

        main_window.active_file.File.Path = new_path;
        main_window.active_file.File.EditingDate = DateTime.Now;

        main_window.context.SaveChanges();
    }

    public void ProgressChanged(ProgressChangedEventArgs e)
    {
        Progress.Value = e.ProgressPercentage;
    }

    public void Close()
    {
        Grid parent = (Grid)Parent;
        parent.Children.Remove(this);
        main_window.encryption_menu_open = false;
    }
}
