using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FortuneTeller4.Services
{
    public interface IEurekaClientService
    {
        Task<string> GetServices();

        Task<string> GetServicesWithHystrix();
    }
}
