﻿//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/t4models).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using LinqToDB;
using LinqToDB.DataProvider.SqlServer;
using LinqToDB.Mapping;

namespace cms.dbase.models
{
	/// <summary>
	/// Database       : Medicin
	/// Data Source    : 192.168.92.6\MSSQLSERVER2
	/// Server Version : 09.00.5057
	/// </summary>
	public partial class registry : LinqToDB.Data.DataConnection
	{
		public ITable<V_Doctors>   V_Doctorss   { get { return this.GetTable<V_Doctors>(); } }
		public ITable<V_Hospitals> V_Hospitalss { get { return this.GetTable<V_Hospitals>(); } }

		public registry()
			: base("registry")
		{
			InitDataContext();
		}

		public registry(string configuration)
			: base(configuration)
		{
			InitDataContext();
		}

		partial void InitDataContext();

		#region FreeTextTable

		public class FreeTextKey<T>
		{
			public T   Key;
			public int Rank;
		}

		[FreeTextTableExpression]
		public ITable<FreeTextKey<TKey>> FreeTextTable<TTable,TKey>(string field, string text)
		{
			return this.GetTable<FreeTextKey<TKey>>(
				this,
				((MethodInfo)(MethodBase.GetCurrentMethod())).MakeGenericMethod(typeof(TTable), typeof(TKey)),
				field,
				text);
		}

		[FreeTextTableExpression]
		public ITable<FreeTextKey<TKey>> FreeTextTable<TTable,TKey>(Expression<Func<TTable,string>> fieldSelector, string text)
		{
			return this.GetTable<FreeTextKey<TKey>>(
				this,
				((MethodInfo)(MethodBase.GetCurrentMethod())).MakeGenericMethod(typeof(TTable), typeof(TKey)),
				fieldSelector,
				text);
		}

		#endregion
	}

	// View
	[Table(Schema="portal", Name="V_Doctors")]
	public partial class V_Doctors
	{
		[Column, Nullable] public string C_FIO           { get; set; } // varchar(250)
		[Column, Nullable] public string C_SNILS         { get; set; } // varchar(11)
		[Column, Nullable] public string C_District      { get; set; } // varchar(250)
		[Column, Nullable] public string C_DistrictType  { get; set; } // varchar(100)
		[Column, Nullable] public string C_DoctorSpec    { get; set; } // varchar(250)
		[Column, Nullable] public string C_Registry_URL  { get; set; } // varchar(71)
		[Column, Nullable] public string C_Hospital_OID  { get; set; } // varchar(128)
		[Column, Nullable] public string C_Hospital_Name { get; set; } // varchar(200)
	}

	// View
	[Table(Schema="portal", Name="V_Hospitals")]
	public partial class V_Hospitals
	{
		[Identity          ] public int    Id            { get; set; } // int
		[Column,   Nullable] public string C_FullName    { get; set; } // varchar(1024)
		[Column,   Nullable] public string C_ShortName   { get; set; } // varchar(200)
		[Column,   Nullable] public string C_RegistryURL { get; set; } // varchar(71)
		[Column,   Nullable] public string C_OID         { get; set; } // varchar(128)
	}
}
 