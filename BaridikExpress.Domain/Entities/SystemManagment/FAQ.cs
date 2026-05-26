using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.SystemManagment;

public class FAQ : BaseEntity
{
    public Guid Id { get; private set; }
    public string QuestionAr { get; private set; } = string.Empty;
    public string QuestionEn { get; private set; } = string.Empty;
    public string AnswerAr { get; private set; } = string.Empty;
    public string AnswerEn { get; private set; } = string.Empty;

    private FAQ() { }

    public static FAQ Create(
        string questionAr,
        string questionEn,
        string answerAr,
        string answerEn)
    {
        return new FAQ
        {
            Id = Guid.NewGuid(),
            QuestionAr = questionAr,
            QuestionEn = questionEn,
            AnswerAr = answerAr,
            AnswerEn = answerEn,
        };
    }

    public void Update(
        string? questionAr = null,
        string? questionEn = null,
        string? answerAr = null,
        string? answerEn = null)
    {
        if (!string.IsNullOrWhiteSpace(questionAr)) QuestionAr = questionAr;
        if (!string.IsNullOrWhiteSpace(questionEn)) QuestionEn = questionEn;
        if (!string.IsNullOrWhiteSpace(answerAr)) AnswerAr = answerAr;
        if (!string.IsNullOrWhiteSpace(answerEn)) AnswerEn = answerEn;
    }
}