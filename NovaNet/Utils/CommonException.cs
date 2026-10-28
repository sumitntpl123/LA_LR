/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 19/2/2009
 * Time: 1:58 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data.Odbc;
namespace NovaNet
{
namespace Utils
{
	
	/// <summary>
	/// Description of ihException.
	/// </summary>
	public class KeyCheckException: Exception
	{
		public KeyCheckException(String msg): base(msg)
		{}
	}
	
	/// <summary>
	/// Custom error class for handling database error while saving 
	/// </summary>
	public class DbCommitException: Exception
	{
		
		public DbCommitException(string message)
			: base(message){}        
	}
	
	/// <summary>
	/// Custom error class for any error while creating folder 
	/// </summary>
	public class CreateFolderException: Exception
	{
	
		public CreateFolderException(string message)
			: base(message){}        
	}
	
	
	
	/// <summary>
	/// Custom error class for any error occured while connecting with database
	/// </summary>
	public class DBConnectionException: Exception
	{
		
	}
	
	public class INIFileException: Exception
	{
	
	}
	
}
}