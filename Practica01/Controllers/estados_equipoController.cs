using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        private object _equiposContexto;

        public estados_equipoController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los estados de los equipos existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<estados_equipo> listadoEstados = (from e in _equiposContext.estados_equipo
                                              select e).ToList();

            if (listadoEstados.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEstados);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEstados([FromBody] estados_equipo estadoEquipos)
        {
            try
            {
                _equiposContext.estados_equipo.Add(estadoEquipos);
                _equiposContext.SaveChanges();
                return Ok(estadoEquipos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEstados(int id, [FromBody] estados_equipo estadosModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            estados_equipo? estadosActual = (from e in _equiposContext.estados_equipo
                                        where e.id_estados_equipo == id
                                        select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (estadosActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            estadosActual.descripcion = estadosModificar.descripcion;
            estadosActual.estado = estadosModificar.estado;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos
            _equiposContext.Entry(estadosActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(estadosModificar);
        }

        [HttpPut]
        [Route("actualizarEstado/{id}")]

        public IActionResult ActualizarEstadoEquipo(int id, string estado)
        {
            estados_equipo? estado_equipo = (from ee in _equiposContext.estados_equipo
                                       where ee.id_estados_equipo == id
                                       select ee).FirstOrDefault();
            if (estado_equipo == null)
            {
                return NotFound();
            }
            estado_equipo.estado = estado;
            _equiposContext.Entry(estado_equipo).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(estado_equipo);
        }
    }
}
