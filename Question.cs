using System.Collections.Generic;

namespace ProgPart3
{
    public class Question
    {
        public string Text { get; set; }
        public List<string> Options { get; set; } = new();
        public int CorrectOptionIndex { get; set; } // Index of correct answer in Options list
        public string Explanation { get; set; }
        public bool IsTrueFalse { get; set; } // true if True/False question
    }
}
