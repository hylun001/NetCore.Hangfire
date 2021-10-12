using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Hangfire.Interface
{
	public interface ICustomService
	{
		bool SayMessage(string msg);
	}
}
