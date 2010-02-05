using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;

namespace DrivingBarefoot.MembershipHelpers
{
    public class OnlineMembersWrapper
    {
        /// <summary>
        /// Return a MembershipUserCollection of currently online users
        /// </summary>
        /// <returns></returns>
        public static MembershipUserCollection GetOnlineUsers()
        {
            MembershipUserCollection members = System.Web.Security.Membership.GetAllUsers();
            MembershipUserCollection onlineMembers = new MembershipUserCollection();

            foreach (MembershipUser user in members)
            {
                if (user.IsOnline)
                    onlineMembers.Add(user);
            }

            return onlineMembers;
        }
    }
}
