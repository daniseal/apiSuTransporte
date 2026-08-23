using apiSuTransporte.DTOs;
using apiSuTransporte.Models;
using Microsoft.EntityFrameworkCore;

namespace apiSuTransporte.Clases
{
    public class ClCamion
    {
        private readonly BdSuTransporteContext _context;

        public ClCamion(BdSuTransporteContext context)
        {
            _context = context;
        }

        public async Task<List<object>> Listar()
        {
            var datos = await (
                from c in _context.Set<Camion>()
                join m in _context.Set<Marca>() on c.IdMarca equals m.IdMarca
                join s in _context.Set<Sede>() on c.IdSede equals s.IdSede
                join tc in _context.Set<TipoCamion>() on c.IdTipoCamion equals tc.IdTipoCamion
                where c.Activo == true
                select new
                {
                    c.IdCamion,
                    c.IdMarca,
                    Marca = m.Nombre,
                    c.IdSede,
                    Sede = s.Nombre,
                    c.IdTipoCamion,
                    Tipo = tc.Nombre,
                    c.Modelo,
                    c.NumeroMotor,
                    c.NumeroChasis,
                    c.Placa,
                    c.Potencia,
                    c.Estado,
                    c.Activo
                }
            ).ToListAsync<object>();

            return datos;
        }

        public async Task<object?> Buscar(int id)
        {
            var dato = await (
                from c in _context.Set<Camion>()
                join m in _context.Set<Marca>() on c.IdMarca equals m.IdMarca
                join s in _context.Set<Sede>() on c.IdSede equals s.IdSede
                join tc in _context.Set<TipoCamion>() on c.IdTipoCamion equals tc.IdTipoCamion
                where c.IdCamion == id
                select new
                {
                    c.IdCamion,
                    c.IdMarca,
                    Marca = m.Nombre,
                    c.IdSede,
                    Sede = s.Nombre,
                    c.IdTipoCamion,
                    Tipo = tc.Nombre,
                    c.Modelo,
                    c.NumeroMotor,
                    c.NumeroChasis,
                    c.Placa,
                    c.Potencia,
                    c.Estado,
                    c.Activo
                }
            ).FirstOrDefaultAsync();

            return dato;
        }

        public async Task<string> Insertar(CamionDTO dto)
        {
            bool existePlaca = await _context.Set<Camion>()
                .AnyAsync(c => c.Placa == dto.Placa);

            if (existePlaca)
                return "Ya existe un camión con esa placa";

            bool existeMotor = await _context.Set<Camion>()
                .AnyAsync(c => c.NumeroMotor == dto.NumeroMotor);

            if (existeMotor)
                return "Ya existe un camión con ese número de motor";

            bool existeChasis = await _context.Set<Camion>()
                .AnyAsync(c => c.NumeroChasis == dto.NumeroChasis);

            if (existeChasis)
                return "Ya existe un camión con ese número de chasis";

            bool existeTipoCamion = await _context.Set<TipoCamion>()
                .AnyAsync(tc => tc.IdTipoCamion == dto.IdTipoCamion && tc.Activo == true);

            if (!existeTipoCamion)
                return "El tipo de camión seleccionado no existe o está inactivo";

            Camion camion = new Camion
            {
                IdMarca = dto.IdMarca,
                IdSede = dto.IdSede,
                IdTipoCamion = dto.IdTipoCamion,
                Modelo = dto.Modelo,
                NumeroMotor = dto.NumeroMotor,
                NumeroChasis = dto.NumeroChasis,
                Placa = dto.Placa,
                Potencia = dto.Potencia,
                Estado = dto.Estado,
                Activo = true
            };

            _context.Set<Camion>().Add(camion);
            await _context.SaveChangesAsync();

            return "Camión registrado correctamente";
        }

        public async Task<string> Modificar(int id, CamionDTO dto)
        {
            var camion = await _context.Set<Camion>().FindAsync(id);

            if (camion == null)
                return "No se encontró el camión";

            bool existePlaca = await _context.Set<Camion>()
                .AnyAsync(c => c.Placa == dto.Placa && c.IdCamion != id);

            if (existePlaca)
                return "Ya existe otro camión con esa placa";

            bool existeMotor = await _context.Set<Camion>()
                .AnyAsync(c => c.NumeroMotor == dto.NumeroMotor && c.IdCamion != id);

            if (existeMotor)
                return "Ya existe otro camión con ese número de motor";

            bool existeChasis = await _context.Set<Camion>()
                .AnyAsync(c => c.NumeroChasis == dto.NumeroChasis && c.IdCamion != id);

            if (existeChasis)
                return "Ya existe otro camión con ese número de chasis";

            bool existeTipoCamion = await _context.Set<TipoCamion>()
                .AnyAsync(tc => tc.IdTipoCamion == dto.IdTipoCamion && tc.Activo == true);

            if (!existeTipoCamion)
                return "El tipo de camión seleccionado no existe o está inactivo";

            camion.IdMarca = dto.IdMarca;
            camion.IdSede = dto.IdSede;
            camion.IdTipoCamion = dto.IdTipoCamion;
            camion.Modelo = dto.Modelo;
            camion.NumeroMotor = dto.NumeroMotor;
            camion.NumeroChasis = dto.NumeroChasis;
            camion.Placa = dto.Placa;
            camion.Potencia = dto.Potencia;
            camion.Estado = dto.Estado;
            camion.Activo = dto.Activo;

            await _context.SaveChangesAsync();

            return "Camión modificado correctamente";
        }

        public async Task<string> Eliminar(int id)
        {
            var camion = await _context.Set<Camion>().FindAsync(id);

            if (camion == null)
                return "No se encontró el camión";

            camion.Activo = false;

            await _context.SaveChangesAsync();

            return "Camión eliminado correctamente";
        }
    }
}