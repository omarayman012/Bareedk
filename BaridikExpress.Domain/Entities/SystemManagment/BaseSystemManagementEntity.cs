using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.SystemManagment;

public abstract class BaseSystemManagementEntity : BaseEntity
{
    public Guid Id { get; protected set; }

    public string? DescriptionAr { get; protected set; }
    public string? DescriptionEn { get; protected set; }

    protected BaseSystemManagementEntity()
    {
        Id = Guid.NewGuid();
    }

    public void Update(
        string? descriptionAr = null,
        string? descriptionEn = null)
    {
        if (descriptionAr is not null)
        {
            DescriptionAr = string.IsNullOrWhiteSpace(descriptionAr)
                ? null
                : descriptionAr.Trim();
        }

        if (descriptionEn is not null)
        {
            DescriptionEn = string.IsNullOrWhiteSpace(descriptionEn)
                ? null
                : descriptionEn.Trim();
        }
    }

    public static T Create<T>(
        string? descriptionAr = null,
        string? descriptionEn = null)
        where T : BaseSystemManagementEntity, new()
    {
        var entity = new T();

        entity.Update(descriptionAr, descriptionEn);

        return entity;
    }
}