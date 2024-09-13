using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Service._12312.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ConsulController : ControllerBase
{

    [HttpGet]
    public string GetState()
    {
        Contants.GetAndSet();
        return "success";
    }

}
