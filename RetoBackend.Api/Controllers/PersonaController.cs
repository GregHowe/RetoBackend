using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RetoBackend.Api.Aplicacion;
using RetoBackend.Api.CloudStorage;

namespace RetoBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<PersonaController> _logger;
        private readonly ICloudStorage _cloudStorage;

        public PersonaController(ILogger<PersonaController> logger,
            IMediator mediator,
            ICloudStorage cloudStorage)
        {
            _mediator = mediator;
            _logger = logger;
            _cloudStorage = cloudStorage;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            try
            {
                await UploadFile(data);
                return await _mediator.Send(data);
            }
            catch (Exception e)
            {
                _logger?.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        private async Task UploadFile(Nuevo.Ejecuta infoImg)
        {
            string fileNameForStorage = FormFileName(infoImg.title, infoImg.fileName);
            infoImg.imageUrl = await _cloudStorage.UploadFileAsync(infoImg.imageByte, fileNameForStorage);
            infoImg.imageStorageName = fileNameForStorage;
        }

        private static string FormFileName(string title, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{title}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            return fileNameForStorage;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonaDto>> GetPersonaUnico(int id)
        {
            _logger.LogInformation("Start : Getting item details for {ID}", id);
            return await _mediator.Send(new ConsultaFiltro.PersonaUnico { PersonaId = id });
        }


        [HttpGet]
        public async Task<ActionResult<List<PersonaDto>>> GetPersonas()
        {
            _logger.LogInformation("Start : Getting Personas");
            return await _mediator.Send(new Consulta.Ejecuta());
        }

        [HttpPut]
        public async Task<ActionResult<Unit>> Update(Update.Ejecuta data)
        {
            try
            {
                return await _mediator.Send(data);
            }
            catch (Exception e)
            {
                _logger?.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            try
            {
                return await _mediator.Send(new Delete.DeletePersona { PersonaId = id });
            }
            catch (Exception e)
            {
                _logger?.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

    }
}
