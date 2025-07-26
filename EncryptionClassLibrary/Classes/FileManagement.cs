using ClassLibrary.Data.Models;
namespace EncryptionClassLibrary;

static public class FileManagement
{
    static public ApplicationDbContext context = new ApplicationDbContext();

    static public void AddFileToRecent(string Path)
    {
        RecentFile recent_file = new()
        {
            Path = Path,
            EditingDate = DateTime.Now,
        };

        context.RecentFiles.Add(recent_file);
        context.SaveChanges();
        UpdateRecentFiles();
    }

    static public void UpdateRecentFiles()
    {

        List<RecentFile> recent_files = context.RecentFiles.ToList();

        for (int index = recent_files.Count - 1; index >= 0; index--)
        {
            if (!File.Exists(recent_files[index].Path))
            {
                context.RecentFiles.Remove(recent_files[index]);
            }
        }

        context.SaveChanges();
    }

    static public List<RecentFile> GetRecentFiles()
    {
        return context.RecentFiles.OrderByDescending(f => f.EditingDate).ToList();
    }

    static public void RemoveRecentFile(int Id)
    {
        RecentFile recent_file = context.RecentFiles.FirstOrDefault(f => f.Id == Id);
        context.RecentFiles.Remove(recent_file);
        context.SaveChanges();
    }

    static public bool AlreadyInRecentFiles(string Path)
    {
        List<RecentFile> recent_files = context.RecentFiles.ToList();

        for (int index = 0; index < recent_files.Count; index++)
        {
            if (recent_files[index].Path == Path)
            {
                return true;
            }
        }

        return false;
    }
}
