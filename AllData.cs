using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnCore_Replacement
{
	public static class AllData
	{
		private static string nameServer;
		private static string dbName;

		public static string NameServer { get => nameServer; set => nameServer = value; }
		public static string DbName { get => dbName; set => dbName = value; }
	}
}
