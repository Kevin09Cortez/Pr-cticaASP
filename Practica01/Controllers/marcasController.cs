using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcasController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        private object _equiposContexto;

        public marcasController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos las marcas existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<marcas> listadoMarcas = (from e in _equiposContext.marcas
                                                  select e).ToList();

            if (listadoMarcas.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoMarcas);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarMarcas([FromBody] marcas marca)
        {
            try
            {
                _equiposContext.marcas.Add(marca);
                _equiposContext.SaveChanges();
                return Ok(marca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarMarca(int id, [FromBody] marcas marcaModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            marcas? marcaActual = (from e in _equiposContext.marcas
                                           where e.id_marcas == id
                                           select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (marcaActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            marcaActual.nombre_marca = marcaModificar.nombre_marca;
            marcaActual.estados = marcaModificar.estados;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos
            _equiposContext.Entry(marcaActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(marcaModificar);
        }

        [HttpPut]
        [Route("actualizarEstado/{id}")]

        public IActionResult ActualizarEstadoMarca(int id, string estado)
        {
            marcas? marca = (from m in _equiposContext.marcas
                             where m.id_marcas == id
                             select m).FirstOrDefault();
            if (marca == null)
            {
                return NotFound();
            }
            marca.estados = estado;
            _equiposContext.Entry(marca).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(marca);
        }

    }
}
