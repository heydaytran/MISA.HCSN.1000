using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces;
using MISA.QLTS.Core.Result;
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
        public override void Validate(ResponseResult responseResult, Asset entity, Guid? entityID, string functionName)
        {
            var propertyUnique = "Mã tài sản";
            var propertyRequired = new Dictionary<string, string>();
            propertyRequired.Add("Mã tài sản", entity.AssetCode);
            propertyRequired.Add("Tên tài sản", entity.AssetName);

            foreach (var property in propertyRequired)
            {
                if (string.IsNullOrEmpty(property.Value))
                {
                    responseResult.IsSuccess = false;
                    responseResult.ErrorCode = Enum.ErrorCode.BADREQUEST;
                    responseResult.DevMsg = property.Key + " " + Resource.ResourceMessage.Error_Required;
                    responseResult.UserMsg = property.Key + " " + Resource.ResourceMessage.Error_Required;
                }
            }

            // kiểm tra xem trường nào là duy nhất (có thuộc tính Unique) thì check duplicate

            bool checkDuplicateCode = _unitOfWork.Asset.CheckStoreCode(entityID, entity.AssetCode, functionName);

            if (checkDuplicateCode)
            {
                responseResult.IsSuccess = false;
                responseResult.ErrorCode = Enum.ErrorCode.BADREQUEST;
                responseResult.DevMsg = propertyUnique + " " + Resource.ResourceMessage.Error_Duplicate;
                responseResult.UserMsg = propertyUnique + " " + Resource.ResourceMessage.Error_Duplicate;
            }
        }

    }
}
