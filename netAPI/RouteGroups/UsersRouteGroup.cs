namespace netAPI.RouteGroups;
using netAPI.Models.Users;
using netAPI.Services;

public class UsersRouteGroup
{
    private IUserService _userService;

    public UsersRouteGroup(IUserService userService)
    {
        _userService = userService;
    }
    public IResult GetAll()
    {   
        var users = _userService.GetAll();
        return Results.Ok(users);
    }
    public IResult GetById(int id)
    {
        var user = _userService.GetById(id);
        return Results.Ok(user);
    }
    public IResult Create(CreateRequest model)
    {
        _userService.Create(model);
        return Results.Ok(new { message = "User created" });
    }
    public IResult Update(int id, UpdateRequest model)
    {
        _userService.Update(id, model);
        return Results.Ok(new { message = "User updated" });
    }
    public IResult Delete(int id)
    {
        _userService.Delete(id);
        return Results.Ok(new { message = "User deleted" });
    }

}