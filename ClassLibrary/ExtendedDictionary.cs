using System.Collections;

namespace ClassLibrary;

public class ExtendedDictionary<T, U, V> : IEnumerable<ExtendedDictionaryElement<T, U, V>>
{
    private Dictionary<T, ExtendedDictionaryElement<T, U, V>> _dictionary;
    
    public ExtendedDictionary()
    {
        _dictionary = new Dictionary<T, ExtendedDictionaryElement<T, U, V>>();
    }

    public void Add(T key, U value_one, V value_two)
    {
        _dictionary[key] = new ExtendedDictionaryElement<T, U, V>(key, value_one, value_two);
    }

    public bool Remove(T key)
    {
        return _dictionary.Remove(key);
    }

    public bool ContainsKey(T key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool ContainsValue(U value_one, V value_two)
    {
        foreach(var pair in _dictionary.Values)
        {
            if(EqualityComparer<U>.Default.Equals(pair.Element1, value_one) &&
               EqualityComparer<V>.Default.Equals(pair.Element2, value_two))
            {
                return true;
            }
        }

        return false;
    }

    public new ExtendedDictionaryElement<T, U, V> this[T key]
    {
        get
        {
            if(_dictionary.TryGetValue(key, out var value))
            {
                return value;
            }
            
            throw new KeyNotFoundException();
        }
    }

    public IEnumerator<ExtendedDictionaryElement<T, U, V>> GetEnumerator()
    {
        return _dictionary.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}