using MedicalRecordsRepository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MedicalRecordsRepository
{
	public static class RepositoryModule
	{
		public static void AddCoreRepository(this IServiceCollection services)
		{
			//Inject Business Login Layers. Use Scoped for Repos that use EF (our BLL) since EF uses Scope.
			//See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1 for details

		}
	}
}
