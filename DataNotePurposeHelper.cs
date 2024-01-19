using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnCore_Replacement
{
	public static class DataNotePurposeHelper
	{
		public static string GetDescription(DataNotePurpose value)
		{
			var field = value.GetType().GetField(value.ToString());
			var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

			return attribute == null ? value.ToString() : attribute.Description;
		}
	}
}
