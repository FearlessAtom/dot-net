using System.Windows.Media.Imaging;
using ClassLibrary.Data.Models;
using System.Windows;
using EncryptionWindowApplication.UserControls;
using Microsoft.Win32;
using EncryptionClassLibrary;
using System.Windows.Media;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace EncryptionWindowApplication;

public partial class MainWindow : Window
{
    public readonly ApplicationDbContext context = new ApplicationDbContext();

    public FileItem? active_file;
    public bool encryption_menu_open = false;

    public MainWindow()
    {
        InitializeComponent();
        FileManagement.UpdateRecentFiles();

        DisplayRecentFiles();

        CheckForZeroFiles();
        SetFirstFileAsActive();

        StackPanelGrid.Drop += FileDrop;
        StackPanelGrid.DragOver += DragEnter;
        StackPanelGrid.DragLeave += DragLeave;
    }
    
    private void BrowseButton(object sender, RoutedEventArgs e)
    {
        OpenFileDialog file_dialog = new OpenFileDialog();
        file_dialog.InitialDirectory = "C:\\";
        bool result = (bool)file_dialog.ShowDialog();

        if (!result || string.IsNullOrEmpty(file_dialog.FileName))
        {
            return;
        }

        AddFile(file_dialog.FileName);
    }


    public void AddFile(string path)
    {
        if (FileManagement.AlreadyInRecentFiles(path))
        {
            MessageBox.Show("File is already added!");
            return;
        }

        FileNameTextBlock.Text = Path.GetFileName(path);
        FileManagement.AddFileToRecent(path);

        DisplayRecentFiles();
        SetFirstFileAsActive();
    }

    public void UpdateButton(object sender, RoutedEventArgs e)
    {
        FileManagement.UpdateRecentFiles();
        DisplayRecentFiles();
        SetFirstFileAsActive();
    }

    public void CheckForZeroFiles()
    {
        if (RecentFilesStackPanel.Children.Count != 0)
        {
            return;
        }

        active_file = null;
        FileNameTextBlock.Text = "Click \"Browse\" to add a file";
        PathTextBlock.Text = string.Empty;
    }

    public void DisplayRecentFiles()
    {
       RecentFilesStackPanel.Children.Clear();
        
       List<RecentFile> recent_files =  FileManagement.GetRecentFiles();
       
       for (int index = 0; index < recent_files.Count(); index++)
       {

           FileItem recent_file = new FileItem(recent_files[index]);
           RecentFilesStackPanel.Children.Add(recent_file);
       }
    }
    
    public void SetActiveFile(FileItem file_item)
    {
        if (active_file != null)
        {
            active_file.Background = new SolidColorBrush(Colors.Transparent);
        }
        
        active_file = file_item;

        active_file.Background = new SolidColorBrush(Colors.LightSteelBlue);
        
        FileNameTextBlock.Text = Path.GetFileName(file_item.File.Path);
        PathTextBlock.Text = file_item.File.Path;

        bool is_encrypted = file_item.File.IsEncrypted;

        EncryptButton.Content = is_encrypted ? "Decrypt" : "Encrypt";

        EncryptButton.Click -= DecryptButtonClick;
        EncryptButton.Click -= EncryptButtonClick;

        EncryptButton.Click += is_encrypted ? DecryptButtonClick : EncryptButtonClick;
    }

    public void DeleteActiveFile(object sender, RoutedEventArgs e)
    {
        if (active_file == null || encryption_menu_open)
        {
            return;
        }
        
        FileManagement.RemoveRecentFile(active_file.File.Id);
        DisplayRecentFiles();

        CheckForZeroFiles();
        SetFirstFileAsActive();
    }

    public void SetFirstFileAsActive()
    {
        if (RecentFilesStackPanel.Children.Count == 0)
        {
            CheckForZeroFiles();
            return;
        }

        SetActiveFile((FileItem)RecentFilesStackPanel.Children[0]);
    }

    public BitmapImage GetBitImage(string Path)
    {
        Icon file_icon = System.Drawing.Icon.ExtractAssociatedIcon(Path);
        MemoryStream memory_stream = new MemoryStream();
        
        file_icon.ToBitmap().Save(memory_stream, ImageFormat.Png);
        memory_stream.Seek(0, SeekOrigin.Begin);
        
        var bitmap_image = new BitmapImage();
        
        bitmap_image.BeginInit();
        bitmap_image.StreamSource = memory_stream;
        bitmap_image.CacheOption = BitmapCacheOption.OnLoad;
        bitmap_image.EndInit();
        
        memory_stream.Close();
        
        return bitmap_image;
    }

    public void EncryptButtonClick(object sender, RoutedEventArgs e)
    {
        if (active_file == null || encryption_menu_open)
        {
            return;
        }

        StackPanelGrid.Children.Add(new EncryptionMenu(true));
        encryption_menu_open = true;
    }

    public void DecryptButtonClick(object sender, RoutedEventArgs e)
    {
        if (active_file == null || encryption_menu_open)
        {
            return;
        }

        StackPanelGrid.Children.Add(new EncryptionMenu(false));
        encryption_menu_open = true;
    }

    private void DragEnter(object sender, DragEventArgs e)
    {
        StackPanelBorder.BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);
    }

    private void DragLeave(object sender, DragEventArgs e)
    {
        StackPanelBorder.BorderBrush = new SolidColorBrush(Colors.Black);
    }

    private void FileDrop(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            return;
        }

        DragLeave(null, null);

        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

        foreach (string file in files)
        {
            AddFile(file);
        }
    }
}
