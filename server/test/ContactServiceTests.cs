using lib.Models;
using lib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace test;

public class ContactServiceTests
{
    private ContactService _contactService;
    private UpdateContact _entry = new UpdateContact
    {
        Name = "Test",
        Address = "ABC",
        Phone = "40201040",
    };

    public ContactServiceTests()
    {
        var serviceProvider = TestServices.GetServiceProvider();
        _contactService = serviceProvider.GetRequiredService<ContactService>();
    }

    [Fact]
    public async Task GetContactsTest()
    {
        var items = await _contactService.Contacts();
        Assert.Empty(items);

        await _contactService.AddContact(_entry);

        items = await _contactService.Contacts();
        Assert.NotEmpty(items);
    }

    [Fact]
    public async Task AddContactTest()
    {
        var id = await _contactService.AddContact(_entry);
        Assert.NotEqual(Guid.Empty, id);
    }

    [Fact]
    public async Task UpdateContactTest()
    {
        Assert.False(await _contactService.UpdateContact(_entry));

        var id = await _contactService.AddContact(_entry);
        _entry.Id = id;

        Assert.True(await _contactService.UpdateContact(_entry));
    }

    [Fact]
    public async Task RemoveContactTest()
    {
        var id = await _contactService.AddContact(_entry);
        Assert.True(await _contactService.RemoveContact(id));
    }
}