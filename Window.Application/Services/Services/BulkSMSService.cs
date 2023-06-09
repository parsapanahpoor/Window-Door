#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Data.Context;

namespace Window.Application.Services.Services;

#endregion

public class BulkSMSService : IBulkSMSService
{
	#region Ctor

	private readonly WindowDbContext _context;

	public BulkSMSService(WindowDbContext context)
	{
		_context= context;
	}

	#endregion

	#region Admin Side 



	#endregion
}
