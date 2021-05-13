using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Service
{
    public class AssetService:BaseService<Asset>, IAssetService
    {
        IUnitOfWork _unitOfWork;
        public AssetService(IUnitOfWork unitOfWork, IBaseRepository<Asset> baseRepository):base(baseRepository)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
