using BusinessLayer;
using Helper.CommonModel;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implementation
{
    public class SchedulerImpl : IScheduler
    {
        public readonly SchedulerBLL _schedulerBLL;
        public SchedulerImpl(SchedulerBLL schedulerBLL)
        {
            _schedulerBLL = schedulerBLL;
        }
        public async Task<CommonResponse> EarnedMonthlyLeave()
        {
            return await _schedulerBLL.EarnedMonthlyLeave();
        }
    }
}
