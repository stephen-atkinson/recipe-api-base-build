using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Api.Models.Requests;
using Recipes.Api.Models.Requests.Groups;
using Recipes.Api.Models.Results;
using Recipes.Core.Application;
using Recipes.Core.Domain;
using Recipes.Core.Infrastructure.Database;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupsController : ControllerBase
{
    private readonly RecipesDbContext _recipesDbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public GroupsController(RecipesDbContext recipesDbContext, IDateTimeProvider dateTimeProvider, IMapper mapper)
    {
        _recipesDbContext = recipesDbContext;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateGroupRequest request, CancellationToken cancellationToken)
    {
        var user = await _recipesDbContext.Users
            .FindAsync(new object[] { User.Identity.Name }, cancellationToken);
        
        var recipes = await _recipesDbContext.Recipes
            .Where(r => request.RecipeIds.Contains(r.Id))
            .ToArrayAsync(cancellationToken);

        var utcNow = _dateTimeProvider.UtcNow;
        
        var group = new Group
        {
            Course = request.Course,
            Diet = request.Diet,
            Name = request.Name,
            Recipes = recipes,
            ApplicationUser = user,
            Created = utcNow,
            LastUpdated = utcNow,
        };

        await _recipesDbContext.Groups.AddAsync(group, CancellationToken.None);

        await _recipesDbContext.SaveChangesAsync(CancellationToken.None);

        var dto = _mapper.Map<GroupDto>(group);
        
        var url = Url.Action("ReadSingle", new { group.Id });

        return Created(url!, dto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ReadSingle(int id, CancellationToken cancellationToken)
    {
        var group = await _recipesDbContext.Groups
            .Include(g => g.ApplicationUser)
            .Include(g => g.Recipes)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        var dto = _mapper.Map<GroupDto>(group);

        return Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery]GetGroupsRequest request, CancellationToken cancellationToken)
    {
        var skip = (request.Page - 1)  * request.PageSize;

        var recipes = await _recipesDbContext.Groups
            .Include(r => r.ApplicationUser)
            .Include(r => r.Recipes)
            .Where(r => request.Course == null || r.Course == request.Course)
            .Where(r => request.Diet == null || r.Diet == request.Diet)
            .OrderBy(r => r.Id)
            .Skip(skip)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        var dtos = _mapper.Map<IReadOnlyCollection<GroupDto>>(recipes);

        return Ok(dtos);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateGroupRequest request, CancellationToken cancellationToken)
    {
        var group = await _recipesDbContext.Groups
            .Include(g => g.ApplicationUser)
            .Include(g => g.Recipes)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        
        var recipes = await _recipesDbContext.Recipes
            .Where(r => request.RecipeIds.Contains(r.Id))
            .ToArrayAsync(cancellationToken);
        
        group.Course = request.Course;
        group.Diet = request.Diet;
        group.Name = request.Name;
        group.Recipes = recipes;
        group.LastUpdated = _dateTimeProvider.UtcNow;
        
        await _recipesDbContext.SaveChangesAsync(CancellationToken.None);
        
        var dto = _mapper.Map<GroupDto>(group);
        
        return Ok(dto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var group = await _recipesDbContext.Groups.FindAsync(new object[] { id }, cancellationToken);

        _recipesDbContext.Groups.Remove(group);

        await _recipesDbContext.SaveChangesAsync(CancellationToken.None);

        return NoContent();
    }
}