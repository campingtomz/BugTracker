namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            { 
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }
            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            #region demo-roles
            if (!context.Roles.Any(r => r.Name == "Demo-Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Demo-Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Demo-Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Demo-Moderator" });
            }
            if (!context.Roles.Any(r => r.Name == "Demo-ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "Demo-ProjectManager" });
            }
            if (!context.Roles.Any(r => r.Name == "Demo-Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Demo-Submitter" });
            }
            if (!context.Roles.Any(r => r.Name == "Demo-Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Demo-Developer" });
            }
            #endregion



            var userManager = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "AndrewRussell@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "AndrewRussell@coderfoundry.com",
                    UserName = "AndrewRussell@coderfoundry.com",
                    FirstName = "Andrew",
                    LastName = "Russell",
                    AvatarPath = "",
                    contactNumber = "111111111"


                },
                "HelloNurse!");


            }
            var userId = userManager.FindByEmail("AndrewRussell@coderfoundry.com").Id;
            userManager.AddToRole(userId, "ProjectManager");


            if (!context.Users.Any(u => u.Email == "thomas.j.zanis@gmail.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "thomas.j.zanis@gmail.com",
                    UserName = "thomas.j.zanis@gmail.com",
                    FirstName = "Thomas",
                    LastName = "Zanis",
                    AvatarPath = "",
                    contactNumber = "111111111"


                },
                "Tobeornot123!");


            }
            userId = userManager.FindByEmail("thomas.j.zanis@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            if (!context.Users.Any(u => u.Email == "moderator@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "moderator@coderfoundry.com",
                    UserName = "moderator@coderfoundry.com",
                    FirstName = "moderator",
                    AvatarPath = "",
                    contactNumber = "111111111"


                },
                "123456Abc$");


            }
            userId = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Moderator");


            for(int i = 0; i< 10; i++) {
                var emailCurr = $"bugtrackerdev{i}@mailinator.com";
                if (!context.Users.Any(u => u.Email == emailCurr))
                {
                    userManager.Create(new ApplicationUser()
                    {
                        Email = $"bugtrackerdev{i}@mailinator.com",
                        UserName = $"bugtrackerdev{i}@mailinator.com",
                        FirstName = $"dev{i}",
                        LastName = "dev",
                        AvatarPath = "",
                        contactNumber = "111111111"

                    },
                    "123456Abc$");


                }
                userId = userManager.FindByEmail(emailCurr).Id;
                userManager.AddToRole(userId, "Developer");
            }

            for (int i = 0; i < 10; i++)
            {
                var emailCurr = $"bugtrackerpm{i}@mailinator.com";
                if (!context.Users.Any(u => u.Email == emailCurr))
                {
                    userManager.Create(new ApplicationUser()
                    {
                        Email = $"bugtrackerpm{i}@mailinator.com",
                        UserName = $"bugtrackerpm{i}@mailinator.com",
                        FirstName = $"pm{i}",
                        LastName = "pm",
                        AvatarPath = "",
                        contactNumber = "111111111"

                    },
                    "123456Abc$"); 


                }
                userId = userManager.FindByEmail(emailCurr).Id;
                userManager.AddToRole(userId, "ProjectManager");
            }

            for (int i = 0; i < 10; i++)
            {
                var emailCurr = $"bugtrackersub{i}@mailinator.com";
                if (!context.Users.Any(u => u.Email == emailCurr))
                {
                    userManager.Create(new ApplicationUser()
                    {
                        Email = $"bugtrackersub{i}@mailinator.com",
                        UserName = $"bugtrackersub{i}@mailinator.com",
                        FirstName = $"sub{i}",
                        LastName = "sub",
                        AvatarPath ="",
                        contactNumber = "111111111"

                    },
                    "123456Abc$");


                }
                userId = userManager.FindByEmail(emailCurr).Id;
                userManager.AddToRole(userId, "Submitter");
            }


            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


        }
    }
}

   