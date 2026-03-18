namespace EngConnect.Domain.Constants;

public static class SummarizePromptConstant
{
    public const string SummarizePromptTemplate =
        """
        Role: Bạn là chuyên gia phân tích nội dung học tập.

        Input:
        - transcript: Văn bản từ Whisper
        - outcome: Danh sách mục tiêu học tập

        Nhiệm vụ:
        1. AiSummarizeText:
        Tạo bản tóm tắt theo dạng "ghi chú học tập" (learning notes), không phải tóm tắt chung chung.
        Yêu cầu:
        - Giữ lại các kiến thức quan trọng, có thể học lại từ AiSummarizeText
        - Bao gồm:
          + Cấu trúc bài học
          + Các ý chính
          + Ví dụ nếu có
        - Viết rõ ràng, dễ hiểu, có thể dùng bullet points
        - Không được viết quá chung chung
        - Độ dài: tối thiểu 5-7 ý
        
        Mục tiêu:
        Người đọc AiSummarizeText có thể hiểu và ôn lại bài học mà không cần transcript

        2. So sánh với outcome:
        Dựa trên transcript

        Quy tắc đánh giá:
        - pass: Outcome được đề cập rõ ràng hoặc >=70% ý chính
        - fail: Không đề cập hoặc đề cập rất ít / sai
        Khi đánh giá pass/fail:
        - Chỉ đánh dấu "pass" nếu nội dung trong AiSummarizeText đề cập rõ ràng và trực tiếp đến outcome
        - Không suy diễn hoặc mở rộng ý nghĩa
        - Nếu chỉ liên quan một phần hoặc không rõ ràng → fail
        - Phải nhất quán: cùng input phải cho cùng output
        
        3. detail:
        {{
          "pass": [],
          "fail": []
        }}

        Output:
        - Có thể dùng markdown
        + Không sử dụng markdown (###, **, -, ...)
        + Chỉ trả về text thuần, rõ ràng
        Trả về JSON thuần:
        {{
          "AiSummarizeText": "...",
          "detail": {{ "pass": [], "fail": [] }}
        }}

        Dữ liệu:
        transcript: {0}
        outcome: {1}
        """;
    public const decimal PercentageEvaluation = 70;
}