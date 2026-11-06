using InvoiceTracker.Api.Controllers;
using InvoiceTracker.Domain.Interfaces.Services;
using InvoiceTracker.Domain.Models.Invoice;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InvoiceTracker.UnitTests;

public sealed class InvoiceControllerTests
{
    [Fact]
    public async Task GetById_ReturnsOk_WhenInvoiceExists()
    {
        var invoice = CreateInvoice();
        var service = new Mock<IInvoiceService>();
        service
            .Setup(item => item.GetByIdAsync(7, It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoice);

        var controller = new InvoiceController(service.Object);
        var result = await controller.GetById(7, CancellationToken.None);

        var response = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(invoice, response.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenInvoiceDoesNotExist()
    {
        var service = new Mock<IInvoiceService>();
        service
            .Setup(item => item.GetByIdAsync(7, It.IsAny<CancellationToken>()))
            .ReturnsAsync((InvoiceDto?)null);

        var controller = new InvoiceController(service.Object);
        var result = await controller.GetById(7, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Add_ReturnsCreatedAtAction_WhenInvoiceIsCreated()
    {
        var invoice = CreateInvoice();
        var service = new Mock<IInvoiceService>();
        service
            .Setup(item => item.AddAsync(It.IsAny<CreateInvoiceRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoice);

        var controller = new InvoiceController(service.Object);
        var result = await controller.Add(
            new CreateInvoiceRequest(
                2,
                "INV-7",
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(30),
                1,
                100,
                "tester"),
            CancellationToken.None);

        var response = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(InvoiceController.GetById), response.ActionName);
        Assert.Same(invoice, response.Value);
    }

    [Fact]
    public async Task Remove_ReturnsNoContent_WhenInvoiceIsRemoved()
    {
        var service = new Mock<IInvoiceService>();
        service
            .Setup(item => item.RemoveInvoice(7, "tester", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var controller = new InvoiceController(service.Object);
        var result = await controller.Remove(7, "tester", CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
        service.Verify(item => item.RemoveInvoice(7, "tester", It.IsAny<CancellationToken>()), Times.Once);
    }

    private static InvoiceDto CreateInvoice() => new(
        7,
        2,
        "INV-7",
        DateTime.UtcNow,
        DateTime.UtcNow.AddDays(30),
        1,
        100,
        "Pending",
        "Test Customer",
        []);
}
