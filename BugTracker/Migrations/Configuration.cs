namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BugTracker.Helpers;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            UserRoleHelper roleHelper = new UserRoleHelper();
            ProjectHelper projectHelper = new ProjectHelper();
            TicketHelper ticketHelper = new TicketHelper();
            #region User Roles

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
            if (!context.Roles.Any(r => r.Name == "No-Role"))
            {
                roleManager.Create(new IdentityRole { Name = "Default" });
            }
            #endregion
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
            context.SaveChanges();
            #region TicketType Seed
            context.TicketTypes.AddOrUpdate(
                tt => tt.Name,
                new TicketType() { Name = "Software" },
                 new TicketType() { Name = "Hardware" },
                  new TicketType() { Name = "UI" },
                   new TicketType() { Name = "Defect" },
                    new TicketType() { Name = "Feature Request" },
                    new TicketType() { Name = "Other" }
                );

            #endregion
            #region TicketPriority Seed
            context.TicketPriorities.AddOrUpdate(
                tp => tp.Name,
                new TicketPriority() { Name = "Low" },
                 new TicketPriority() { Name = "Medium" },
                  new TicketPriority() { Name = "High" },
                   new TicketPriority() { Name = "On Hold" }
                );
            #endregion
            #region TicketStatus Seed
            context.TicketStatuses.AddOrUpdate(
                ts => ts.Name,
                new TicketStatus() { Name = "Open" },
                 new TicketStatus() { Name = "Assigned" },
                  new TicketStatus() { Name = "Resolved" },
                   new TicketStatus() { Name = "Reopened" },
                    new TicketStatus() { Name = "Archived" }
                );
            #endregion
            #region Project Seed
            context.Projects.AddOrUpdate(
                p => p.Name,
                new Project() { Name = "Seed1", Created = DateTime.Now.AddDays(-60), DueDate = DateTime.Now.AddDays(-10), IsArchive = true },
                new Project() { Name = "Seed2", Created = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Seed3", Created = DateTime.Now.AddDays(-45), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Seed4", Created = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Seed5", Created = DateTime.Now.AddDays(-4), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Seed6", Created = DateTime.Now.AddDays(-60), DueDate = DateTime.Now.AddDays(-10), IsArchive = true },
                new Project() { Name = "Seed7", Created = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Seed8", Created = DateTime.Now.AddDays(-45), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Seed9", Created = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Seed10", Created = DateTime.Now.AddDays(-4), DueDate = DateTime.Now.AddDays(+100) }
                );
            #endregion
            context.SaveChanges();
            #region user seed
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
                    AvatarPath = "/Images/Default_Avatar.png",
                    PhoneNumber = "(111)111-1111"


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
                    AvatarPath = "/Images/Default_Avatar.png",
                    PhoneNumber = "(111)111-1111"


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
                    AvatarPath = "/Images/Default_Avatar.png",
                    PhoneNumber = "(111)111-1111"


                },
                "123456Abc$");


            }
            userId = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Moderator");


            for (int i = 0; i < 10; i++)
            {
                var emailCurr = $"bugtrackerdev{i}@mailinator.com";
                if (!context.Users.Any(u => u.Email == emailCurr))
                {
                    userManager.Create(new ApplicationUser()
                    {
                        Email = $"bugtrackerdev{i}@mailinator.com",
                        UserName = $"bugtrackerdev{i}@mailinator.com",
                        FirstName = $"dev{i}",
                        LastName = "dev",
                        AvatarPath = "/Images/Default_Avatar.png",
                        PhoneNumber = "(111)111-1111"

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
                        AvatarPath = "/Images/Default_Avatar.png",
                        PhoneNumber = "(111)111-1111"

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
                        AvatarPath = "/Images/Default_Avatar.png",
                        PhoneNumber = "(111)111-1111"

                    },
                    "123456Abc$");


                }
                userId = userManager.FindByEmail(emailCurr).Id;
                userManager.AddToRole(userId, "Submitter");
            }
            #endregion
            context.SaveChanges();
            #region add users to projects
            List<ApplicationUser> ProjectManagers = roleHelper.UsersInRole("ProjectManager").ToList();
            List<ApplicationUser> Developers = roleHelper.UsersInRole("Developer").ToList();
            List<ApplicationUser> Submitters = roleHelper.UsersInRole("Submitter").ToList();
            foreach (var project in context.Projects)
            {
                var rand = new Random();
                projectHelper.AddUserToProject(ProjectManagers[rand.Next(ProjectManagers.Count)].Id, project.Id);
                for (int i = 0; i < 3; i++)
                {
                    var randDev = rand.Next(Developers.Count);
                    var randSub = rand.Next(Submitters.Count);
                    projectHelper.AddUserToProject(Developers[randDev].Id, project.Id);
                    projectHelper.AddUserToProject(Submitters[randSub].Id, project.Id);
                }
            }

            context.SaveChanges();
            #endregion
            #region seed tickets 10 tickets/project
            List<TicketPriority> ticketPriorities = ticketHelper.ListTicketProities();
            List<TicketType> ticketTypes = ticketHelper.ListTicketTypes();
            var StatusId = context.TicketStatuses.Where(ts => ts.Name == "Open").FirstOrDefault().Id;
            foreach (var project in context.Projects)
            {
                var rand = new Random();
                List<ApplicationUser> projectDevelopers = projectHelper.ListUserOnProjectInRole(project.Id, "Developer").ToList();
                List<ApplicationUser> projectSubmitters = projectHelper.ListUserOnProjectInRole(project.Id, "Submitter").ToList();

                for (int i = 0; i < 10; i++)
                {
                    Ticket seedTicket = new Ticket();
                    var randDev = rand.Next(projectDevelopers.Count);
                    var randSub = rand.Next(projectSubmitters.Count);

                    seedTicket.DeveloperId = projectDevelopers[randDev].Id;
                    seedTicket.SubmitterId = projectSubmitters[randSub].Id;

                    seedTicket.TicketStatusId = StatusId;
                    seedTicket.TicketPriorityId = ticketPriorities[rand.Next(ticketPriorities.Count)].Id;
                    seedTicket.TicketTypeId = ticketTypes[rand.Next(ticketTypes.Count)].Id; ;
                    seedTicket.Created = DateTime.Now;
                    seedTicket.Issue = $"There is an issue with {project.Name}";
                    seedTicket.IsArchived = false;
                    seedTicket.IsResolved = false;
                    seedTicket.ProjectId = project.Id;
                    context.Tickets.Add(seedTicket);
                }
            }
            context.SaveChanges();
            #endregion
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


        }
    }
}

