using NetCore.Hangfire.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Hangfire.Service
{

	public class CustomService : ICustomService
	{
		public bool SayMessage(string msg)
		{
			Console.WriteLine("Say " + msg);
			return true;
		}
	}
}
