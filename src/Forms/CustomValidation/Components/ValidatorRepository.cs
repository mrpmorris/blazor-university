using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CustomValidation.Components
{
	public static class ValidatorRepository
	{
		readonly static HashSet<Assembly> Assemblies;
		readonly static Dictionary<Type, HashSet<Type>> ValidatorTypesByModelType;

		static ValidatorRepository()
		{
			Assemblies = new HashSet<Assembly>();
			ValidatorTypesByModelType = new Dictionary<Type, HashSet<Type>>();
		}

		public static void AddValidatorsInAssembly(this IServiceCollection services, Assembly assembly)
		{
			if (Assemblies.Contains(assembly))
				return;
			ScanAssembly(services, assembly);
		}

		public static IEnumerable<Type> GetValidators(Type modelType)
		{
			if (ValidatorTypesByModelType.TryGetValue(modelType, out HashSet<Type> validatorTypes))
				return validatorTypes;

			return Array.Empty<Type>();
		}

		private static void ScanAssembly(IServiceCollection services, Assembly assembly)
		{
			Assemblies.Add(assembly);

			assembly.GetTypes()
				.Where(t => !t.IsAbstract)
				.Where(t => !t.IsGenericType)
				.Where(t => t.IsClass)
				.SelectMany(t => t.GetInterfaces()
					.Select(i => new { Type = t, Interface = i })
				)
				.Where(x => x.Interface.IsGenericType)
				.Where(x => x.Interface.GetGenericTypeDefinition() == typeof(IValidator<>))
				.Select(x => new { ValidatorType = x.Type, ModelType = x.Interface.GenericTypeArguments[0] })
				.ToList()
				.ForEach(x => RegisterValidator(services, x.ModelType, x.ValidatorType));
		}

		private static void RegisterValidator(IServiceCollection services, Type modelType, Type validatorType)
		{
			HashSet<Type> validatorTypes;
			if (!ValidatorTypesByModelType.TryGetValue(modelType, out validatorTypes))
			{
				validatorTypes = new HashSet<Type>();
				ValidatorTypesByModelType[modelType] = validatorTypes;
			}
			if (!validatorTypes.Contains(validatorType))
			{
				validatorTypes.Add(validatorType);
				services.AddScoped(validatorType);
			}
		}
	}
}
