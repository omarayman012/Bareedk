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
            await SeedTermsAndConditionsAsync(context);
            await SeedPrivacyPolicyAsync(context);
            await SeedShippingPolicyAsync(context);
            await SeedSalesAndPurchasePolicyAsync(context);
            await SeedHelpAsync(context);
            await SeedDeliveryDriverRegistrationTermsAsync(context);
            await SeedCustomerRegistrationAsync(context);
            await SeedSocialMediaLinksAsync(context);

            Console.WriteLine("System Management seeded successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding System Management: {ex.Message}");
            context.ChangeTracker.Clear();
        }
    }

    #region AboutUs

    private static async Task SeedAboutUsAsync(ApplicationDbContext context)
    {
        if (await context.AboutUs.AnyAsync()) return;

        var aboutUs = AboutUs.Create(
            imageUrl: string.Empty,
            titleAr: "عن بريدك إكسبريس",
            titleEn: "About Baridik Express",
            descriptionAr: "بريدك إكسبريس شركة رائدة في مجال الشحن والتوصيل.",
            descriptionEn: "Baridik Express is a leading company in shipping and delivery.");

        await context.AboutUs.AddAsync(aboutUs);
        await context.SaveChangesAsync();
    }

    #endregion

    #region TermsAndConditions

    private static async Task SeedTermsAndConditionsAsync(ApplicationDbContext context)
    {
        if (await context.TermsAndConditions.AnyAsync()) return;

        var terms = TermsAndConditions.Create(
            descriptionAr: "الشروط والأحكام الخاصة ببريدك إكسبريس.",
            descriptionEn: "Terms and conditions of Baridik Express.");

        await context.TermsAndConditions.AddAsync(terms);
        await context.SaveChangesAsync();
    }

    #endregion

    #region PrivacyPolicy

    private static async Task SeedPrivacyPolicyAsync(ApplicationDbContext context)
    {
        if (await context.PrivacyPolicies.AnyAsync()) return;

        var privacy = PrivacyPolicy.Create(
            descriptionAr: "سياسة الخصوصية الخاصة ببريدك إكسبريس.",
            descriptionEn: "Privacy policy of Baridik Express.");

        await context.PrivacyPolicies.AddAsync(privacy);
        await context.SaveChangesAsync();
    }

    #endregion

    #region ShippingPolicy

    private static async Task SeedShippingPolicyAsync(ApplicationDbContext context)
    {
        if (await context.ShippingPolicies.AnyAsync()) return;

        var shipping = ShippingPolicy.Create(
            descriptionAr: "سياسة الشحن الخاصة ببريدك إكسبريس.",
            descriptionEn: "Shipping policy of Baridik Express.");

        await context.ShippingPolicies.AddAsync(shipping);
        await context.SaveChangesAsync();
    }

    #endregion

    #region SalesAndPurchasePolicy

    private static async Task SeedSalesAndPurchasePolicyAsync(ApplicationDbContext context)
    {
        if (await context.SalesAndPurchasePolicies.AnyAsync()) return;

        var policy = SalesAndPurchasePolicy.Create(
            descriptionAr: "سياسة البيع والشراء الخاصة ببريدك إكسبريس.",
            descriptionEn: "Sales and purchase policy of Baridik Express.");

        await context.SalesAndPurchasePolicies.AddAsync(policy);
        await context.SaveChangesAsync();
    }

    #endregion

    #region Help

    private static async Task SeedHelpAsync(ApplicationDbContext context)
    {
        if (await context.Help.AnyAsync()) return;

        var help = Help.Create(
            descriptionAr: "مركز المساعدة الخاص ببريدك إكسبريس.",
            descriptionEn: "Help center of Baridik Express.");

        await context.Help.AddAsync(help);
        await context.SaveChangesAsync();
    }

    #endregion

    #region DeliveryDriverRegistrationTerms

    private static async Task SeedDeliveryDriverRegistrationTermsAsync(ApplicationDbContext context)
    {
        if (await context.DeliveryDriverRegistrationTerms.AnyAsync()) return;

        var terms = DeliveryDriverRegistrationTerms.Create(
            descriptionAr: "شروط تسجيل مندوب التوصيل.",
            descriptionEn: "Delivery driver registration terms.");

        await context.DeliveryDriverRegistrationTerms.AddAsync(terms);
        await context.SaveChangesAsync();
    }

    #endregion

    #region CustomerRegistration

    private static async Task SeedCustomerRegistrationAsync(ApplicationDbContext context)
    {
        if (await context.CustomerRegistrationTerms.AnyAsync()) return;

        var terms = CustomerRegistration.Create(
            descriptionAr: "شروط تسجيل العميل.",
            descriptionEn: "Customer registration terms.");

        await context.CustomerRegistrationTerms.AddAsync(terms);
        await context.SaveChangesAsync();
    }

    #endregion

    #region SocialMediaLinks

    private static async Task SeedSocialMediaLinksAsync(ApplicationDbContext context)
    {
        if (await context.SocialMediaLinks.AnyAsync()) return;

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
        await context.SaveChangesAsync();
    }

    #endregion
}