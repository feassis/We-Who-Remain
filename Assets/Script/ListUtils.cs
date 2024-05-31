using System;
using System.Collections.Generic;

public class ListUtils
{
    private static Random random = new Random();

    public static T GetRandomMember<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new ArgumentException("The list is null or empty.");
        }

        int randomIndex = random.Next(list.Count);
        return list[randomIndex];
    }
}