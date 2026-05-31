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
            await SeedFAQsAsync(context);

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

    #region FAQs

    private static async Task SeedFAQsAsync(ApplicationDbContext context)
    {
        if (await context.FAQs.AnyAsync())
            return;

        var faqs = new List<FAQ>
        {
            FAQ.Create(
                questionAr: "ما هي بريدك إكسبريس؟",
                questionEn: "What is Baridik Express?",
                answerAr: "بريدك إكسبريس شركة رائدة في مجال الشحن والتوصيل توفر خدمات لوجستية متكاملة.",
                answerEn: "Baridik Express is a leading shipping and delivery company providing integrated logistics services."),

            FAQ.Create(
                questionAr: "كيف يمكنني تتبع شحنتي؟",
                questionEn: "How can I track my shipment?",
                answerAr: "يمكنك تتبع شحنتك من خلال التطبيق أو الموقع الإلكتروني باستخدام رقم التتبع.",
                answerEn: "You can track your shipment through the app or website using your tracking number."),

            FAQ.Create(
                questionAr: "ما هي مناطق التوصيل المتاحة؟",
                questionEn: "What delivery areas are available?",
                answerAr: "نوفر خدمات التوصيل في جميع أنحاء المملكة العربية السعودية.",
                answerEn: "We provide delivery services across all regions of Saudi Arabia."),

            FAQ.Create(
                questionAr: "كيف يمكنني التسجيل كمندوب توصيل؟",
                questionEn: "How can I register as a delivery driver?",
                answerAr: "يمكنك التسجيل من خلال التطبيق واتباع خطوات التسجيل وتقديم المستندات المطلوبة.",
                answerEn: "You can register through the app by following the registration steps and submitting the required documents."),

            FAQ.Create(
                questionAr: "ما هي طرق الدفع المتاحة؟",
                questionEn: "What payment methods are available?",
                answerAr: "نقبل الدفع النقدي وبطاقات الائتمان والمدفوعات الإلكترونية.",
                answerEn: "We accept cash, credit cards, and electronic payments."),
        };

        await context.FAQs.AddRangeAsync(faqs);
    }

    #endregion
}