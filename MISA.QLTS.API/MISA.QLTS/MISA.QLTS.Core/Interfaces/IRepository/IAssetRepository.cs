using MISA.QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Interfaces
{
    /// <summary>
    /// interface tài sản
    /// </summary>
    /// author: TVThinh 12.05.2021
    public interface IAssetRepository : IBaseRepository<Asset>
    {
        public bool CheckStoreCode(Guid? assetId, string assetCode, string functionName);
    }
}
