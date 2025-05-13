public struct Key
{
    public int k1;
    public int k2;

    public Key(int k1, int k2)
    {
        this.k1 = k1;
        this.k2 = k2;
    }

    public static bool operator <(Key key1, Key key2)
        => key1.k1 < key2.k1 || (key1.k1 == key2.k1 && key1.k2 < key2.k2);

    public static bool operator >(Key key1, Key key2)
        => key1.k1 > key2.k1 || (key1.k1 == key2.k1 && key1.k2 > key2.k2);
}