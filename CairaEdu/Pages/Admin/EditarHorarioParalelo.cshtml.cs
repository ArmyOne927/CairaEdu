using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CairaEdu.Pages.Admin;

public class EditarHorarioParaleloModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EditarHorarioParaleloModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty(SupportsGet = true)]
    public int? Id { get; set; }
    public string NombreParalelo { get; set; } = "";
    public List<object> EventosHorario { get; set; } = [];
    public string NombreCurso { get; set; } = "";

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == null) return BadRequest("Parámetro 'id' faltante.");
        var paralelo = await _context.Paralelos
            .Include(p => p.Curso)
            .Include(p => p.Horarios)
            .ThenInclude(h => h.Materia)
            .FirstOrDefaultAsync(p => p.Id == Id);

        if (paralelo == null) return BadRequest("Parámetro 'id' faltante.");
        NombreParalelo = paralelo.Nombre;
        NombreCurso = paralelo.Curso.Nombre;

        EventosHorario = paralelo.Horarios
            .Where(h => h.Estado == 'A')
            .Select(h => new
            {
                id = h.Id,
                title = h.Materia.Nombre,
                start = h.HoraInicio,
                end = h.HoraFin
            }).Cast<object>().ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync([FromBody] EventoInput nuevo)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        var paralelo = await _context.Paralelos
            .Include(p => p.Curso)
            .Include(p => p.Horarios)
            .ThenInclude(h => h.Materia)
            .FirstOrDefaultAsync(p => p.Id == Id);


        var materia = await _context.Materias
            .FirstOrDefaultAsync(m => m.Nombre == nuevo.Title && m.InstitucionId == user.InstitucionId);

        if (materia == null) return BadRequest("Materia no encontrada o no pertenece a tu institución.");

        _context.HorariosParalelo.Add(new HorarioParalelo
        {
            ParaleloId = Id.Value,
            MateriaId = materia.Id,
            HoraInicio = DateTime.Parse(nuevo.Start),
            HoraFin = DateTime.Parse(nuevo.End),
            Estado = 'A'
        });

        await _context.SaveChangesAsync();
        return new JsonResult(new { success = true });
    }

    public async Task<IActionResult> OnPutAsync([FromBody] EventoEdit evento)
    {
        var horario = await _context.HorariosParalelo.FindAsync(evento.Id);
        if (horario == null) return NotFound();

        horario.HoraInicio = DateTime.Parse(evento.Start);
        horario.HoraFin = DateTime.Parse(evento.End);

        await _context.SaveChangesAsync();
        return new JsonResult(new { success = true });
    }

    public async Task<IActionResult> OnDeleteAsync([FromBody] EventoDelete evento)
    {
        var horario = await _context.HorariosParalelo.FindAsync(evento.Id);
        if (horario == null) return NotFound();

        horario.Estado = 'I'; // Borrado lógico
        await _context.SaveChangesAsync();
        return new JsonResult(new { success = true });
    }

    public class EventoInput { public string Title { get; set; } = ""; public string Start { get; set; } = ""; public string End { get; set; } = ""; }
    public class EventoEdit { public int Id { get; set; } public string Start { get; set; } = ""; public string End { get; set; } = ""; }
    public class EventoDelete { public int Id { get; set; } }
}
