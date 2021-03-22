using Nicehash.Withdrawal.Config;
using Nicehash.Withdrawal.Mvc.Controller.Scheduler;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System.Threading.Tasks;

namespace Nicehash.Withdrawal.Mvc.Model.Scheduler
{
    public class SchedulerProxy : BaseProxy
    {
        /// <summary>
        /// Планировщик
        /// </summary>
        public IScheduler Scheduler { get; private set; }

        public SchedulerProxy()
        {
            LogProvider.IsDisabled = true;
        }

        public override async Task<bool> OnInitialization()
        {
            // Создаём планировщик
            Scheduler = await new StdSchedulerFactory().GetScheduler();

            // Создаём работу для ежедневной проверки выплат
            await CreateScheduleBalanceJob();

            return await base.OnInitialization();
        }

        public override async Task<bool> StartupAsync()
        {
            // Запускаем планировщик
            await Scheduler.Start();

            return await base.StartupAsync();
        }

        /// <summary>
        /// Создание работы для проверки баланса
        /// </summary>
        private async Task CreateScheduleBalanceJob()
        {
            // Добавляем работу
            var job = JobBuilder.Create<NiceHashScheduleJob>().Build();
            var trigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x => x.WithIntervalInMinutes(Configuration.NiceHash.IntervalInMinutes).RepeatForever()).Build();
            await Scheduler.ScheduleJob(job, trigger);
        }
    }
}