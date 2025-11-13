using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlagQuestion
{
    public string countryName;
    public List<string> acceptedAnswers; // Variations like "USA", "United States", "America"
    public Sprite flagSprite;
    
    public FlagQuestion(string country, List<string> answers, Sprite sprite)
    {
        countryName = country;
        acceptedAnswers = answers;
        flagSprite = sprite;
    }
}

[CreateAssetMenu(fileName = "FlagData", menuName = "Game/Flag Data")]
public class FlagData : ScriptableObject
{
    public List<FlagQuestion> flags = new List<FlagQuestion>();
    
    public void InitializeDefaultFlags()
    {
        flags = new List<FlagQuestion>
        {
            new FlagQuestion("United States", new List<string> { "united states", "usa", "america", "us" }, null),
            new FlagQuestion("United Kingdom", new List<string> { "united kingdom", "uk", "britain", "england" }, null),
            new FlagQuestion("France", new List<string> { "france" }, null),
            new FlagQuestion("Germany", new List<string> { "germany", "deutschland" }, null),
            new FlagQuestion("Japan", new List<string> { "japan" }, null),
            new FlagQuestion("Canada", new List<string> { "canada" }, null),
            new FlagQuestion("Australia", new List<string> { "australia" }, null),
            new FlagQuestion("Brazil", new List<string> { "brazil", "brasil" }, null),
            new FlagQuestion("India", new List<string> { "india" }, null),
            new FlagQuestion("Italy", new List<string> { "italy", "italia" }, null)
        };
    }
}

