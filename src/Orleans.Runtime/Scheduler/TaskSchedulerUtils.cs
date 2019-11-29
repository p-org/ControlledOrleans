using System;
using System.Threading.Tasks;
using Nekara.Core;
using Nekara.Client;

namespace Orleans.Runtime.Scheduler
{
    internal class TaskSchedulerUtils
    {
        public static ITestingService nekara = RuntimeEnvironment.Client.Api;
        public static Nekara.Helpers.UniqueIdGenerator TaskIdGenerator = RuntimeEnvironment.Client.TaskIdGenerator;

        private static readonly Action<object> TaskFunc = state => RunWorkItemTask((IWorkItem)state);

        private static readonly Action<object> ControlledTaskFunc = state => RunControlledWorkItemTask((IWorkItem)state);

        internal static Task WrapWorkItemAsTask(IWorkItem todo)
        {
            // return new Task(TaskFunc, todo);
            if (todo.ItemType == WorkItemType.Invoke)
            {
                return new Task(ControlledTaskFunc, todo);
            }
            else
            {
                return new Task(TaskFunc, todo);
            }
        }

        private static void RunWorkItemTask(IWorkItem todo)
        {
            try
            {
                //Console.WriteLine("  ... [{0}] Invoke Start", todo.ItemType);
                RuntimeContext.SetExecutionContext(todo.SchedulingContext);
                todo.Execute();
            }
            finally
            {
                RuntimeContext.ResetExecutionContext();
                //Console.WriteLine("  ... [{0}] Invoke End", todo.ItemType);
            }
        }

        private static void RunControlledWorkItemTask(IWorkItem todo)
        {
            var taskId = TaskIdGenerator.Generate();
            try
            {
                //Console.WriteLine("  ... [{0}] Invoke Start", todo.ItemType);
                nekara.StartTask(taskId);
                RuntimeContext.SetExecutionContext(todo.SchedulingContext);
                todo.Execute();
            }
            finally
            {
                RuntimeContext.ResetExecutionContext();
                nekara.EndTask(taskId);
                //Console.WriteLine("  ... [{0}] Invoke End", todo.ItemType);
            }
        }
    }
}
