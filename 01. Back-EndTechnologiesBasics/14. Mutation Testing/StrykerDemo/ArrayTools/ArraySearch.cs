namespace ArrayTools
{
    public class ArraySearch
    {
        public static int FindIndex(int[] array, int element)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == element)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
