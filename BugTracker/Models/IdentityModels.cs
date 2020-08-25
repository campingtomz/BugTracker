using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        #region Parents/Childrem
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<TicketAttachment> Attachments { get; set; }
        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<ProjectHistory> ProjectHistories { get; set; }
        public virtual ICollection<UserHistory> UserHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }
        public virtual ICollection<ProjectNotification> ProjectNotifications { get; set; }
        public virtual ICollection<Connection> Connections { get; set; }
        public virtual ICollection<Message> Messages { get; set; }


        #endregion
        #region Actuall Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarPath { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        #endregion

        #region Constructor
        public ApplicationUser()
        {
            Projects = new HashSet<Project>();
            Attachments = new HashSet<TicketAttachment>();
            TicketHistories = new HashSet<TicketHistory>();
            ProjectHistories = new HashSet<ProjectHistory>();
            UserHistories = new HashSet<UserHistory>();
            TicketNotifications = new HashSet<TicketNotification>();
            ProjectNotifications = new HashSet<ProjectNotification>();            
            Comments = new HashSet<TicketComment>();
            Connections = new HashSet<Connection>();
            Messages = new HashSet<Message>();


        }
        #endregion
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<ProjectHistory> ProjectHistories { get; set; }
        public DbSet<UserHistory> UserHistories { get; set; }
        public DbSet<TicketNotification> TicketNotifications { get; set; }
        public DbSet<ProjectNotification> ProjectNotifications { get; set; }
        public DbSet<AttachmentHistory> AttachmentHistories { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Message> Messages { get; set; }

    }
}