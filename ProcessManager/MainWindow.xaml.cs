using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ProcessManager;

namespace ProcessManager;

public class ProcessItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Memory { get; set; } = string.Empty;
    public int CountOfThreads { get; set; }
    public string StartTime { get; set; } = string.Empty;
}

public partial class MainWindow : Window
{
    ObservableCollection<ProcessItem> processes_collection;
    ContextMenu context_menu = new ContextMenu();
    ProcessItem current_process;
    Timer timer;

    public MainWindow()
    {
        InitializeComponent();
        UpdateProcesses();

        MainDataGrid.IsReadOnly = true; 
        timer = new Timer((object state) => TimerTick(state), null, 0, 1000);
        MainDataGrid.Sorting += (sender, e) => e.Handled = true;

        SetApplications();
    }

    public void CreateApplication(string Content, string Path)
    {
        Button button = new Button();

        button.Click += (sender, e) =>
        {
            try
            {
                Process.Start(Path);
            }

            catch
            {
                MessageBox.Show($"Application {Path} is not found!");
            }
        };

        button.FontFamily = new FontFamily("Verdana");
        button.Margin = new Thickness(1, 1, 1, 1);
        button.Padding = new Thickness(0, 5, 0, 5);
        button.Background = new SolidColorBrush(Colors.White);
        button.Content = Content;
        ApplicationsStackPanel.Children.Add(button);
    }

    public void SetApplications()
    {
        CreateApplication("Powershell", "powershell.exe");
        CreateApplication("Word", "winword.exe");
        CreateApplication("Calculator", "calc.exe");
        CreateApplication("Paint", "mspaint.exe");
        CreateApplication("Notepad", "notepad.exe");
    }

    public void TimerTick(object state)
    {
        Dispatcher.Invoke(() =>
        {
            if(context_menu.IsOpen)
            {
                return;
            }

            UpdateProcesses();
        });
    }

    public void UpdateProcesses()
    {
        ProcessItem selected_item = (ProcessItem)MainDataGrid.SelectedItem;
        int Id = -1;
        if (selected_item != null)
        {
            Id = selected_item.Id;
        }

        Process[] processes = Process.GetProcesses();
        processes = processes.OrderByDescending(p => p.WorkingSet64).ToArray();

        processes_collection = new ObservableCollection<ProcessItem>();
        
        for (int index = 0; index < processes.Count(); index++)
        {
            string memory = (processes[index].WorkingSet64 / (1024.0 * 1024.0)).ToString("F0") + " MB";

            string start_time;

            try
            {
                start_time = processes[index].StartTime.ToString();
            }

            catch
            {
                start_time = string.Empty;
            }

            processes_collection.Add(new ProcessItem(){ Id = processes[index].Id,
                    Name = processes[index].ProcessName, Memory = memory, StartTime = start_time,
                    CountOfThreads = processes[index].Threads.Count});
        }
        
        MainDataGrid.PreviewMouseRightButtonDown += (sender, e) => RightClick(sender, e);
        MainDataGrid.DataContext = processes_collection;
        
        if (Id != -1)
        {
            for (int index = 0; index < MainDataGrid.Items.Count; index++)
            {
                if (((ProcessItem)MainDataGrid.Items[index]).Id == Id)
                {
                    MainDataGrid.SelectedIndex = index;
                    MainDataGrid.Focus();
                    break;
                }
            }
        }
        
    }

    public void RightClick(object sender, MouseButtonEventArgs e)
    {

        if (context_menu.IsOpen)
        {
            return;
        }

        Point position = e.GetPosition(MainDataGrid);
        DependencyObject hit = MainDataGrid.InputHitTest(position) as DependencyObject;

        while (hit is not DataGridRow)
        {
            hit = VisualTreeHelper.GetParent(hit);
        }

        var clicked_item = ((DataGridRow)hit).Item;
        MainDataGrid.SelectedItem = clicked_item;
        
        current_process = (ProcessItem)clicked_item;
        int Id = current_process.Id;

        context_menu = new ContextMenu();

        MenuItem end_process_menu_item = new MenuItem();
        end_process_menu_item.Header = "End process";
        end_process_menu_item.Click += (sender, e) =>
        {
            Process process = Process.GetProcessById(Id);

            try
            {
                process.Kill();
                processes_collection.Remove((ProcessItem)clicked_item);
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        };

        context_menu.Items.Add(end_process_menu_item);

        try
        {
            ComboBox process_priority = new ComboBox();

            process_priority.Items.Add("High");
            process_priority.Items.Add("Above normal");
            process_priority.Items.Add("Normal");
            process_priority.Items.Add("Below normal");
            process_priority.Items.Add("Low");
            
            process_priority.SelectedItem = GetPriorityString(Process.GetProcessById(Id).PriorityClass);

            process_priority.SelectionChanged += (sender, e) => Process.GetProcessById(Id).PriorityClass =
                GetPriority((string)process_priority.SelectedItem);

            context_menu.Items.Add(process_priority);
        }

        catch {}

        context_menu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
        context_menu.IsOpen = true;
    }

    public ProcessPriorityClass GetPriority(string Priority)
    {
        switch (Priority)
        {
            case "Low":
                return ProcessPriorityClass.Idle;
            case "Below normal":
                return ProcessPriorityClass.BelowNormal;
            case "Normal":
                return ProcessPriorityClass.Normal;
            case "Above normal":
                return ProcessPriorityClass.AboveNormal;
            case "High":
                return ProcessPriorityClass.High;
            default:
                return ProcessPriorityClass.Normal;
        }
    }

    public string GetPriorityString(ProcessPriorityClass Priority)
    {
        switch (Priority)
        {
            case ProcessPriorityClass.Idle:
                return "Low";
            case ProcessPriorityClass.BelowNormal:
                return "Below normal";
            case ProcessPriorityClass.Normal:
                return "Normal";
            case ProcessPriorityClass.AboveNormal:
                return "Above normal";
            case ProcessPriorityClass.High:
                return "High";
            default:
                return "Normal";
        }
    }
}
