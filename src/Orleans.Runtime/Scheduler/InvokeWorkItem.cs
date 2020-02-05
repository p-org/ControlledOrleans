using System;
using Nekara.Client;
using Nekara.Models; 
using Microsoft.Extensions.Logging;
// using Nekara.Core;

namespace Orleans.Runtime.Scheduler
{
    internal class InvokeWorkItem : WorkItemBase
    {
        private readonly ILogger logger;
        private readonly ActivationData activation;
        private readonly Message message;
        private readonly Dispatcher dispatcher;

        /* public static ITestingService nekara = RuntimeEnvironment.Client.Api;
        public static Nekara.Helpers.UniqueIdGenerator TaskIdGenerator = RuntimeEnvironment.Client.TaskIdGenerator; */

        public InvokeWorkItem(ActivationData activation, Message message, Dispatcher dispatcher, ILogger logger)
        {
            // nekara.CreateTask();
            // Console.WriteLine("Testing - Arun");

            this.logger = logger;
            if (activation?.GrainInstance == null)
            {
                var str = string.Format("Creating InvokeWorkItem with bad activation: {0}. Message: {1}", activation, message);
                logger.Warn(ErrorCode.SchedulerNullActivation, str);
                throw new ArgumentException(str);
            }

            this.activation = activation;
            this.message = message;
            this.dispatcher = dispatcher;
            this.SchedulingContext = activation.SchedulingContext;
            activation.IncrementInFlightCount();
        }

        public override WorkItemType ItemType
        {
            get { return WorkItemType.Invoke; }
        }

        public override string Name
        {
            get { return String.Format("InvokeWorkItem:Id={0} {1}", message.Id, message.DebugContext); }
        }

        public override void Execute()
        {
            // var taskId = TaskIdGenerator.Generate();
            try
            {
                // nekara.StartTask(taskId);

                var grain = activation.GrainInstance;
                var runtimeClient = this.dispatcher.RuntimeClient;
                Task task = runtimeClient.Invoke(grain, this.activation, this.message);

                // Note: This runs for all outcomes of resultPromiseTask - both Success or Fault
                if (task.IsCompleted)
                {
                    OnComplete();
                }
                else
                {
                    task.ContinueWith(t => OnComplete()).Ignore();
                }
                // nekara.EndTask(taskId);
            }
            catch (Exception exc)
            {
                logger.Warn(ErrorCode.InvokeWorkItem_UnhandledExceptionInInvoke, 
                    String.Format("Exception trying to invoke request {0} on activation {1}.", message, activation), exc);
                OnComplete();
            }
        }

        private void OnComplete()
        {
            activation.DecrementInFlightCount();
            this.dispatcher.OnActivationCompletedRequest(activation, message);
        }

        public override string ToString()
        {
            return String.Format("{0} for activation={1} Message={2}", base.ToString(), activation, message);
        }
    }
}
