using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnCore_Replacement.DataSQL
{
	public class ConnectSQL
	{
		SqlConnection cn;

		public ConnectSQL(string connectionString)
		{
			cn = new SqlConnection(connectionString);
		}
		public bool IsConnection
		{
			get
			{
				if (cn.State == System.Data.ConnectionState.Closed)
					cn.Open();
				return true;
			}
		}
	}
}
