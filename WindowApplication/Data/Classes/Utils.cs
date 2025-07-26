using System.Windows.Media.Imaging;

namespace WindowApplication.Data;

public class Utils
{
    public const decimal ForeignFee = (decimal)0.50;
    
    public static BitmapImage GetLogo(string name)
    {
        Uri url = new Uri("../Data/Images/Logos/" + name, UriKind.Relative);
        return new BitmapImage(url);
    }
}