using Hangfire;

namespace LibraSoft.Api.Events
{
    public class HangfireActivator(IServiceProvider serviceProvider) : JobActivator
    {
        public override object ActivateJob(Type jobType)
        {
            return serviceProvider.GetRequiredService(jobType);
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            return new HangfireActivatorScope(serviceProvider.CreateScope());
        }

        private class HangfireActivatorScope(IServiceScope serviceScope) : JobActivatorScope
        {
            public override object Resolve(Type type)
            {
                return serviceScope.ServiceProvider.GetRequiredService(type);
            }

            public override void DisposeScope()
            {
                serviceScope.Dispose();
            }
        }
    }
}