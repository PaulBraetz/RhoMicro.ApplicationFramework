namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System.Collections.Generic;
using System.Threading.Tasks;

internal class SynchronousTaskScheduler : TaskScheduler
{
    public override Int32 MaximumConcurrencyLevel => 1;
    protected override void QueueTask(Task task) => TryExecuteTask(task);
    protected override Boolean TryExecuteTaskInline(Task task, Boolean taskWasPreviouslyQueued) => TryExecuteTask(task);
    protected override IEnumerable<Task> GetScheduledTasks() => [];
}
