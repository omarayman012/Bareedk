using BaridikExpress.Domain.Entities.Addresses;
using BaridikExpress.Domain.Entities.AuthModule;
using BaridikExpress.Domain.Entities.Banners;
using BaridikExpress.Domain.Entities.BlogsModules;
using BaridikExpress.Domain.Entities.CareerFields;
using BaridikExpress.Domain.Entities.ClientModule;
using BaridikExpress.Domain.Entities.ContactUs;
using BaridikExpress.Domain.Entities.CurrencyModule;
using BaridikExpress.Domain.Entities.Customers;
using BaridikExpress.Domain.Entities.DeliveryModule;
using BaridikExpress.Domain.Entities.DeliveryType;
using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Domain.Entities.Nationality;
using BaridikExpress.Domain.Entities.NotificationModules;
using BaridikExpress.Domain.Entities.PublishingHouseModule;
using BaridikExpress.Domain.Entities.RoleModule;
using BaridikExpress.Domain.Entities.Services;
using BaridikExpress.Domain.Entities.Shipments;
using BaridikExpress.Domain.Entities.SystemManagment;
using BaridikExpress.Domain.Entities.Vehicles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BaridikExpress.Application.Interfaces;

public interface IApplicationDbContext
{
    #region Auth & Users

    DbSet<User> ApplicationUsers { get; }
    DbSet<IdentityRole> Roles { get; }
    DbSet<RolePermission> RolePermissions { get; set; }
    DbSet<Permission> Permissions { get; set; }
    DbSet<IdentityUserRole<string>> UserRoles { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<SubAdminEmployee> SubAdminEmployees { get; set; }

    #endregion

    #region Location

    DbSet<Country> Countries { get; set; }
    DbSet<Government> Governments { get; set; }
    DbSet<City> Cities { get; set; }
    DbSet<Village> Villages { get; set; }
    DbSet<Nationality> Nationalities { get; set; }

    #endregion

    #region Customers

    DbSet<Customer> Customers { get; set; }
    DbSet<CustomerAccount> CustomerAccounts { get; set; }
    DbSet<CustomerAddress> CustomerAddresses { get; set; }
    DbSet<CustomerContact> CustomerContacts { get; set; }

    #endregion

    #region Delivery

    DbSet<Delivery> Deliveries { get; }
    DbSet<DeliveryType> DeliveryTypes { get; set; }

    #endregion

    #region Shipments

    DbSet<Shipment> Shipments { get; set; }
    DbSet<ShipmentAddress> ShipmentAddresses { get; set; }
    DbSet<ShipmentAttachment> ShipmentAttachments { get; set; }
    DbSet<ShipmentService> ShipmentServices { get; set; }
    DbSet<ShipmentStatusHistory> ShipmentStatusHistories { get; set; }

    #endregion

    #region Core Business

    DbSet<CareerField> CareerFields { get; set; }
    DbSet<Vehicle> Vehicles { get; set; }
    DbSet<Service> Services { get; set; }
    DbSet<ContactUs> ContactUs { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogsCategory> BlogsCategorys { get; set; }
    public DbSet<BlogComment> BlogComments { get; set; }
    public DbSet<BlogsAuthor> BlogsAuthors { get; set; }
    public DbSet<BlogSeo> BlogSeos { get; set; }
    public DbSet<BlogTag> BlogTags { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<CommentReaction> CommentReactions { get; set; }
    public DbSet<BlogReaction> BlogReactions { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    #endregion

    #region System Management

    DbSet<AboutUs> AboutUs { get; set; }
    DbSet<TermsAndConditions> TermsAndConditions { get; set; }
    DbSet<PrivacyPolicy> PrivacyPolicies { get; set; }
    DbSet<ShippingPolicy> ShippingPolicies { get; set; }
    DbSet<SalesAndPurchasePolicy> SalesAndPurchasePolicies { get; set; }
    DbSet<Help> Help { get; set; }
    DbSet<DeliveryDriverRegistrationTerms> DeliveryDriverRegistrationTerms { get; set; }
    DbSet<CustomerRegistration> CustomerRegistrationTerms { get; set; }
    DbSet<SocialMediaLinks> SocialMediaLinks { get; set; }
    DbSet<FAQ> FAQs { get; set; }
    DbSet<Currency> Currencies { get; set; }

    #endregion

    #region Client
    DbSet<Client> Clients { get; set; }

    DbSet<ClientAddress> ClientAddresses { get; set; }
    #endregion
    

    DbSet<Banner>  Banners { get; set; }
    DbSet<PublishingHouse> PublishingHouses { get; set; }

    public DbSet<SendNotification> SendNotifications { get; set; }
    public DbSet<NotificationRecipient> NotificationRecipients { get; set; }
    public DbSet<MessageNotification> MessageNotifications { get; set; }

    DatabaseFacade Database { get; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}