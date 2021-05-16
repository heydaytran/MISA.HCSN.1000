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

        [HttpGet("Filter")]
        public IActionResult Get(string input)
        {
            int a = 0;
            Console.WriteLine(a);
            var responseResult = _assetService.GetEntitiesFilter(input);
            return Ok(responseResult);
        }
    }
}
