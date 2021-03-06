﻿using System;
using System.Collections.Generic;
using System.Text;

namespace dnlib.DotNet
{
    /// <summary>
    /// Indicate the structural definitions of interest
    /// </summary>
    /// <remarks>
    /// <see cref="IDefinition"/> is implemented by <see cref="AssemblyDef"/>,
    /// <see cref="ModuleDef"/>, <see cref="TypeDef"/>, <see cref="MethodDef"/>,
    /// <see cref="FieldDef"/>, <see cref="PropertyDef"/> and <see cref="EventDef"/>.
    /// </remarks>
    public interface IDefinition : ICodedToken, IHasCustomAttribute
	{
		/// <summary>
		/// Gets the name
		/// </summary>
		UTF8String Name { get; set; }

		/// <summary>
		/// Gets the full name
		/// </summary>
        string FullName { get; }
    }
}
