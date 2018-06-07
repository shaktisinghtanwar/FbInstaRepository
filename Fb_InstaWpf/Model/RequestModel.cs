using Fb_InstaWpf.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fb_InstaWpf.Model {
	class RequestModel {

		public TabType PlatformType
		{
			get;
			set;
		}

		public PostType PostType
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}
		public string ImageSource
		{
			get;
			set;
		}
	}

	//enum PlatformType {
	//FacebookPage,
	//FacebookMessenger,
	//Instagram
	//}

	enum PostType {
	Image,
	Text
	}

	public class MessageProcessor {
	
	void Enqueue(RequestModel request)
	{
		
		//Save to Job table

	    Process();
	}

	static void Process()
	{
	  //Get from job table
	  //Process
	  //Update job table
	}

	}

}
