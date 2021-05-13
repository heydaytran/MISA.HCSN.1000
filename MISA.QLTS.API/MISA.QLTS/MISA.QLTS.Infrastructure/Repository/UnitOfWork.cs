using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.EShop.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IAssetRepository asset, 
            IAssetTypeRepository assetType, 
            IDepartmentRepository department
            )
        {
            Asset = asset;
            AssetType = assetType;
            Department = department;
        }

        public IAssetRepository Asset { get; }

        public IAssetTypeRepository AssetType { get; }

        public IDepartmentRepository Department { get; }

    }
}
