using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CustomValidation.Components
{
	/// <summary>
	///	This class is Fluent Validation specific. It scans assemblies for classes that implement
	///	IValidator&lt;T&gt; and stores a list of validator types grouped by the
	///	type of object they validate (the &lt;T&gt;)
	/// <para>
	///		This is not related to Blazor validation.
	///		See <see cref="CustomValidation.Startup.ConfigureServices(IServiceCollection)"/> for an example of
	///		how to use this class.
	/// </para>
	/// </summary>
	/// <example>
	///	public void ConfigureServices(IServiceCollection services)
	///	{
	///		services.AddValidatorsInAssembly(typeof(Startup).Assembly);
	///	}
	/// </example>
	public static class ValidatorRepository
	{
		// Keeps a list of assemblies we've already scanned, so we don't scan twice
		readonly static HashSet<Assembly> Assemblies;
		// Keeps a list of validator types we can look up by type of object to validate
		readonly static Dictionary<Type, HashSet<Type>> ValidatorTypesByModelType;

		static ValidatorRepository()
		{
			Assemblies = new HashSet<Assembly>();
			ValidatorTypesByModelType = new Dictionary<Type, HashSet<Type>>();
		}

		/// <summary>
		/// Scans through the assembly for validators
		/// </summary>
		public static void AddValidatorsInAssembly(this IServiceCollection services, Assembly assembly)
		{
			// If scanned before, do not scan it again
			if (Assemblies.Contains(assembly))
				return;

			// Otherwise, reflect over the types in the assembly
			ReflectOverAssemblyForValidators(services, assembly);
		}

		/// <summary>
		/// Returns a list of validator types that can be used to validate
		/// the <paramref name="modelType"/> type
		/// </summary>
		/// <param name="modelType"></param>
		/// <returns></returns>
		public static IEnumerable<Type> GetValidators(Type modelType)
		{
			if (ValidatorTypesByModelType.TryGetValue(modelType, out HashSet<Type> validatorTypes))
				return validatorTypes;

			return Array.Empty<Type>();
		}

		/// <summary>
		/// Finds all types that implement <see cref="FluentValidation.IValidator{T}"/>.
		/// Stores the validator types in a lookup keyed on the type of object they validate,
		/// and ensures the validator type is registered with Dependency Injection
		/// </summary>
		/// <param name="services"></param>
		/// <param name="assembly"></param>
		private static void ReflectOverAssemblyForValidators(IServiceCollection services, Assembly assembly)
		{
			// Note that we have scanned this assembly, so it is never scanned twice
			Assemblies.Add(assembly);

			// Get types in the assembly
			assembly.GetTypes()
				.Where(t => t.IsClass)
				// Exclude any class that is abstract
				.Where(t => !t.IsAbstract)
				// Exclude any type that is generic
				.Where(t => !t.IsGenericType)
				// For each implemented interface select ClassType+InterfaceType
				.SelectMany(t => t.GetInterfaces()
					.Select(i => new { Type = t, Interface = i })
				)
				// Only where the interface is FluentValidation.IValidator<T>
				.Where(x => x.Interface.IsGenericType)
				.Where(x => x.Interface.GetGenericTypeDefinition() == typeof(IValidator<>))
				// Give us a list of ModelType+ValidatorType
				.Select(x => new { ModelType = x.Interface.GenericTypeArguments[0], ValidatorType = x.Type })
				.ToList()
				// Now add these to the look-up and register it with dependency injection
				.ForEach(x => RegisterValidator(services, x.ModelType, x.ValidatorType));
		}

		/// <summary>
		/// Stores the <paramref name="validatorType"/> in a list of applicable validators, keyed by
		/// the type of object they validate <paramref name="modelType"/>.
		/// Also registers the validator type with dependency injection so we can resolve instances
		/// of it at runtime.
		/// </summary>
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
