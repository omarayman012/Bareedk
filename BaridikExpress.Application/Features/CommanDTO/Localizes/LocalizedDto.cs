namespace BaridikExpress.Application.Features.CommanDTO.Localizes
{
    public class LocalizedDto
    {
        public string? EN { get; set; }
        public string? AR { get; set; }
    }
    public class LocalizedNameDto
    {
        public Guid? Id { get; set; }
        public string? EN { get; set; }
        public string? AR { get; set; }
    }

    public  class LocalizedListDto
    {
        public List<string> AR { get; set; } = [];
        public List<string> EN { get; set; } = [];
    }

}

