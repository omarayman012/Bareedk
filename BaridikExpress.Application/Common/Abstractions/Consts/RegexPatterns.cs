namespace BaridikExpress.Application.Consts;

public static class RegexPatterns
{
    public const string Email = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public const string ImageFilePattern = @"^.*\.(jpg|jpeg|png)$";

    //public const string Password = @"(?=(.*[0-9]))(?=.*[\!@#$%^&*()\\[\]{}\-_+=~`|:;""'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";
    public const string Password = @"(?=(.*[0-9]))(?=.*[\!@#$%^&*()\\[\]{}\-_+=~`|:;""'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";
}
