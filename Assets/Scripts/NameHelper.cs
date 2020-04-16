using UnityEngine;

public static class NameHelper
{
    public static string GenerateName()
    {
        var names = new[]
        {
            "Aldous",
            "Alistair",
            "Constantine",
            "Drake",
            "Jeffery",
            "Luther",
            "Percival",
            "Randall",
            "Robin",
        };

        return names[Random.Range(0, names.Length)];
    }

    public static string GenerateFamilyName()
    {
        var familyNames = new[]
        {
            "Baker",
            "Brown",
            "Cheeseman",
            "Hayward",
            "Hughes",
            "Mason",
            "Payne",
            "Sawyer",
            "Shepherd",
            "White",
            "Webster",
        };

        return familyNames[Random.Range(0, familyNames.Length)];
    }
}
