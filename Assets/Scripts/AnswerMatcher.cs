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
            
            // Contains match - but only if BOTH are short abbreviations (2-3 chars)
            // Single characters should NOT match full words (e.g., "i" should not match "india")
            // This prevents false positives like "i" matching "india" or "france" matching "canada"
            bool bothAreShort = normalizedAccepted.Length <= 3 && normalizedSpoken.Length <= 3;
            bool oneIsSingleChar = normalizedAccepted.Length == 1 || normalizedSpoken.Length == 1;
            
            // Only allow contains match if:
            // 1. Both are short (2-3 chars) - e.g., "uk" matches "usa" if one contains the other
            // 2. OR one is a single char AND the other is also a single char (e.g., "i" matches "i")
            // But NOT if one is single char and the other is longer (e.g., "i" should NOT match "india")
            if (bothAreShort && !oneIsSingleChar && (normalizedSpoken.Contains(normalizedAccepted) || normalizedAccepted.Contains(normalizedSpoken)))
            {
                Debug.Log($"AnswerMatcher: Abbreviation match! '{spokenAnswer}' contains '{acceptedAnswer}' or vice versa");
                return true;
            }
            
            // Single character exact match only
            if (normalizedSpoken.Length == 1 && normalizedAccepted.Length == 1 && normalizedSpoken == normalizedAccepted)
            {
                Debug.Log($"AnswerMatcher: Single character exact match! '{spokenAnswer}' == '{acceptedAnswer}'");
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

