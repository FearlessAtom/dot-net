using System.ComponentModel.DataAnnotations.Schema;
using EncryptionClassLibrary;

namespace ClassLibrary.Data.Models;

public class RecentFile
{
    public int Id { get; set; }
    public string Path { get; set; } = string.Empty;
    public DateTime EditingDate { get; set; }

    [NotMapped] public bool IsEncrypted
    {
        get
        {
            string result = string.Empty;

            for (int index = Path.Length - 1; index >= 0; index--)
            {
                if (Path[index] == '.')
                {
                    break;
                }

                result = result + Path[index];
            }

            return new string(result.Reverse().ToArray()) == Encryption.encrypted_extension;
        }
    }
}
