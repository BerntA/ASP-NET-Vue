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
        await _contactService.RemoveAll();

        var items = await _contactService.Contacts();
        Assert.Empty(items);

        await _contactService.AddContact(_entry);

        items = await _contactService.Contacts();
        Assert.NotEmpty(items);
    }

    [Fact]
    public async Task AddContactTest()
    {
        await _contactService.RemoveAll();
        var contact = await _contactService.AddContact(_entry);
        Assert.NotNull(contact);
        Assert.NotEqual(Guid.Empty, contact.Id);
    }

    [Fact]
    public async Task UpdateContactTest()
    {
        await _contactService.RemoveAll();
        Assert.Null(await _contactService.UpdateContact(_entry));

        var contact = await _contactService.AddContact(_entry);
        Assert.NotNull(contact);
        _entry.Id = contact.Id;

        Assert.NotNull(await _contactService.UpdateContact(_entry));
    }

    [Fact]
    public async Task RemoveContactTest()
    {
        await _contactService.RemoveAll();
        var contact = await _contactService.AddContact(_entry);
        Assert.NotNull(contact);
        Assert.True(await _contactService.RemoveContact(contact.Id));
    }
}