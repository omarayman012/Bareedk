using BaridikExpress.Application.Features.SystemManagement.DTOs;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateFAQ;

public sealed class UpdateFAQCommand : IRequest<Result<FAQResponse>>
{
    public Guid Id { get; set; }
    public string? QuestionAr { get; set; }
    public string? QuestionEn { get; set; }
    public string? AnswerAr { get; set; }
    public string? AnswerEn { get; set; }
}