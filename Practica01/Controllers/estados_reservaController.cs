using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_reservaController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        private object _equiposContexto;

        public estados_reservaController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los estados de las reservas existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<estados_reserva> listadoReservas = (from e in _equiposContext.estados_reserva
                                              select e).ToList();

            if (listadoReservas.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoReservas);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarReservas([FromBody] estados_reserva estadoReserva)
        {
            try
            {
                _equiposContext.estados_reserva.Add(estadoReserva);
                _equiposContext.SaveChanges();
                return Ok(estadoReserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarReservas(int id, [FromBody] estados_reserva reservasModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            estados_reserva? reservasActual = (from e in _equiposContext.estados_reserva
                                        where e.estado_res_id == id
                                        select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (reservasActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            reservasActual.estado = reservasModificar.estado;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos
            _equiposContext.Entry(reservasActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(reservasModificar);
        }

        [HttpPut]
        [Route("actualizarUso/{id}")]
        public IActionResult actualizarUso(int id, string estado)
        {
            estados_reserva? estado_reserva = (from er in _equiposContext.estados_reserva
                                         where er.estado_res_id == id
                                         select er).FirstOrDefault();
            if (estado_reserva == null)
            {
                return NotFound();
            }

            estado_reserva.estado = estado;

            _equiposContext.Entry(estado_reserva).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(estado_reserva);
        }
    }
}
