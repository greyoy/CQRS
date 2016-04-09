﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#region Copyright
// -----------------------------------------------------------------------
// <copyright company="cdmdotnet Limited">
//     Copyright cdmdotnet Limited. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#endregion
using System;
using System.CodeDom.Compiler;
using System.Linq;
using Cqrs.Configuration;
using Cqrs.DataStores;
using cdmdotnet.Logging;
using Cqrs.Services;
using Ninject.Modules;
using Ninject.Parameters;

namespace Northwind.Domain.Configuration
{
	[GeneratedCode("CQRS UML Code Generator", "1.601.864")]
	public partial class DomainNinjectModule : NinjectModule
	{
		#region Overrides of NinjectModule

		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public override void Load()
		{
			RegisterLogger();

			RegisterFactories();
			RegisterServices();
			RegisterRepositories();
			RegisterCqrsCommandHandlers();
		}

		#endregion

		/// <summary>
		/// Register the <see cref="ILogger"/>
		/// </summary>
		protected virtual void RegisterLogger()
		{
			bool isLoggerBound = Kernel.GetBindings(typeof(ILogger)).Any();
			if (!isLoggerBound)
			{
				Bind<ILogger>()
					.To<ConsoleLogger>()
					.InSingletonScope();
			}
		}

		/// <summary>
		/// Register the all factories
		/// </summary>
		public virtual void RegisterFactories()
		{
			Bind<Factories.IDomainDataStoreFactory>()
				.To<Factories.DomainSimplifiedSqlDataStoreFactory>()
				.InSingletonScope();

			Bind<Cqrs.Authentication.ISingleSignOnTokenFactory>()
				.To<Cqrs.Authentication.SingleSignOnTokenFactory>()
				.InSingletonScope();

			OnFactoriesRegistered();
		}

		partial void OnFactoriesRegistered();

		/// <summary>
		/// Register the all services
		/// </summary>
		public virtual void RegisterServices()
		{
			Bind<IUnitOfWorkService>()
				.To<UnitOfWorkService<Cqrs.Authentication.ISingleSignOnToken>>()
				.InThreadScope();

			Bind<Orders.Services.IOrderService>()
				.To<Orders.Services.OrderService>()
				.InSingletonScope();


			OnServicesRegistered();
		}

		partial void OnServicesRegistered();

		/// <summary>
		/// Register the all repositories
		/// </summary>
		public virtual void RegisterRepositories()
		{
			Bind<Orders.Repositories.IOrderRepository>()
				.To<Orders.Repositories.OrderRepository>()
				.InSingletonScope();

			OnRepositoriesRegistered();
		}

		partial void OnRepositoriesRegistered();

		/// <summary>
		/// Register the all Cqrs command handlers
		/// </summary>
		public virtual void RegisterCqrsCommandHandlers()
		{
			var dependencyResolver = Resolve<IDependencyResolver>();
			var registrar = new BusRegistrar(dependencyResolver);
			RegisterCqrsCommandHandlers(registrar);
		}

		/// <summary>
		/// Register the all Cqrs command handlers
		/// </summary>
		protected virtual void RegisterCqrsCommandHandlers(BusRegistrar registrar)
		{
			// This will load all the handlers from the domain assembly by reading all command handlers AND event handlers in the same assembly as the provided type
			registrar.Register(typeof(Orders.Commands.Handlers.CreateOrderCommandHandler));
			OnCqrsCommandHandlersRegistered();
		}

		partial void OnCqrsCommandHandlersRegistered();

		protected T Resolve<T>()
		{
			return (T)Resolve(typeof(T));
		}

		protected object Resolve(Type serviceType)
		{
			return Kernel.Resolve(Kernel.CreateRequest(serviceType, null, new Parameter[0], true, true)).SingleOrDefault();
		}
	}
}
