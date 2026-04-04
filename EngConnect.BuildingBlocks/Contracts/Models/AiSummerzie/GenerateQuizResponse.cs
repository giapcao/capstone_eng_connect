using System.Text.Json.Serialization;

namespace EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;

public class QuizQuestionResponse
{
    [JsonPropertyName("Question")]
    public string Question { get; set; } = string.Empty;

    [JsonPropertyName("Answer")]
    public Dictionary<string, string> Answer { get; set; } = [];

    [JsonPropertyName("True")]
    public string CorrectAnswer { get; set; } = string.Empty;

    [JsonPropertyName("Explanation")]
    public string Explanation { get; set; } = string.Empty;
}

public class GenerateQuizResponse
{
    public List<QuizQuestionResponse> Questions { get; set; } = [];
}