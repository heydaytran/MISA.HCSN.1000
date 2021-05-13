using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.QLTS.Web.Controllers
{
    public class AssetController : BaseEntityController<Asset>
    {
        private IAssetService _assetService;
        public AssetController(IAssetService assetService) : base(assetService)
        {
            _assetService = assetService;
        }


        /// <summary>
        /// API lấy thông tin đối tượng theo mã 
        /// </summary>
        /// <param name="storeCode">Mã cửa hàng</param>
        /// <returns>ResponseResult với data là Danh sách bản ghi có mã cửa hàng bằng mã cửa hàng truyền vào</returns>
        /// CreatedBy: ntcu 20.04.20201
        //[HttpGet("getbycode")]
        //public IActionResult Get(string storeCode)
        //{
        //    var responseResult = new ResponseResult();
        //    try
        //    {
        //        responseResult = _storeService.GetStoreByStoreCode(storeCode);
        //    }
        //    catch (Exception e)
        //    {
        //        responseResult.OnException(responseResult, e);
        //    }

        //    return Ok(responseResult);
        //}

        ///// <summary>
        ///// API lấy thông tin cửa hàng theo các tham số lọc: storeCode, storeName, address, phoneNumber, status
        ///// </summary>
        ///// <param name="filterparams">Đối tượng có các thuộc tính các tham số lọc</param>
        ///// <returns>ResponseResult với data là Danh sách cửa hàng lọc được</returns>
        ///// CreatedBy: nctu 20.04.2021
        //[HttpGet("Filter")]
        //public IActionResult Get([FromQuery] FilterStoreParams filterparams)
        //{
        //    var responseResult = new ResponseResult();
        //    try
        //    {
        //        responseResult = _storeService.GetStoreFilter(
        //            filterparams.storeCode,
        //            filterparams.storeName,
        //            filterparams.address,
        //            filterparams.phoneNumber,
        //            filterparams.status,
        //            filterparams.recordNumber,
        //            filterparams.pageNumber
        //            );
        //    }
        //    catch (Exception e)
        //    {
        //        responseResult.OnException(responseResult, e);
        //    }
        //    return Ok(responseResult);
        //}

        /// <summary>
        /// API lấy danh sách cửa hàng theo chỉ số trang và số lượng bản ghi/trang
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi/trang</param>
        /// <param name="pageIndex">Chỉ số trang</param>
        ///// <returns>ResponseResult với data là Danh sách cửa hàng</returns>
        //[HttpGet("paging")]
        //public IActionResult Get(int pageSize, int pageIndex)
        //{
        //    var responseResult = new ResponseResult();
        //    try
        //    {
        //        responseResult = _storeService.GetStorePaging(pageSize, pageIndex);
        //    }
        //    catch (Exception e)
        //    {
        //        responseResult.OnException(responseResult, e);
        //    }

        //    return Ok(responseResult);
        //}

    }
}
