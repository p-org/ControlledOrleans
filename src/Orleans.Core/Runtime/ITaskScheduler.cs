using Nekara.Client; using Nekara.Models; 


namespace Orleans.Runtime.Scheduler
{
    internal interface ITaskScheduler
    {
        void RunTask(Task task);
    }
}
