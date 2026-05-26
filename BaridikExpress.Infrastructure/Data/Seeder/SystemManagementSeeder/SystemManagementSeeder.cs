using BaridikExpress.Domain.Entities.SystemManagment;
using BaridikExpress.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Infrastructure.Data.Seeder.SystemManagementSeeder;

public static class SystemManagementSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        try
        {
            await SeedAboutUsAsync(context);

            await SeedSystemManagementEntityAsync<TermsAndConditions>(
                context.TermsAndConditions,
                "الشروط والأحكام الخاصة ببريدك إكسبريس.",
                "Terms and conditions of Baridik Express.");

            await SeedSystemManagementEntityAsync<PrivacyPolicy>(
                context.PrivacyPolicies,
                "سياسة الخصوصية الخاصة ببريدك إكسبريس.",
                "Privacy policy of Baridik Express.");

            await SeedSystemManagementEntityAsync<ShippingPolicy>(
                context.ShippingPolicies,
                "سياسة الشحن الخاصة ببريدك إكسبريس.",
                "Shipping policy of Baridik Express.");

            await SeedSystemManagementEntityAsync<SalesAndPurchasePolicy>(
                context.SalesAndPurchasePolicies,
                "سياسة البيع والشراء الخاصة ببريدك إكسبريس.",
                "Sales and purchase policy of Baridik Express.");

            await SeedSystemManagementEntityAsync<Help>(
                context.Help,
                "مركز المساعدة الخاص ببريدك إكسبريس.",
                "Help center of Baridik Express.");

            await SeedSystemManagementEntityAsync<DeliveryDriverRegistrationTerms>(
                context.DeliveryDriverRegistrationTerms,
                "شروط تسجيل مندوب التوصيل.",
                "Delivery driver registration terms.");

            await SeedSystemManagementEntityAsync<CustomerRegistration>(
                context.CustomerRegistrationTerms,
                "شروط تسجيل العميل.",
                "Customer registration terms.");

            await SeedSocialMediaLinksAsync(context);

            await context.SaveChangesAsync();

            Console.WriteLine("System Management seeded successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            context.ChangeTracker.Clear();
        }
    }

    #region Generic System Management Seeder

    private static async Task SeedSystemManagementEntityAsync<TEntity>(
        DbSet<TEntity> dbSet,
        string descriptionAr,
        string descriptionEn)
        where TEntity : BaseSystemManagementEntity, new()
    {
        if (await dbSet.AnyAsync())
            return;

        var entity = BaseSystemManagementEntity.Create<TEntity>(
            descriptionAr,
            descriptionEn);

        await dbSet.AddAsync(entity);
    }

    #endregion

    #region AboutUs

    private static async Task SeedAboutUsAsync(ApplicationDbContext context)
    {
        if (await context.AboutUs.AnyAsync())
            return;

        var aboutUs = AboutUs.Create(
            imageUrl: string.Empty,
            titleAr: "عن بريدك إكسبريس",
            titleEn: "About Baridik Express",
            descriptionAr: "بريدك إكسبريس شركة رائدة في مجال الشحن والتوصيل.",
            descriptionEn: "Baridik Express is a leading company in shipping and delivery.");

        await context.AboutUs.AddAsync(aboutUs);
    }

    #endregion

    #region SocialMediaLinks

    private static async Task SeedSocialMediaLinksAsync(ApplicationDbContext context)
    {
        if (await context.SocialMediaLinks.AnyAsync())
            return;

        var links = new List<SocialMediaLinks>
        {
            SocialMediaLinks.Create("WhatsApp",  "https://wa.me/baridikexpress"),
            SocialMediaLinks.Create("Facebook",  "https://www.facebook.com/baridikexpress"),
            SocialMediaLinks.Create("TikTok",    "https://www.tiktok.com/@baridikexpress"),
            SocialMediaLinks.Create("Snapchat",  "https://www.snapchat.com/add/baridikexpress"),
            SocialMediaLinks.Create("Twitter",   "https://twitter.com/baridikexpress"),
            SocialMediaLinks.Create("Instagram", "https://www.instagram.com/baridikexpress"),
            SocialMediaLinks.Create("LinkedIn",  "https://www.linkedin.com/company/baridikexpress"),
            SocialMediaLinks.Create("Email",     "mailto:info@baridikexpress.com"),
            SocialMediaLinks.Create("PlayStore", "https://play.google.com/store/apps/details?id=com.baridikexpress"),
            SocialMediaLinks.Create("AppStore",  "https://apps.apple.com/app/baridikexpress"),
        };

        await context.SocialMediaLinks.AddRangeAsync(links);
    }

    #endregion
}