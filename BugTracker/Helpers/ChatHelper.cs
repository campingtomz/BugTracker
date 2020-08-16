using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using BugTracker.Helpers;
using Microsoft.AspNet.Identity;
namespace BugTracker.Helpers
{
    public class ChatHelper
    { UserHelper userHelper = new UserHelper();
        //public bool chatExists(string user2Id)
        //{
        //    var user = userHelper.getUser(HttpContext.Current.User.Identity.GetUserId());
        //    if (user.UserChats.Any(u => u.User2Id == user2Id))
        //    {
        //        foreach (var chatList in user.UserChats.Where(uc => uc.User2Id == user2Id).ToList())
        //        {
        //            if (!chatList.isArchived)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
        //public ApplicationUserManager GetNewestReplyUser(UserChat Chat)
        //{
        //    var latestMessage = Chat.Messages.Where(m => m.UserId != HttpContext.Current.User.Identity.GetUserId()).;
        //}
    }
}