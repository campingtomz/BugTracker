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
        private Tuple<string, string> generateRandomName()
        {
            var firstNames = new List<string>() { "Brandon", "Andrew", "Jason", "Glen", "Peter", "Wade", "Richard", "Jackson", "Adam", "Khoa", "Douglas", "Angelica", "Beth", "Jaylin", "Jeremy", "Kayla", "Kodi", "Thomas" };
            var lastNames = new List<string>() { "Russell", "Gallagher", "Velez", "Nguyen", "Olmo", "Swaney", "Dolteren", "Campbell", "Stewart", "Cooper", "Twichell", "Dennis", "McGraw", "Kane", "Gutherie", "Cranford", "Zanis" };
            var rand = new Random();
            return Tuple.Create(firstNames[rand.Next(firstNames.Count)], lastNames[rand.Next(lastNames.Count)]);
        }
        private string RandomPhoneNumber()
        {
            var rand = new Random();
            return $"({rand.Next(100, 1000)})-{rand.Next(100, 1000)}-{rand.Next(1000, 10000)}";
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
                    PhoneNumber = RandomPhoneNumber(),
                    Description = "Administrator of the Site"


                },
                "Tobeornot123!");
            }
            userId = userManager.FindByEmail("thomas.j.zanis@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");


            if (!context.Users.Any(u => u.Email == "DemoAdmin@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoAdmin@mailinator.com",
                    UserName = "DemoAdmin@mailinator.com",
                    FirstName = "Admin",
                    LastName = "Demo",
                    AvatarPath = "/Images/Default_Avatar.png",
                    PhoneNumber = RandomPhoneNumber(),
                    Description = "Demo Administrator of the Site"
                },
                "Dmrss6HDRwd");
            }
            userId = userManager.FindByEmail("DemoAdmin@mailinator.com").Id;
            userManager.AddToRole(userId, "Admin");


            if (!context.Users.Any(u => u.Email == "DemoSubmitter@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoSubmitter@mailinator.com",
                    UserName = "DemoSubmitter@mailinator.com",
                    FirstName = "Submitter",
                    LastName = "Demo",
                    AvatarPath = "/Images/Default_Avatar.png",
                    PhoneNumber = RandomPhoneNumber(),
                    Description = "Demo Submitter of the Site"


                },
                "Dmrss6HDRwd");
            }
            userId = userManager.FindByEmail("DemoSubmitter@mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");


            if (!context.Users.Any(u => u.Email == "DemoProjectManager@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoProjectManager@mailinator.com",
                    UserName = "DemoProjectManager@mailinator.com",
                    FirstName = "ProjectManager",
                    LastName = "Demo",
                    AvatarPath = "/Images/Default_Avatar.png",
                    PhoneNumber = RandomPhoneNumber(),
                    Description = "Demo ProjectManager of the Site"
                },
                "Dmrss6HDRwd");
            }
            userId = userManager.FindByEmail("DemoProjectManager@mailinator.com").Id;
            userManager.AddToRole(userId, "ProjectManager");


            if (!context.Users.Any(u => u.Email == "DemoDeveloper@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoDeveloper@mailinator.com",
                    UserName = "DemoDeveloper@mailinator.com",
                    FirstName = "Developer",
                    LastName = "Demo",
                    AvatarPath = "/Images/Default_Avatar.png",
                    PhoneNumber = RandomPhoneNumber(),
                    Description = "Demo Developer of the Site"


                },
                "Dmrss6HDRwd");
            }
            userId = userManager.FindByEmail("DemoDeveloper@mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");


            for (int i = 0; i < 10; i++)
            {
                var name = generateRandomName();
                var emailCurr = $"{name.Item1}{name.Item2}@mailinator.com";
                if (!context.Users.Any(u => u.Email == emailCurr))
                {
                    userManager.Create(new ApplicationUser()
                    {
                        Email = emailCurr,
                        UserName = emailCurr,
                        FirstName = $"{name.Item1}",
                        LastName = $"{name.Item2}",
                        AvatarPath = "/Images/Default_Avatar.png",
                        PhoneNumber = RandomPhoneNumber()

                    },
                    "123456Abc$");


                }
                userId = userManager.FindByEmail(emailCurr).Id;
                userManager.AddToRole(userId, "Developer");
            }

            for (int i = 0; i < 10; i++)
            {
                var name = generateRandomName();
                var emailCurr = $"{name.Item1}{name.Item2}@mailinator.com";
                if (!context.Users.Any(u => u.Email == emailCurr))
                {
                    userManager.Create(new ApplicationUser()
                    {
                        Email = emailCurr,
                        UserName = emailCurr,
                        FirstName = $"{name.Item1}",
                        LastName = $"{name.Item2}",
                        AvatarPath = "/Images/Default_Avatar.png",
                        PhoneNumber = RandomPhoneNumber()

                    },
                    "123456Abc$");


                }
                userId = userManager.FindByEmail(emailCurr).Id;
                userManager.AddToRole(userId, "ProjectManager");
            }

            for (int i = 0; i < 10; i++)
            {
                var name = generateRandomName();
                var emailCurr = $"{name.Item1}{name.Item2}@mailinator.com";
                if (!context.Users.Any(u => u.Email == emailCurr))
                {
                    userManager.Create(new ApplicationUser()
                    {
                        Email = emailCurr,
                        UserName = emailCurr,
                        FirstName = $"{name.Item1}",
                        LastName = $"{name.Item2}",
                        AvatarPath = "/Images/Default_Avatar.png",
                        PhoneNumber = RandomPhoneNumber()

                    },
                    "123456Abc$");


                }
                userId = userManager.FindByEmail(emailCurr).Id;
                userManager.AddToRole(userId, "Submitter");
            }
            #endregion
            context.SaveChanges();
            #region add connection sessions
            var user1 = userManager.Users.ToList().Where(u => u.Email == "thomas.j.zanis@gmail.com").FirstOrDefault();
            var user2 = userManager.Users.ToList().Where(u => u.Email == "DemoDeveloper@mailinator.com").FirstOrDefault();


            Connection seedConnection = new Connection();
                seedConnection.Users.Add(user1);
                seedConnection.Users.Add(user2);
                seedConnection.isArchived = false;
                seedConnection.Created = DateTime.Now;
                context.Connections.Add(seedConnection);

            
            #endregion
            context.SaveChanges();
            #region add Messages to Chat
            foreach (var connection in user1.Connections)
            {

                for (int j = 0; j < 10; j++)
                {
                    Message message = new Message();
                    message.Created = DateTime.Now;
                    message.ConnectionId = connection.Id;
                    if (j % 2 == 0)
                    {
                        message.SenderId = user1.Id;
                        message.Content = $"{user1.FullName} this is test {j}";
                    }
                    else
                    {

                        message.SenderId = user2.Id;
                        message.Content = $"{user2.FullName} this is test {j}";

                    }
                    context.Messages.Add(message);
                }
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
#endregion
            context.SaveChanges();   
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
                    seedTicket.TicketTypeId = ticketTypes[rand.Next(ticketTypes.Count)].Id;
                    seedTicket.Created = DateTime.Now;
                    seedTicket.Issue = $"There is an issue with {project.Name}";
                    seedTicket.IsArchived = false;
                    seedTicket.IsResolved = false;
                    seedTicket.ProjectId = project.Id;
                    context.Tickets.Add(seedTicket);
                }
            }
            
            #endregion
            context.SaveChanges();
            

        }
    }
}

