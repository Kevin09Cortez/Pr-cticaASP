using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipo_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        private object _equiposContexto;

        public tipo_equipoController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los tipos de equipos existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<tipo_equipo> listadoTipos = (from e in _equiposContext.tipo_equipo
                                          select e).ToList();

            if (listadoTipos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoTipos);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarTipos([FromBody] tipo_equipo tipo_equipos)
        {
            try
            {
                _equiposContext.tipo_equipo.Add(tipo_equipos);
                _equiposContext.SaveChanges();
                return Ok(tipo_equipos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarTipos(int id, [FromBody] tipo_equipo tiposModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            tipo_equipo? tiposActual = (from e in _equiposContext.tipo_equipo
                                   where e.id_tipo_equipo == id
                                   select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (tiposActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            tiposActual.descripcion = tiposModificar.descripcion;
            tiposActual.estado = tiposModificar.estado;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos
            _equiposContext.Entry(tiposActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(tiposModificar);
        }

        [HttpPut]
        [Route("actualizarEstado/{id}")]

        public IActionResult ActualizarTipoEquipo(int id, string estado)
        {
            tipo_equipo? tipo_equipo = (from te in _equiposContext.tipo_equipo
                                    where te.id_tipo_equipo == id
                                    select te).FirstOrDefault();
            if (tipo_equipo == null)
            {
                return NotFound();
            }
            tipo_equipo.estado = estado;
            _equiposContext.Entry(tipo_equipo).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(tipo_equipo);
        }
    }
}
