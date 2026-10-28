/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 5/3/2009
 * Time: 11:58 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;

namespace NovaNet
{
namespace Utils
{
	public delegate void NotifyProgress(int prmNt);
	public delegate int ImageManupulation();
	public delegate void FormType(int prmFormType);
	public delegate void NotifyImageDetails(string prmImageName,long prmImageSize);	
	public delegate void NotifyPageCountMismatch(int prmPageCountToBeScanned,int prmPageScanned);	
	/// <summary>
	/// Description of UtilsDeletgates.
	/// </summary>
	
    public interface StateData
    {
        MemoryStream StateLog();
    }

		public class TimeWatcher {
			private System.Diagnostics.Stopwatch _watch;
			public TimeWatcher() { }
			public void start() {
				_watch = System.Diagnostics.Stopwatch.StartNew();
			}
			public long stop(string caption = "Time taken: ") {
				_watch.Stop();
				print(caption);
				return _watch.ElapsedMilliseconds;
			}
			public long current(string caption = "Current Time: ") {
				if (_watch.IsRunning) {
					print(caption);
					return _watch.ElapsedMilliseconds;
				}
				return 0;
			}
			public void print(string caption = "Time taken: ") {
				System.Diagnostics.Debug.Print(caption + _watch.ElapsedMilliseconds.ToString());
			}
		};
}
}