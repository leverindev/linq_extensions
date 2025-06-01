using LinqExtensions.Tests.Models;

namespace LinqExtensions.Tests;

[TestClass]
public class OuterJoinLinqExtensionsTests
{
    [TestMethod]
    public void LeftJoinTest()
    {
        var users =
            from user in GetUsers().ToOuter()
            join login in GetLogins()
                on user.Id equals login.Id
            select user;

        Assert.AreEqual(3, users.Count());
    }

    [TestMethod]
    public void RightJoinTest()
    {
        var logins =
            from user in GetUsers()
            join login in GetLogins().ToOuter()
                on user.Id equals login.Id
            select login;

        Assert.AreEqual(3, logins.Count());
    }

    [TestMethod]
    public void FullJoinTest()
    {
        var userLogins = (
                from user in GetUsers().ToOuter()
                join login in GetLogins().ToOuter()
                    on user.Id equals login.Id
                select new
                {
                    User = user,
                    Login = login,
                })
            .ToArray();

        Assert.AreEqual(4, userLogins.Length);
        Assert.AreEqual(3, userLogins.Count(x => x.User != null));
        Assert.AreEqual(3, userLogins.Count(x => x.Login != null));
    }

    private List<User> GetUsers()
    {
        return new List<User>
        {
            new () { Id = 1, Name = "User1" },
            new () { Id = 2, Name = "User2" },
            new () { Id = 3, Name = "User3" },
        };
    }

    private List<Login> GetLogins()
    {
        return new List<Login>
        {
            new () { Id = 1, Name = "Login1" },
            new () { Id = 2, Name = "Login2" },
            new () { Id = 4, Name = "Login4" },
        };
    }
}
