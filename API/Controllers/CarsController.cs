using API.Services;
using API.Singleton;
using API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IModelsService modelsService;
        public CarsController(IModelsService modelsService)
        {
            this.modelsService = modelsService;
        }

        [HttpGet("models")]
        public ActionResult<IEnumerable<string>> Get(int? modelyear,string make)
        {
            JosnResult result = new JosnResult();
            if (!modelyear.HasValue || string.IsNullOrEmpty(make))
            {
                result.Message = "Invalid model year or make name";
                return BadRequest(result);
            }
            
            var csvList = CSVSingleton.GetCarMakeList;
            if (csvList.Count() > 0)
            {
                var car = csvList.FirstOrDefault(x => x.make_name.ToLower().Equals(make.ToLower()));
                if(car != null)
                {
                    var response = modelsService.GetModelsForMakeIdYear(car.make_id, modelyear.Value);
                    if(response != null)
                    {
                        if(response.Message == "Results returned successfully")
                        {
                            result.Message = response.Message;
                            result.Data = response.Results;
                            return Ok(result);
                        }
                        else
                        {
                            result.Message = response.Message;
                            return BadRequest(result);
                        }
                    }
                    else
                    {
                        result.Message = string.Format("please try again");
                        return BadRequest(result);
                    }
                }
                else
                {
                    result.Message = string.Format("can't found car with name {0}", make);
                    return BadRequest(result);
                }
                
            }
            else
            {
                result.Message = "can't read csv file please check file path wwwroot/file/.csv";
                return BadRequest(result);
            }
           
      
        }
    }
}
