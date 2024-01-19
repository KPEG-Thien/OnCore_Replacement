using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ingr.SP3D.Common.Client;
using Ingr.SP3D.Common.Middle;
using OnCore_Replacement.Views;

namespace OnCore_Replacement
{
    public class Program : BaseGraphicCommand
	{
		private Frm_Main frm = new Frm_Main();

		public override void OnStart(int instanceId, object argument)
		{
			frm.Show();
			base.OnStart(instanceId, argument);
		}

		public override void OnIdle()
		{
			base.OnIdle();
		}

		public override void OnStop()
		{
			base.OnStop();
		}
	}
}
