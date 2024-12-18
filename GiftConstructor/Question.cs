using System;
using System.Collections.Generic;

namespace GiftQuestionCreator
{
    public class Question
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; } = new List<string>();
        public string CorrectAnswer { get; set; }

        public string ToGiftFormat()
        {
            switch (Type)
            {
                case "Множественный выбор":
                    return GenerateMultipleChoice();
                case "Верно/Неверно":
                    return GenerateTrueFalse();
                case "Короткий ответ":
                    return GenerateShortAnswer();
                case "Соответствие":
                    return GenerateMatching();
                case "Эссе":
                    return GenerateEssay();
                default:
                    return "";
            }
        }

        private string GenerateMultipleChoice()
        {
            var gift = $"::{Text}:: {Options[0]} {{\n";
            foreach (var option in Options)
            {
                gift += option == CorrectAnswer ? $"={option}\n" : $"~{option}\n";
            }
            gift += "}";
            return gift;
        }

        private string GenerateTrueFalse()
        {
            return $"::{Text}:: {(CorrectAnswer.ToLower() == "true" ? "TRUE" : "FALSE")}";
        }

        private string GenerateShortAnswer()
        {
            return $"::{Text}:: {{={CorrectAnswer}}}";
        }

        private string GenerateMatching()
        {
            var gift = $"::{Text}:: {{\n";
            for (int i = 0; i < Options.Count; i += 2)
            {
                gift += $"={Options[i]} -> {Options[i + 1]}\n";
            }
            gift += "}";
            return gift;
        }

        private string GenerateEssay()
        {
            return $"::{Text}:: {{}}";
        }
    }
}
