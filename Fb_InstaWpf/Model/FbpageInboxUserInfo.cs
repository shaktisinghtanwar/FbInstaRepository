using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Fb_InstaWpf.Model
{
   public class FbpageInboxUserInfo
    {
       public string InboxUserId { get; set; }
        public string InboxUserName { get; set; }
        public string InboxUserImage { get; set; }
        public string InboxNavigationUrl { get; set; }

    }
   public class ImageLoginTextbox
   {
       public string loginimageurl { get; set; }
   }

   public class FacebookPageInboxmember
   {

       public string Fbcomment_InboxUserId { get; set; }
       public string FbPageName { get; set; }
       public string FbPageImage { get; set; }
       public string FBInboxNavigationUrl { get; set; }

   }

   public class InstaInboxmember
   {
       public string Insta_inboxUserId { get; set; }
       public string InstaInboxUserName { get; set; }
       public string InstaInboxUserImage { get; set; }
       public string InstaInboxNavigationUrl { get; set; }
   }


   public class ListUsernameInfo
   {
       public string InboxNavigationUrl { get; set; }

       public string ListUsername { get; set; }

       public string ListUserId { get; set; }

       
 
      
   }


    public class FBListUsernameInfo
    {
        
    }


}
