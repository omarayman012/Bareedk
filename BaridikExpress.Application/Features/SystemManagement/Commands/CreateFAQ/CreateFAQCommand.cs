using BaridikExpress.Application.Features.SystemManagement.DTOs;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.CreateFAQ;

public sealed record CreateFAQCommand(
    string QuestionAr,
    string QuestionEn,
    string AnswerAr,
    string AnswerEn)
    : IRequest<Result<FAQResponse>>;