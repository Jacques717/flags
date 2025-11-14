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
        // Use the comprehensive list of all countries
        flags = new List<FlagQuestion>();
        
        foreach (var country in AllCountriesList.GetAllCountries())
        {
            flags.Add(new FlagQuestion(country.countryName, country.acceptedAnswers, null));
        }
        
        Debug.Log($"FlagData initialized with {flags.Count} countries");
    }
}

