namespace ClassLibrary;

public static class ArrayExtension
{
    public static int ElementCount<T>(this T[] array, T element)
    {
        return array.Where(i => EqualityComparer<T>.Default.Equals(i, element)).Count();
    }

    public static T[] UniqueElements<T>(this T[] array)
    {
        List<T> result = new List<T>();

        for(int index = 0; index < array.Length; index++)
        {
            if(!result.Contains(array[index]))
            {
                result.Add(array[index]);
            }
        }

        return result.ToArray();
    }
}