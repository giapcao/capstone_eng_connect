namespace EngConnect.Domain.Constants;

public class GenerateQuizConstant
{
    public const string GeneratePromptTemplate = 
        """
        Bạn là một hệ thống tạo câu hỏi trắc nghiệm.
        Nhiệm vụ:
        - Dựa trên nội dung tôi cung cấp, hãy tạo khoảng 10 câu hỏi trắc nghiệm.
        - Mỗi câu hỏi phải có 4 lựa chọn: A, B, C, D.
        - Mỗi câu hỏi phải có thêm trường "Explanation":
          + Giải thích ngắn gọn tại sao đáp án đúng là đúng
          + Và vì sao các đáp án khác sai
        - Chỉ có 1 đáp án đúng.
        
        Yêu cầu format output:
        - Trả về kết quả dưới dạng JSON array.
        - Mỗi phần tử trong list có format chính xác như sau (không dư dấu phẩy ở cuối):
        
        [
          {
            "Question": "Nội dung câu hỏi",
            "Answer": { "A": "...", "B": "...", "C": "...", "D": "..." },
            "True": "A",
            "Explanation": "Giải thích tại sao A đúng và các đáp án khác sai"
          }
        ]
        
        Quy tắc:
        - Không giải thích thêm.
        - Không thêm text ngoài JSON.
        - Nội dung câu hỏi phải bám sát nội dung tôi cung cấp.
        - Các đáp án sai phải hợp lý (không quá vô lý).
        - Đáp án đúng phải chính xác.
        
        Nội dung:
        {{CONTENT}}
        """;
}