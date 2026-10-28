/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 20/2/2009
 * Time: 9:36 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using ClearImage;

namespace NovaNet
{
	namespace Utils
	{
		/// <summary>
		/// Description of FileorFolder.
		/// </summary>
		public class FileorFolder
		{


			public FileorFolder()
			{
			}
			public static bool CreateFolder(string prmPath)
			{
				string dirPath = @prmPath;
				bool crtFolderBol = true;
				string err = null;

				try
				{
					if (Directory.Exists(dirPath) == false)
					{
						Directory.CreateDirectory(dirPath);
					}
					crtFolderBol = true;
				}
				catch (IOException ioe)
				{
					crtFolderBol = false;
					err = ioe.Message;
				}
				return crtFolderBol;
			}

			public static bool RenameFolder(string pOldFolder, string pNewPath)
			{
				bool crtFolderBol = true;
				string err = string.Empty;

				try
				{
					if (Directory.Exists(pOldFolder) == true)
					{
						Directory.Move(pOldFolder, pNewPath);
					}
					crtFolderBol = true;
				}
				catch (IOException ioe)
				{
					crtFolderBol = false;
					err = ioe.Message;
				}
				return crtFolderBol;
			}

			public void MoveFile(string sourceFolder, string destFolder, string fileName, bool prmOverWright)
			{
				int pos = fileName.IndexOf("-");
				string originalImage;
				string sourceFilePath;
				string destFilePath;


				if (pos > 0)
				{
					originalImage = fileName.Substring(0, pos) + "*" + ".TIF";
					string[] searchFileName = Directory.GetFiles(sourceFolder, originalImage);
					originalImage = searchFileName[0];
					sourceFilePath = originalImage;
					destFilePath = destFolder + "\\" + fileName;
				}
				else
				{
					sourceFilePath = sourceFolder + "\\" + fileName;
					destFilePath = destFolder + "\\" + fileName;
				}
				if ((File.Exists(destFilePath) == false) && (prmOverWright == false))
				{
					File.Copy(sourceFilePath, destFilePath, prmOverWright);
				}
				else if (prmOverWright == true)
					File.Copy(sourceFilePath, destFilePath, prmOverWright);
			}
			


		}
	}
}