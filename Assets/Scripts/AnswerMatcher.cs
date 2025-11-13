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
            {
                Debug.Log($"AnswerMatcher: Exact match! '{spokenAnswer}' == '{acceptedAnswer}'");
                return true;
            }
            
            // Contains match - but only if one is a short abbreviation (2-3 chars) and the other is the full name
            // This prevents false positives like "france" matching "canada" because both contain "an"
            bool isShortAbbrev = normalizedAccepted.Length <= 3 || normalizedSpoken.Length <= 3;
            if (isShortAbbrev && (normalizedSpoken.Contains(normalizedAccepted) || normalizedAccepted.Contains(normalizedSpoken)))
            {
                Debug.Log($"AnswerMatcher: Abbreviation match! '{spokenAnswer}' contains '{acceptedAnswer}' or vice versa");
                return true;
            }
            
            // Fuzzy match using Levenshtein distance (only for similar length strings)
            // Make it stricter - only allow 1-2 character differences for very similar words
            int lengthDiff = Mathf.Abs(normalizedSpoken.Length - normalizedAccepted.Length);
            int distance = LevenshteinDistance(normalizedSpoken, normalizedAccepted);
            
            // Only fuzzy match if:
            // 1. Length difference is small (max 2 chars)
            // 2. Distance is very small (max 1 char difference)
            // 3. Both strings are at least 4 chars (to avoid matching short words incorrectly)
            if (lengthDiff <= 2 && distance <= 1 && normalizedSpoken.Length >= 4 && normalizedAccepted.Length >= 4)
            {
                Debug.Log($"AnswerMatcher: Fuzzy match! '{spokenAnswer}' similar to '{acceptedAnswer}' (distance: {distance})");
                return true;
            }
            
            // Log why it didn't match for debugging
            if (distance <= 3)
            {
                Debug.Log($"AnswerMatcher: Close but no match - '{spokenAnswer}' vs '{acceptedAnswer}' (distance: {distance}, lengthDiff: {lengthDiff})");
            }
        }
        
        Debug.Log($"AnswerMatcher: No match found for '{spokenAnswer}' against country '{question.countryName}'");
        
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

