using System;
using Nekara.Client; using Nekara.Models; 


namespace Orleans.Runtime.Scheduler
{
    internal class TaskSchedulerUtils
    {
        private static readonly Action<object> TaskFunc = state => RunWorkItemTask((IWorkItem)state);

        internal static Task WrapWorkItemAsTask(IWorkItem todo)
        {
            return new Task(TaskFunc, todo);
        }

        private static void RunWorkItemTask(IWorkItem todo)
        {
            try
            {
                RuntimeContext.SetExecutionContext(todo.SchedulingContext);
                todo.Execute();
            }
            finally
            {
                RuntimeContext.ResetExecutionContext();
            }
        }
    }
}
