namespace ClassLibrary;

public class ExtendedDictionaryElement<T, U, V>
{
    public T Key { get; set; }
    public U Element1 { get; set; }
    public V Element2 { get; set; }

    public ExtendedDictionaryElement(T key, U value1, V value2)
    {
        Key = key;
        Element1 = value1;
        Element2 = value2;
    }
}