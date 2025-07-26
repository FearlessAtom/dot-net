using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;

namespace EncryptionClassLibrary;

public class Encryption
{
    public static string encrypted_extension = "encrypted";

    public static void EncryptFile(string file_path, byte[] key, byte[] iv, BackgroundWorker background_worker)
    {
        string output_file_path = file_path + "." + encrypted_extension;
        Aes aes = Aes.Create();

        aes.Key = key;
        aes.IV = iv;

        FileStream input_stream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
        FileStream output_stream = new FileStream(output_file_path, FileMode.Create, FileAccess.Write);
        CryptoStream crypto_stream = new CryptoStream(output_stream, aes.CreateEncryptor(),
                CryptoStreamMode.Write);


        long total_bytes = input_stream.Length;
        long bytes_processed = 0;

        byte[] buffer = new byte[4096];
        int bytes_read;

        while((bytes_read = input_stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            bytes_processed = bytes_processed + bytes_read;

            crypto_stream.Write(buffer, 0, bytes_read);

            int percentage = (int)((bytes_processed * 100) / total_bytes);

            background_worker.ReportProgress(percentage);
        }

        crypto_stream.FlushFinalBlock();

        crypto_stream.Dispose();
        output_stream.Dispose();
        input_stream.Dispose();
        

        aes.Dispose();

        File.Delete(file_path);
    }
    
    public static bool DecryptFile(string file_path, byte[] key, byte[] iv, BackgroundWorker background_worker)
    {
        string output_file_path = file_path.Replace("." + encrypted_extension, "");

        Aes aes = Aes.Create();

        aes.Key = key;
        aes.IV = iv;

        FileStream input_file_stream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
        FileStream output_file_stream = new FileStream(output_file_path, FileMode.Create, FileAccess.Write);
        CryptoStream crypto_stream = new CryptoStream(input_file_stream, aes.CreateDecryptor(),
                CryptoStreamMode.Read);
        
        long total_bytes = input_file_stream.Length;
        long bytes_processed = 0;

        byte[] buffer = new byte[4096];
        int bytes_read;

        try
        {
            while((bytes_read = crypto_stream.Read(buffer, 0, buffer.Length)) > 0)
            {
        	    output_file_stream.Write(buffer, 0, bytes_read);

                bytes_processed = bytes_processed + bytes_read;

                int percentage = (int)((bytes_processed * 100) / total_bytes);

                background_worker.ReportProgress(percentage);
            }
        }

        catch
        {
            crypto_stream.Dispose();
            output_file_stream.Dispose();
            input_file_stream.Dispose();

            File.Delete(output_file_path);
            return false;
        }

        crypto_stream.Dispose();
        output_file_stream.Dispose();
        input_file_stream.Dispose();

        aes.Dispose();

        File.Delete(file_path);

        return true;
    }
}
