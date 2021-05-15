﻿using Dapper;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Infrastructure.Repository
{
    public class AssetRepository:BaseRepository<Asset>, IAssetRepository
    {
        public bool CheckStoreCode(Guid? assetId, string assetCode, string functionName)
        {
            var sqlCheckExistCode = "";
            if (functionName == "Insert")
            {
                // Thực hiện lấy số lượng bản ghi có mã khách hàng trùng với mã khách hàng truyền vào
                sqlCheckExistCode = $"Select AssetCode from Asset where Asset.AssetCode = '{assetCode}'";
            }
            else if (functionName == "Update")
            {
                // Thực hiện lấy số lượng bản ghi có mã khách hàng trùng với mã khách hàng truyền vào, loại bỏ bản ghi hiện tại
                sqlCheckExistCode = $"Select AssetCode from Asset where Asset.AssetCode = '{assetCode}' and  Asset.AssetId != '{assetId}'";
            }

            

            // thực hiện truy vấn 
            var storeExistCode = _dbConnection
                .Query<string>(
                    sqlCheckExistCode,
                    commandType: CommandType.Text);

            // kiểm tra kết quả:
            if (storeExistCode.AsList().Count > 0)
            {
                return true;
            }

            return false;
        }

        

    }
}
