using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AnswerMatcher
{
    /// <summary>
    /// Checks if the spoken answer matches any of the accepted answers for a flag question
    /// Uses fuzzy matching to handle variations and typos
    /// </summary>
    public static bool IsAnswerCorrect(string spokenAnswer, FlagQuestion question)
    {
        if (string.IsNullOrEmpty(spokenAnswer) || question == null)
            return false;
        
        string normalizedSpoken = NormalizeString(spokenAnswer);
        
        // Check exact matches first
        foreach (string acceptedAnswer in question.acceptedAnswers)
        {
            string normalizedAccepted = NormalizeString(acceptedAnswer);
            
            // Exact match
            if (normalizedSpoken == normalizedAccepted)
                return true;
            
            // Contains match (e.g., "united states" contains "usa" or vice versa)
            if (normalizedSpoken.Contains(normalizedAccepted) || normalizedAccepted.Contains(normalizedSpoken))
                return true;
            
            // Fuzzy match using Levenshtein distance
            if (LevenshteinDistance(normalizedSpoken, normalizedAccepted) <= 2)
                return true;
        }
        
        return false;
    }
    
    private static string NormalizeString(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";
        
        return input.ToLower().Trim();
    }
    
    /// <summary>
    /// Calculate Levenshtein distance between two strings
    /// Returns the minimum number of single-character edits needed to transform one string into another
    /// </summary>
    private static int LevenshteinDistance(string s, string t)
    {
        if (string.IsNullOrEmpty(s))
            return string.IsNullOrEmpty(t) ? 0 : t.Length;
        
        if (string.IsNullOrEmpty(t))
            return s.Length;
        
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];
        
        // Initialize
        for (int i = 0; i <= n; i++)
            d[i, 0] = i;
        
        for (int j = 0; j <= m; j++)
            d[0, j] = j;
        
        // Fill matrix
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Mathf.Min(
                    d[i - 1, j] + 1,      // deletion
                    d[i, j - 1] + 1,      // insertion
                    d[i - 1, j - 1] + cost // substitution
                );
            }
        }
        
        return d[n, m];
    }
}

