using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        private object _equiposContexto;

        public usuariosController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los usuarios existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<usuarios> listadoUsuarios = (from e in _equiposContext.usuarios
                                              select e).ToList();

            if (listadoUsuarios.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoUsuarios);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarUsuarios([FromBody] usuarios usuario)
        {
            try
            {
                _equiposContext.usuarios.Add(usuario);
                _equiposContext.SaveChanges();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarUsuarios(int id, [FromBody] usuarios usuarioModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            usuarios? usuarioActual = (from e in _equiposContext.usuarios
                                       where e.usuario_id == id
                                       select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (usuarioActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            usuarioActual.nombre = usuarioModificar.nombre;
            usuarioActual.documento = usuarioModificar.documento;
            usuarioActual.tipo = usuarioModificar.tipo;
            usuarioActual.carnet = usuarioModificar.carnet;
            usuarioActual.carrera_id = usuarioModificar.carrera_id;


            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos
            _equiposContext.Entry(usuarioActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(usuarioModificar);
        }

        [HttpPut]
        [Route("actualizarEstado/{id}")]

        public IActionResult ActualizarEstadoUsuario(int id, string estado)
        {
            usuarios? usuario = (from u in _equiposContext.usuarios
                                 where u.usuario_id == id
                                 select u).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound();
            }
            usuario.estado = estado;
            _equiposContext.Entry(usuario).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(usuario);
        }
    }
}
