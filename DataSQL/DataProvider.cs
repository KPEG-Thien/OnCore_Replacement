using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace OnCore_Replacement.DataSQL
{
	public class DataProvider
	{
		private static DataProvider instance; // Ctrl + R + E

		public static DataProvider Instance
		{
			get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
			private set { DataProvider.instance = value; }
		}

		private DataProvider() { }

		public string ConnectionStr(string oDBname)
		{
			string oConnectionStr = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", AllData.NameServer, oDBname);
			return oConnectionStr;
		}
		public DataTable ExecuteQuery(string oDBname, string query, object[] parameter = null)
		{
			string connectionSTR = ConnectionStr(oDBname);
			DataTable data = new DataTable();
			using (SqlConnection connection = new SqlConnection(connectionSTR))
			{
				connection.Open();

				SqlCommand command = new SqlCommand(query, connection);

				if (parameter != null)
				{
					string[] listPara = query.Split(' ');
					int i = 0;
					foreach (string item in listPara)
					{
						if (item.Contains('@'))
						{
							command.Parameters.AddWithValue(item, parameter[i]);
							i++;
						}
					}
				}

				SqlDataAdapter adapter = new SqlDataAdapter(command);

				adapter.Fill(data);

				connection.Close();
			}

			return data;
		}

		public int ExecuteNonQuery(string oDBname, string query, object[] parameter = null)
		{
			int data = 0;
			string connectionSTR = ConnectionStr(oDBname);
			using (SqlConnection connection = new SqlConnection(connectionSTR))
			{
				connection.Open();
				query = query.Replace("N''", "null");
				SqlCommand command = new SqlCommand(query, connection);
				if (parameter != null)
				{
					string[] listPara = query.Split(' ');
					int i = 0;
					foreach (string item in listPara)
					{
						if (item.Contains('@'))
						{
							command.Parameters.AddWithValue(item, parameter[i]);
							i++;
						}
					}
				}

				data = command.ExecuteNonQuery();

				connection.Close();
			}

			return data;
		}

		public object ExecuteScalar(string oDBname, string query, object[] parameter = null)
		{
			object data = 0;
			string connectionSTR = ConnectionStr(oDBname);
			using (SqlConnection connection = new SqlConnection(connectionSTR))
			{
				connection.Open();

				SqlCommand command = new SqlCommand(query, connection);

				if (parameter != null)
				{
					string[] listPara = query.Split(' ');
					int i = 0;
					foreach (string item in listPara)
					{
						if (item.Contains('@'))
						{
							command.Parameters.AddWithValue(item, parameter[i]);
							i++;
						}
					}
				}

				data = command.ExecuteScalar();

				connection.Close();
			}

			return data;
		}

		public DataTable LoadDataReplacement()
		{
			string query = "select po.Oid as tbpartOid\r\n,j1.ItemName as tbpartName\r\n,po.tbobjType \r\n,ppf.Oid as tbfeatOid\r\n,ppf.Tag as tbTag\r\n,ISNULL(cTag.TagNumber,'No') as tbIsTagAvailableInCatalog ,xop.OidOrigin as tbrunOid\r\n,j2.ItemName as tbrunName\r\n,xsh.OidOrigin as tblineOid\r\n,j3.ItemName as tblineName" + "\n" +
				"from \r\n(\r\nselect ri.Oid\r\n,'Instrument' as tbobjType \r\nfrom JRteInstrument ri\r\nunion\r\nselect rs.Oid \r\n,'Specialty' as tbobjType\r\nfrom JRteSpecialtyOccur rs\r\n)po " + "\n" +
				"join JNamedItem j1 on j1.Oid  = po.Oid" + "\n" +
				"join XPathGeneratedParts pgp on pgp.OidDestination = po.Oid" + "\n" +
				"join JRtePipePathFeat ppf on ppf.Oid = pgp.OidOrigin" + "\n" +
				"join XOwnsParts xop on xop.OidDestination = po.Oid" + "\n" +
				"join JNamedItem j2 on j2.Oid = xop.OidOrigin" + "\n" +
				"join XSystemHierarchy xsh on xsh.OidDestination = xop.OidOrigin" + "\n" +
				"join JNamedItem j3 on j3.Oid  = xsh.OidOrigin" + "\n" +
				"left join\r\n(\r\n\tselect ic.Oid \r\n\t, case \r\n\t\twhen ic.TagNumber ='' then ic.GenericTagNumber\r\n\t\telse ic.TagNumber\r\n\tend as TagNumber\r\n\t,'instrument' as itemType\r\n\t from JInstrumentClass ic\r\n\twhere ic.TagNumber <> '' or ic.GenericTagNumber <> ''\r\n\tunion\r\n\tselect pc.Oid \r\n\t, case \r\n\t\twhen pc.TagNumber ='' then pc.GenericTagNumber\r\n\t\telse pc.TagNumber\r\n\tend as TagNumber\r\n\t,'Specialty' as itemType\r\n\t from JPipingSpecialtyClass pc\r\n\twhere pc.TagNumber <> '' or pc.GenericTagNumber <> ''\r\n) cTag on j1.ItemName = cTag.TagNumber ";
			DataTable tb = ExecuteQuery(AllData.DbName, query, new object[] { });
			return tb;
		}

		public DataTable LoadDataBeforeReplacement()
		{
			string query = "select po.Oid as partOid\r\n,j1.ItemName as partName\r\n,po.objType \r\n,ppf.Oid as featOid\r\n,ppf.Tag\r\n,om.MatingQty\r\n,ISNULL(ob.BoltPortQty,0) as BoltPortQty\r\n,ISNULL(cTag.TagNumber,'No') as IsTagReplaced\r\n,'No' as featOidReplaced\r\n,xop.OidOrigin as runOid\r\n,j2.ItemName as runName\r\n,xsh.OidOrigin as lineOid\r\n,j3.ItemName as lineName" + "\n" +
				"from \r\n(\r\nselect ri.Oid\r\n,'Instrument' as objType \r\nfrom JRteInstrument ri\r\nunion\r\nselect rs.Oid \r\n,'Specialty' as objType\r\nfrom JRteSpecialtyOccur rs\r\n)po " + "\n" +
				"join JNamedItem j1 on j1.Oid  = po.Oid" + "\n" +
				"join XPathGeneratedParts pgp on pgp.OidDestination = po.Oid" + "\n" +
				"join JRtePipePathFeat ppf on ppf.Oid = pgp.OidOrigin" + "\n" +
				"join XOwnsParts xop on xop.OidDestination = po.Oid" + "\n" +
				"join JNamedItem j2 on j2.Oid = xop.OidOrigin" + "\n" +
				"join XSystemHierarchy xsh on xsh.OidDestination = xop.OidOrigin" + "\n" +
				"join JNamedItem j3 on j3.Oid  = xsh.OidOrigin " + "\n" +
				"join\r\n(\r\n\tselect \r\n\tppf.Oid \r\n\t,count(ppf.Oid)-1 as MatingQty\r\n\t from JRtePipePathFeat ppf\r\n\tjoin XPathGeneratedParts xpgp on xpgp.OidOrigin  = ppf.Oid \r\n\tjoin JNamedItem j1 on j1.Oid  = xpgp.OidDestination \r\n\tjoin JRtePathGenPart rpgp on rpgp.Oid = xpgp.OidDestination \r\n\tgroup by ppf.Oid\r\n) om on om.Oid = ppf.Oid" + "\n" +
				"left join\r\n(\r\n\tselect\r\n\tj1.Oid as partOid,\r\n\tcount(j1.Oid) as BoltPortQty\r\n\tfrom JRtePathGenPart j1\r\n\tjoin JDistribPort j2 on j2.oidOwner = j1.Oid\r\n\tjoin JDPipePort_CL j3 on j3.Oid = j2.oid\r\n\twhere j3.TerminationClass = 5\r\n\tgroup by j1.Oid \r\n) ob on ob.partOid = po.Oid" + "\n" +
				"left join\r\n(\r\n\tselect ic.Oid \r\n\t, case \r\n\t\twhen ic.TagNumber ='' then ic.GenericTagNumber\r\n\t\telse ic.TagNumber\r\n\tend as TagNumber\r\n\t,'instrument' as itemType\r\n\t from JInstrumentClass ic\r\n\twhere ic.TagNumber <> '' or ic.GenericTagNumber <> ''\r\n\tunion\r\n\tselect pc.Oid \r\n\t, case \r\n\t\twhen pc.TagNumber ='' then pc.GenericTagNumber\r\n\t\telse pc.TagNumber\r\n\tend as TagNumber\r\n\t,'Specialty' as itemType\r\n\t from JPipingSpecialtyClass pc\r\n\twhere pc.TagNumber <> '' or pc.GenericTagNumber <> ''\r\n) cTag on j1.ItemName = cTag.TagNumber";
			DataTable dt = ExecuteQuery(AllData.DbName, query, new object[] { });
			return dt;
		}

		public DataTable LoadDataOIDObject()
		{
			string query = "select po.Oid as tbpartOid\r\n,ppf.Oid as tbfeatOid\r\n,ppf.Tag as tbTag" + "\n" +
				"from \r\n(\r\nselect ri.Oid\r\nfrom JRteInstrument ri\r\nunion\r\nselect rs.Oid \r\nfrom JRteSpecialtyOccur rs\r\n)po" + "\n" +
				"join XPathGeneratedParts pgp on pgp.OidDestination = po.Oid \r\njoin JRtePipePathFeat ppf on ppf.Oid = pgp.OidOrigin ";
			DataTable dt = ExecuteQuery(AllData.DbName, query, new object[] { });
			return dt;
		}

		public List<string> LoadDataTag()
		{
			List<string> lists = new List<string>();
			string query = "Select ShortStringValue From CL_FluidSystem";
			DataTable dt = DataProvider.Instance.ExecuteQuery("", query, new object[] { });
			// Convert the DataTable to a List<string>
			foreach (DataRow row in dt.Rows)
			{
				lists.Add(row.ItemArray[0].ToString().Trim());
			}

			return lists;
		}
	}
}
