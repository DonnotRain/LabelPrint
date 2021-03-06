/*
  2011 - This file is part of AcaLabelPrint 

  AcaLabelPrint is free Software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  AcaLabelprint is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with AcaLabelPrint.  If not, see <http:www.gnu.org/licenses/>.

  We encourage you to use and extend the functionality of AcaLabelPrint,
  and send us an e-mail on the outlines of the extension you build. If
  it's generic, maybe we could add it to the project.
  Send your mail to the projectadmin at http:sourceforge.net/projects/labelprint/
*/
using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using System.Threading;
using ACA.Support.Tools.ACAException;

namespace ACA.LabelX.TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            UnhandledExceptionManager.AddHandler();

            System.Threading.Thread.CurrentThread.Name = "MT";
            GlobalDataStore.RunningWithConsole = true;
            GlobalDataStore.Logger.Info("ACALabelXTestServer has started...");
            GlobalDataStore.Logger.AutoFlush = false;

            Thread thrdBackup = new Thread(new ThreadStart(ACA.LabelX.Managers.LabelXServerBackupManager.DoThreadWork));
            thrdBackup.Name = "BK";
            thrdBackup.Start();

            Thread thrd1 = new Thread(new ThreadStart(ACA.LabelX.Managers.LabelXServerManager.DoThreadWork));
            thrd1.Name = "SV";
            thrd1.Start();

            Console.WriteLine("Press <enter> to stop this application anytime...");
            Console.ReadLine();

            ACA.LabelX.Managers.LabelXServerManager.Stop();
            if (thrdBackup.IsAlive)
            {
                if (!thrdBackup.Join(5000))
                {
                    thrdBackup.Abort();
                    Console.WriteLine("BK aborted");
                }
                else
                {
                    Console.WriteLine("BK Stopped");
                }
            }
            if (thrd1.IsAlive)
            {
                thrd1.Join(60000);
                Console.WriteLine("SV stopped");
            }
        }
    }
}
/*
 AcaLabelPrint Copyright (C) 2011  Retailium Software Development BV.
 This program comes with ABSOLUTELY NO WARRANTY; 
 This is free Software, and you are welcome to redistribute it
 under certain conditions. See the License.txt file or 
 GNU GPL 3.0 License at <http://www.gnu.org/licenses>*/
