using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fb_InstaWpf.ViewModel;

namespace Fb_InstaWpf.Model
{
  public  class FbUserMessageInfo
    {
        public string UserName { get; set; }
        public int UserType { get; set; }//0 Login User 1 for Other user
        public string Message { get; set; }
        public string OtherUserDateTime { get; set; }

        public string otheruserId { get; set; }
        public string otheruserimage { get; set; }
        public string loginguserimage { get; set; }
    
      public string otheruserFbimage { get; set; }
      public string loginguserFbimage { get; set; }

      
      public string loginguserInstaimage { get; set; }
      public string otheruserInstaimage { get; set; }
   

    }

    //create class for contains facebookPage Information
  public class FbPageInfo : BaseViewModel
    {
        public string FbPageId { get; set; }
        public string FbPageName { get; set; }
        public string FbPageUrl { get; set; }
        
    }


 

    
}
