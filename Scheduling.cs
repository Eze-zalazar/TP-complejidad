using System;
using System.Collections.Generic;
using System.Threading;
namespace tpfinal
{
    public class Scheduling
{
        private List<Proceso> procesos = new List<Proceso>();
   private object locker = new object();
    public Scheduling(List<Proceso> procesos)
	{
            this.procesos = procesos;
	}
    public Proceso getProceso()
    {
        String ThreadName = Thread.CurrentThread.Name;
        Console.WriteLine("{0} using C-sharpcorner.com", ThreadName);
        Monitor.Enter(locker);
            try
            {

                if (this.procesos.Count > 0){ 
                 var ele = this.procesos[0]; 
                    this.procesos.RemoveAt(0);
                 return ele;
                }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Monitor.Exit(locker);
            Console.WriteLine("{0} releasing C-sharpcorner.com", ThreadName);
        }
            return null;
    }
}
}