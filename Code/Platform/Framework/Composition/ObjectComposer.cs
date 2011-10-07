using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Inbox2.Platform.Framework.Composition
{
	public static class ObjectComposer
	{
		static CompositionContainer container;
		private static ArrayList objects = new ArrayList();

		public static void InitalizeCatalogWith(AggregateCatalog catalog)
		{
			container = new CompositionContainer(catalog);			
		}

		public static void AddPart(object part)
		{
			objects.Add(part);
		}

		public static T GetObject<T>()
		{
			return container.GetExportedValueOrDefault<T>();
		}

		public static IEnumerable<T> GetObjects<T>()
		{
			return container.GetExportedValues<T>();
		}

		public static void Compose()
		{
			container.ComposeParts(objects);

			objects.Clear();
		}
	}
}