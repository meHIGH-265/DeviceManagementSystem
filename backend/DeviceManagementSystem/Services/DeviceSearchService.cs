using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DeviceManagementSystem.Domain;

namespace DeviceManagementSystem.Services
{
    public class DeviceSearchService
    {
        private const int NameWeight = 10;
        private const int ManufacturerWeight = 6;
        private const int ProcessorWeight = 4;
        private const int RamWeight = 3;

        public int CalculateScore(Device device, string query)
        {
            if (device == null || string.IsNullOrWhiteSpace(query))
                return 0;

            var tokens = NormalizeAndTokenize(query);

            int score = 0;

            score += ScoreField(device.Name, tokens, NameWeight);
            score += ScoreField(device.Manufacturer, tokens, ManufacturerWeight);
            score += ScoreField(device.Processor, tokens, ProcessorWeight);
            score += ScoreField(device.RAM, tokens, RamWeight);

            return score;
        }

        private int ScoreField(string? fieldValue, List<string> tokens, int weight)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
                return 0;

            var normalizedField = Normalize(fieldValue);
            int score = 0;

            foreach (var token in tokens)
            {
                if (normalizedField.Contains(token))
                {
                    // Base match
                    score += weight;

                    // Bonus for exact word match (stronger relevance)
                    if (IsExactWordMatch(normalizedField, token))
                    {
                        score += weight / 2;
                    }

                    // Bonus for prefix match (e.g. "iph" matches "iphone")
                    if (IsPrefixMatch(normalizedField, token))
                    {
                        score += weight / 2;
                    }
                }
            }

            return score;
        }

        private List<string> NormalizeAndTokenize(string input)
        {
            var normalized = Normalize(input);

            normalized = Regex.Replace(normalized, @"(\d)([a-zA-Z])", "$1 $2");
            normalized = Regex.Replace(normalized, @"([a-zA-Z])(\d)", "$1 $2");

            return normalized
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();
        }

        private string Normalize(string input)
        {
            // Lowercase + remove punctuation + normalize spaces
            var lower = input.ToLowerInvariant();

            var noPunctuation = Regex.Replace(lower, @"[^\w\s]", " ");

            var normalizedSpaces = Regex.Replace(noPunctuation, @"\s+", " ").Trim();

            return normalizedSpaces;
        }

        private bool IsExactWordMatch(string field, string token)
        {
            var words = field.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return words.Contains(token);
        }

        private bool IsPrefixMatch(string field, string token)
        {
            var words = field.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return words.Any(w => w.StartsWith(token));
        }
    }
}
