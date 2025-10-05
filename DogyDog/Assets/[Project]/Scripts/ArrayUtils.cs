public static class ArrayUtils
{
    public static void LoopIn<T>(this T[,] array, System.Action<int, int> action)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                action(i, j);
            }
        }
    }
}