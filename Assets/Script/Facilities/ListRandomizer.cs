using System.Collections.Generic;
using System.Linq;

public class ListRandomizer
{
    public static List<T> ShuffleList<T>(List<T> list)
    {
        System.Random random = new System.Random();
        return list.OrderBy(item => random.Next()).ToList();
    }
}