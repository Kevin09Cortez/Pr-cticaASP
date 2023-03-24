using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        private object _equiposContexto;
        private List<equipos> listadoEquipo;

        public equiposController(equiposContext equiposContexto) 
        {
            _equiposContext = equiposContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los equipos existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get() 
        {
            var listadoEquipo = (from e in _equiposContext.equipos
                                 join m in _equiposContext.marcas on e.marca_id equals m.id_marcas
                                 join te in _equiposContext.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                                 where e.estado == "A"
                                 select new
                                 {
                                     e.id_equipos,
                                     e.nombre,
                                     e.descripcion,
                                     e.tipo_equipo_id,
                                     tipo_descripcion = te.descripcion,
                                     e.marca_id,
                                     m.nombre_marca
                                 }).ToList();

            if (listadoEquipo.Count == 0) 
            {
                return NotFound();
            }

            return Ok(listadoEquipo);
        }

        /// <summary>
        /// EndPoint que retorna los registros de una tabla filtrados por su ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id) 
        {
            equipos? equipo = (from e in _equiposContext.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();

            if (equipo == null) 
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        ///<summary>
        ///EndPoint que retorna los registros de una tabla filtrados por descripcion
        ///</summary>
        ///<param name="id"></param>
        ///<returns></returns>
        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro) 
        {
            equipos? equipo = (from e in _equiposContext.equipos
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();

            if (equipo == null) 
            {
                return NotFound();
            }

            return Ok(equipo);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] equipos equipo) 
        {
            try 
            {
                _equiposContext.equipos.Add(equipo);
                _equiposContext.SaveChanges();
                return Ok(equipo);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            equipos? equipoActual = (from e in _equiposContext.equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (equipoActual == null )
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.marca_id = equipoModificar.marca_id;
            equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id;
            equipoActual.anio_compra = equipoModificar.anio_compra;
            equipoActual.costo = equipoModificar.costo;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos
            _equiposContext.Entry(equipoActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(equipoModificar);
        }

        [HttpPut]
        [Route("actualizarEstado/{id}")]

        public IActionResult ActualizarEstadoEquipo(int id, string estado)
        {
            equipos? equipo = (from e in _equiposContext.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            equipo.estado = estado;
            _equiposContext.Entry(equipo).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(equipo);
        }
    }
}
