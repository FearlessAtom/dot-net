using System.Windows.Controls;
using ClassLibrary.Data.Models;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System;

namespace EncryptionWindowApplication.UserControls;

public partial class FileItem : UserControl
{
    public RecentFile File { get; set; }

    private MainWindow main_window = (MainWindow)Application.Current.MainWindow;

    public FileItem(RecentFile file)
    {
        InitializeComponent();

        this.File = file;

        PathTextBlock.Text = file.Path;
        EditionDateTextBlock.Text = file.EditingDate.ToString();
        FileNameTextBlock.Text = Path.GetFileName(file.Path);

        FileIconImage.Source = main_window.GetBitImage(file.Path);
        
        FileIconImage.MouseDown += (sender, e) =>
        {
            if (!System.IO.File.Exists(File.Path))
            {
                return;
            }

            Process.Start( new ProcessStartInfo { FileName = File.Path, UseShellExecute = true } );
        };
    }
    
    private void SetActive(object sender, RoutedEventArgs e)
    {
        main_window.SetActiveFile(this);
    }
}
