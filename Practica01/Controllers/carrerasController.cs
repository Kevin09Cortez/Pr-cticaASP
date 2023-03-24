using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carrerasController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        private object _equiposContexto;

        public carrerasController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos las carreras existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<carreras> listadoCarreras = (from e in _equiposContext.carreras
                                          select e).ToList();

            if (listadoCarreras.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoCarreras);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarCarreras([FromBody] carreras carrera)
        {
            try
            {
                _equiposContext.carreras.Add(carrera);
                _equiposContext.SaveChanges();
                return Ok(carrera);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarCarreras(int id, [FromBody] carreras carreraModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            carreras? carrerasActual = (from e in _equiposContext.carreras
                                   where e.carrera_id == id
                                   select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (carrerasActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            carrerasActual.nombre_carrera = carreraModificar.nombre_carrera;
            carrerasActual.facultad_id = carreraModificar.facultad_id;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos
            _equiposContext.Entry(carrerasActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(carreraModificar);
        }

        [HttpPut]
        [Route("actualizarEstado/{id}")]

        public IActionResult ActualizarEstadoCarrera(int id, string estado)
        {
            carreras? carrera = (from c in _equiposContext.carreras
                                 where c.carrera_id == id
                                 select c).FirstOrDefault();
            if (carrera == null)
            {
                return NotFound();
            }
            carrera.estado = estado;
            _equiposContext.Entry(carrera).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(carrera);
        }
    }
}
