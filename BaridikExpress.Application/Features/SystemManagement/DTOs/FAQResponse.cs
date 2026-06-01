namespace BaridikExpress.Application.Features.SystemManagement.DTOs;

public sealed record FAQResponse(
    Guid Id,
    string QuestionAr,
    string QuestionEn,
    string AnswerAr,
    string AnswerEn,
    string? CreatedBy,
    DateTime CreatedAt,
    string? UpdatedBy,
    DateTime? UpdatedAt
);
