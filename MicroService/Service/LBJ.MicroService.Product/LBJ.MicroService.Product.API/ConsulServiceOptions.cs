using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBJ.MicroService.Product.API
{
    public class ConsulServiceOptions
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 服务IP
        /// </summary>
        public string ServiceIP { get; set; }
        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServicePort { get; set; }
        /// <summary>
        /// 服务健康检查地址
        /// </summary>
        public string ServiceHealthCheck { get; set; }
        /// <summary>
        /// Consul 地址
        /// </summary>
        public string ConsulAddress { get; set; }
    }
}
