using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnCore_Replacement
{
	public enum DataNotePurpose
	{
		[Description("")]
		None = 0,

		[Description("General")]
		General = 1,

		[Description("Design")]
		Design = 2,

		[Description("Fabrication")]
		Fabrication = 3,

		[Description("Installation")]
		Installation = 4,

		[Description("Operation and Maintenance")]
		OperationandMaintenance = 5,

		[Description("Inspection")]
		Inspection = 6,

		[Description("Remark")]
		Remark = 7,

		[Description("Material of Construction")]
		MaterialofConstruction = 8,

		[Description("Design Review")]
		DesignReview = 9,

		[Description("Piping Specification note")]
		PipingSpecificationnote = 10,

		[Description("Justification")]
		Justification = 11,

		[Description("Procurement")]
		Procurement = 12,

		[Description("Standard note")]
		Standardnote = 13
	}

	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class DescriptionAttribute : Attribute
	{
		public string Description { get; }

		public DescriptionAttribute(string description)
		{
			Description = description;
		}
	}

}
