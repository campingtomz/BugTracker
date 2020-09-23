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
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}
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
            SeedHelper seedHelper = new SeedHelper();
            var rand = new Random();
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
                new Project() { Name = "User Interface for Banking", Created = DateTime.Now.AddDays(-60), DueDate = DateTime.Now.AddDays(-10), IsArchive = true },
                new Project() { Name = "Commercial Blog Site", Created = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Financial API", Created = DateTime.Now.AddDays(-45), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Sunset Light calculator", Created = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Factorial Calculator", Created = DateTime.Now.AddDays(-4), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Movie Comparer", Created = DateTime.Now.AddDays(-60), DueDate = DateTime.Now.AddDays(-10), IsArchive = true },
                new Project() { Name = "Fizz Buzz", Created = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Tacos and Cats", Created = DateTime.Now.AddDays(-45), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Fear the sums", Created = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(+100) },
                new Project() { Name = "Finacial portal", Created = DateTime.Now.AddDays(-4), DueDate = DateTime.Now.AddDays(+100) }
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
                    AvatarPath = "/Avatars/DemoUserAvatars/1.jpg",
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
                    AvatarPath = "/Avatars/DemoUserAvatars/2.jpg",
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
                    AvatarPath = "/Avatars/DemoUserAvatars/3.jpg",
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
                    AvatarPath = "/Avatars/DemoUserAvatars/4.jpg",
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
                    AvatarPath = "/Avatars/DemoUserAvatars/5.jpg",
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
                    AvatarPath = "/Avatars/DemoUserAvatars/6.jpg",
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
                        Description = "Developer",
                        AvatarPath = "/Avatars/DemoUserAvatars/7.jpg",
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
                        Description = "ProjectManager",
                        AvatarPath = "/Avatars/DemoUserAvatars/8.jpg",
                        PhoneNumber = RandomPhoneNumber()

                    },
                    "123456Abc$"); ;


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
                        Description = "Submitter",

                        LastName = $"{name.Item2}",
                        AvatarPath = "/Avatars/DemoUserAvatars/d1.jpg",
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
            foreach (var project in context.Projects.ToList())
            {
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
            List<string> supportIssues = new List<string>() { "The Login is not working for 3rd party", "Tickets do not Display text for zero notifications or histories",
                "Project names are not displayed right", "Need to add Users to projects", "add verification for file/image upload",
                "add a new Helper class for notifications", "Seed method duplicating notifications when update-database is ran","Setup default Avatar images for users",
                "When uploading custom avatar image, it does not update database", "upload database to Azure","test"};

            List<string> supportDiscription = new List<string>() { "The 3rd party login is not connecting to the correct service.","Where there are zero notifications or histories for a ticket. It is displaying a empty data table"
            ,"The project names are displaying the incorrect value.","create a seed method to add users to projects. Also create a manage project to add users to the project","File/image verification. so only certain files formats are uploaded. ",
                "Create a helper class that deals with all the methods for creating and displaying notifications for tickets, projects and users.","when updateDatabase is ran, it is duplicating methods in the seed methods, causing issues with projects and tickets."
                ,"There is no default Avatar image set up for the users. Add a feature in registry and manage user profile for uploading an avatar. if no avatar is uploaded add a default image","the custom avatar image path is not being updated in the database.",
            "create a new database in Azure and upload the current working database to it.","test"};
            List<TicketPriority> ticketPriorities = ticketHelper.ListTicketProities();
            List<TicketType> ticketTypes = ticketHelper.ListTicketTypes();
            var StatusId = context.TicketStatuses.Where(ts => ts.Name == "Open").FirstOrDefault().Id;
            foreach (var project in context.Projects.ToList())
            {
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
                    seedTicket.Issue = supportIssues[i];
                    seedTicket.IssueDescription = supportDiscription[i];
                    seedTicket.IsArchived = false;
                    seedTicket.IsResolved = false;
                    seedTicket.ProjectId = project.Id;
                    context.Tickets.Add(seedTicket);
                }
            }
            #endregion
            context.SaveChanges();
            #region seed notifications and Histories
            #region project creation notification
            foreach (var project in context.Projects.ToList())
            {
                foreach (var user in project.Users)
                {
                    var newNotification = new ProjectNotification()
                    {
                        ProjectId = project.Id,
                        UserId = user.Id,
                        Created = DateTime.Now,
                        Icon = "fa-sitemap",
                        NotificationType = "success",
                        Subject = $"Added to Project Id: {project.Id}",
                        Message = $"Hello, {user.FullName} you have been Added to the project: {project.Name}",
                    };
                    context.Notifications.Add(newNotification);
                }

            }
            context.SaveChanges();
            #endregion
            #region project edit add users, remove users. 
            foreach (var project in context.Projects.ToList())
            {
                List<ApplicationUser> UsersNotInProject = projectHelper.ListUsesNotOnProjectInExceptInRole(project.Id, "ProjectManager").ToList();
                List<ApplicationUser> oldUserList = projectHelper.ListUserOnProjectExceptInRole(project.Id, "ProjectManager").ToList();

                var oldProject = context.Projects.AsNoTracking().FirstOrDefault(p => p.Id == project.Id);
                for (int i = 0; i < 5; i++)
                {
                    var userAddId = UsersNotInProject[rand.Next(UsersNotInProject.Count)].Id;
                    projectHelper.RemoveUserFromProject(oldUserList[rand.Next(oldUserList.Count)].Id, project.Id);
                    projectHelper.AddUserToProject(userAddId, project.Id);
                    context.SaveChanges();
                }

                project.DueDate = DateTime.Now;
                project.Name = $"{project.Name}v2";
                project.Description = $"{project.Description} Description has been changed to this";
                context.SaveChanges();
                var newProject = context.Projects.AsNoTracking().FirstOrDefault(p => p.Id == project.Id);
                seedHelper.ProjectHistoriesEdit(oldProject, newProject);
                seedHelper.ProjectChangedNotification(newProject, oldProject, oldUserList);
            }
            context.SaveChanges();
            #endregion

            #region seed ticket creation Notifications
            foreach (var ticket in context.Tickets)
            {
                seedHelper.NewTicketNotification(ticket);
            }
            #endregion

            #region seed ticket edit notifications and Histories
            var TicketPriorities = context.TicketPriorities.ToList();
            var TicketStatuses = context.TicketStatuses.ToList();
            var TicketTypes = context.TicketTypes.ToList();
            foreach (var ticket in context.Tickets.ToList().Take(10))
            {
                var oldTicket = context.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                List<ApplicationUser> projectDevelopers = projectHelper.ListUserOnProjectInRole(ticket.project.Id, "Developer").ToList();

                //ticket.TicketPriorityId;
                ticket.TicketPriorityId = TicketPriorities[rand.Next(TicketPriorities.Count)].Id;
                ticket.TicketStatusId = TicketStatuses[rand.Next(TicketStatuses.Count)].Id;
                ticket.TicketTypeId = TicketTypes[rand.Next(TicketTypes.Count)].Id;
                ticket.DeveloperId = projectDevelopers[rand.Next(projectDevelopers.Count)].Id;
                context.SaveChanges();
                var newTicket = context.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                seedHelper.TicketHistoryEdit(oldTicket, newTicket);
                seedHelper.TicketChangeNotification(oldTicket, newTicket);
            }
            context.SaveChanges();
            #endregion
            #endregion
            #region seed ticket comments and attachments
            List<Project> projects = context.Projects.ToList();
            foreach (var project in projects)
            {
                List<ApplicationUser> UsersInProject = projectHelper.ListUsersOnProject(project.Id);
                var tickets = context.Tickets.Where(t=>t.ProjectId == project.Id).ToList();
                
                    TicketComment newTicket0Comment = new TicketComment()
                    {
                        TicketId = tickets[0].Id,
                        UserId = tickets[0].DeveloperId,
                        Comment = "Checked the Code for the 3rd part ports and SMTP server. Looks correct. ",
                        Created = DateTime.Now
                    };
                context.TicketComments.Add(newTicket0Comment);
                //TicketAttachment newTicketAttachment0 = new TicketAttachment()
                //{
                //    TicketId = tickets[0].Id,
                //    UserId = tickets[0].DeveloperId,
                //    Created = DateTime.Now,
                //    FilePath =
                //};
                //context.TicketAttachments.Add(newTicketAttachment0);

                TicketComment newTicket0Comment2 = new TicketComment()
                {
                    TicketId = tickets[0].Id,
                    UserId = tickets[0].DeveloperId,
                    Comment = "Checked Account controller. The 3rd party login code was commented out. un-commented and it is working ",
                    Created = DateTime.Now
                };
                context.TicketComments.Add(newTicket0Comment2);

                TicketComment newTicket1Comment1 = new TicketComment()
                {
                    TicketId = tickets[1].Id,
                    UserId = tickets[1].DeveloperId,
                    Comment = "looked at the Controller and the view for tickets. Created a If statement in the view",
                    Created = DateTime.Now
                };
                context.TicketComments.Add(newTicket1Comment1);
                TicketAttachment newTicket1Attachment0 = new TicketAttachment()
                {
                    TicketId = tickets[1].Id,
                    UserId = tickets[1].DeveloperId,
                    Created = DateTime.Now,
                    FilePath = "/Uploads/ticketif.PNG",
                    FileName = "ticketif.PNG",
                    Description = "ticketif.PNG"
                };
                context.TicketAttachments.Add(newTicket1Attachment0);
                TicketAttachment newTicket1Attachment1 = new TicketAttachment()
                {
                    TicketId = tickets[1].Id,
                    UserId = tickets[1].DeveloperId,
                    Created = DateTime.Now,
                    FilePath = "/Uploads/attachmentview.PNG",
                    FileName = "attachmentview.PNG",
                    Description = "attachmentview.PNG"
                };
                context.TicketAttachments.Add(newTicket1Attachment1);

                TicketComment newTicket1Comment2 = new TicketComment()
                {
                    TicketId = tickets[1].Id,
                    UserId = tickets[1].DeveloperId,
                    Comment = "Found that I needed to make sure the list of notifications or histories where checked against null and length of Zero",
                    Created = DateTime.Now
                };
                context.TicketComments.Add(newTicket1Comment2);
                TicketComment newTicket2Comment0 = new TicketComment()
                {
                    TicketId = tickets[2].Id,
                    UserId = tickets[2].DeveloperId,
                    Comment = "Checked the View, it was not referencing the project name, it was looking at a null object",
                    Created = DateTime.Now
                };
                context.TicketComments.Add(newTicket2Comment0);
                TicketComment newTicket3Comment0 = new TicketComment()
                {
                    TicketId = tickets[3].Id,
                    UserId = tickets[3].DeveloperId,
                    Comment = "Created a Seed section for adding users to a project",
                    Created = DateTime.Now
                };
                context.TicketComments.Add(newTicket3Comment0);
                TicketAttachment newTicket3Attachment1 = new TicketAttachment()
                {
                    TicketId = tickets[1].Id,
                    UserId = tickets[1].DeveloperId,
                    Created = DateTime.Now,
                    FilePath = "/Uploads/seetprojectuser.PNG",
                    FileName = "seetprojectuser.PNG",
                    Description = "seetprojectuser.PNG"

                };
                context.TicketAttachments.Add(newTicket3Attachment1);
                TicketAttachment newTicket3Attachment2 = new TicketAttachment()
                {
                    TicketId = tickets[1].Id,
                    UserId = tickets[1].DeveloperId,
                    Created = DateTime.Now,
                    FilePath = "/Uploads/projectviewListBox.PNG",
                    FileName = "projectviewListBox.PNG",
                    Description = "projectviewListBox.PNG"
                };
                context.TicketAttachments.Add(newTicket3Attachment2);
                TicketComment newTicket3Comment1 = new TicketComment()
                {
                    TicketId = tickets[3].Id,
                    UserId = tickets[3].DeveloperId,
                    Comment = "Created a multiselect List in the create and edit for projects users and users not in the project ",
                    Created = DateTime.Now
                };
                context.TicketComments.Add(newTicket3Comment1);
                TicketComment newTicket3Comment2 = new TicketComment()
                {
                    TicketId = tickets[3].Id,
                    UserId = tickets[3].DeveloperId,
                    Comment = "Created a method in the post that looped through the selected users added to project ",
                    Created = DateTime.Now
                };
                context.TicketComments.Add(newTicket3Comment2);
                TicketComment newTicket3Comment3 = new TicketComment()
                {
                    TicketId = tickets[3].Id,
                    UserId = tickets[3].DeveloperId,
                    Comment = "in the method, removed all the previous user then add the new list from the edit and create. It is working now ",
                    Created = DateTime.Now
                };
                context.TicketComments.Add(newTicket3Comment3);
                TicketComment newTicket4Comment0 = new TicketComment()
                {
                    TicketId = tickets[4].Id,
                    UserId = tickets[4].DeveloperId,
                    Comment = "In the register and edit user viewModels added a Avatar HttpPostedFileBase property",
                    Created = DateTime.Now
                };
                context.TicketComments.Add(newTicket4Comment0);
                TicketComment newTicket4Comment1 = new TicketComment()
                {
                    TicketId = tickets[4].Id,
                    UserId = tickets[4].DeveloperId,
                    Comment = "Created helper files FileUploadValidator, sluggerHelper, StringUtilites",
                    Created = DateTime.Now
                };
                context.TicketComments.Add(newTicket4Comment1);
                TicketComment newTicket4Comment2 = new TicketComment()
                {
                    TicketId = tickets[4].Id,
                    UserId = tickets[4].DeveloperId,
                    Comment = "Added the method to verify the input file for the avatar and create a custom name. then added it to the User",
                    Created = DateTime.Now
                };
                context.TicketComments.Add(newTicket4Comment2);
                context.SaveChanges();

            }
            #endregion

        }
    }
}

