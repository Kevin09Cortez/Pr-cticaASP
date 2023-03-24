using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        private object _equiposContexto;

        public reservasController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos las reservas existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<reservas> listadoReservas = (from e in _equiposContext.reservas
                                              select e).ToList();

            if (listadoReservas.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoReservas);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarReservas([FromBody] reservas reserva)
        {
            try
            {
                _equiposContext.reservas.Add(reserva);
                _equiposContext.SaveChanges();
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarReservas(int id, [FromBody] reservas reservaModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            reservas? reservaActual = (from e in _equiposContext.reservas
                                        where e.reserva_id == id
                                        select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (reservaActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            reservaActual.equipo_id = reservaModificar.equipo_id;
            reservaActual.usuario_id = reservaModificar.usuario_id;
            reservaActual.fecha_salida = reservaModificar.fecha_salida;
            reservaActual.hora_salida = reservaModificar.hora_salida;
            reservaActual.tiempo_reserva = reservaModificar.tiempo_reserva;
            reservaActual.estado_reserva_id = reservaModificar.estado_reserva_id;
            reservaActual.fecha_retorno = reservaModificar.fecha_retorno;
            reservaActual.hora_retorno = reservaModificar.hora_retorno;


            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos
            _equiposContext.Entry(reservaActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(reservaModificar);
        }

        [HttpPut]
        [Route("actualizarEstado/{id}")]
        public IActionResult actualizarEstado(int id, string estado)
        {
            reservas? reserva = (from r in _equiposContext.reservas
                                 where r.reserva_id == id
                                 select r).FirstOrDefault();
            if (reserva == null)
            {
                return NotFound();
            }

            reserva.estado = estado;

            _equiposContext.Entry(reserva).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(reserva);
        }
    }
}
