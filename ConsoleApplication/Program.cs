using ClassLibrary;

namespace ConsoleApplication;

class Program
{
    static void Main(string[] args)
    {
        //StringExtension
        string message = "Hello world!";
        char character = 'l';

        Console.WriteLine(message.Invert());
        Console.WriteLine(message.CharCount(character));

        //ArrayExtension
        int[] array = {1, 2, 2, 3, 3, 3};
        int element = 2;

        Console.WriteLine(array.ElementCount(element));

        int[] unique_elements = array.UniqueElements();
        for(int index = 0; index < unique_elements.Length; index++)
        {
            Console.Write(unique_elements[index] + " ");
        }

        Console.WriteLine();

        //ExtendedDictionary
        ExtendedDictionary<string, int, int> dictionary = new ExtendedDictionary<string, int, int>();

        string key = "FirstPoint";
        int first_value = 15;
        int second_value = 0;

        dictionary.Add(key, first_value, second_value);
        dictionary.Add("SecondPoint", 5, 10);
        dictionary.Add("ThirdPoint", 10, 5);

        dictionary.Remove("");

        Console.WriteLine($"Element with the key \"{key}\" {(dictionary.ContainsKey(key) ? "exists." : "does not exist!")} ");
        Console.WriteLine($"Element with the values {first_value} and {second_value} {(dictionary.ContainsValue(first_value, second_value) ? "exists." : "does not exist!")} ");

        ExtendedDictionaryElement<string, int, int> pair = dictionary["ThirdPoint"];
        Console.WriteLine($"Key: {pair.Key}, First element: {pair.Element1}, Second element: {pair.Element2}");

        Console.WriteLine("Count: " + dictionary.Count());
    }
}