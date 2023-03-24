using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class facultadesController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        private object _equiposContexto;

        public facultadesController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos las facultades existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<facultades> listadoFacultades = (from e in _equiposContext.facultades
                                              select e).ToList();

            if (listadoFacultades.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoFacultades);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarFacultades([FromBody] facultades facultad)
        {
            try
            {
                _equiposContext.facultades.Add(facultad);
                _equiposContext.SaveChanges();
                return Ok(facultad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarFacultades(int id, [FromBody] facultades facultadModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            facultades? facultadActual = (from e in _equiposContext.facultades
                                        where e.facultad_id == id
                                        select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (facultadActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            facultadActual.nombre_facultad = facultadModificar.nombre_facultad;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos
            _equiposContext.Entry(facultadActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(facultadModificar);
        }

        [HttpPut]
        [Route("actualizarEstado/{id}")]

        public IActionResult ActualizarEstadoFacultad(int id, string estado)
        {
            facultades? facultad = (from f in _equiposContext.facultades
                                    where f.facultad_id == id
                                    select f).FirstOrDefault();
            if (facultad == null)
            {
                return NotFound();
            }
            facultad.estado = estado;
            _equiposContext.Entry(facultad).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(facultad);
        }
    }
}
