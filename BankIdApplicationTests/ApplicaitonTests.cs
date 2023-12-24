using Application.Cache.Interfaces;
using Application.Clients.Interfaces;
using Application.Clients.Models.Responses;
using Application.Clients.Models;
using Application.Clients.Options;
using Application.Handlers;
using Application.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;
using Application.Clients.Models.Requests;
using Application.Models.Requests;
using Application.Models.Responses;
using Application.Exceptions;

namespace BankIdApplicationTests;

[TestFixture]
public class ApplicaitonTests
{
    private BankIdStartHandler _bankIdStartHandler;
    private BankIdCollectHandler _bankIdCollectHandler;
    private BankIdCancelHandler _bankIdCancelHandler;
    private BankIdQrCodeHandler _bankIdQrCodeHandler;
    private Mock<ILogger> _loggerMock;
    private Mock<IBankIdClient> _clientMock;
    private Mock<ICache> _cacheMock;
    private Mock<IOptions<BankIdOptions>> _optionsMock;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private BankIdStartResponse expectedResponse;
    private User userData;
    private string qrPrefix;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger>();
        _clientMock = new Mock<IBankIdClient>();
        _cacheMock = new Mock<ICache>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var bankIdOptions = new BankIdOptions
        {
            CacheBankIdOrderMin = 2
        };
        _optionsMock = new Mock<IOptions<BankIdOptions>>();
        _optionsMock.Setup(x => x.Value).Returns(bankIdOptions);

        _bankIdStartHandler = new BankIdStartHandler(
            _clientMock.Object,
            _optionsMock.Object,
            _httpContextAccessorMock.Object,
            _cacheMock.Object,
            _loggerMock.Object
        );

        _bankIdCollectHandler = new BankIdCollectHandler(
        _clientMock.Object,
        _optionsMock.Object,
        _cacheMock.Object,
        _loggerMock.Object
        );

        _bankIdCancelHandler = new BankIdCancelHandler(
        _clientMock.Object,
        _cacheMock.Object
        );

        _bankIdQrCodeHandler = new BankIdQrCodeHandler(
        _cacheMock.Object,
        _loggerMock.Object
        );

        qrPrefix = "bankid";

        expectedResponse = new BankIdStartResponse
        {
            Status = BankIdStatus.Ok,
            OrderRef = "826f8421-9c0a-4ad8-a8d9-3e5904280062",
            AutoStartToken = "ff9f43fd-6279-42a8-ab3a-dd2a3187c94e",
            QrStartToken = "a22dcea0-aaa7-4951-8de5-4e6e1d087a33",
            QrStartSecret = "a8a77aa9-35e3-4a41-88e4-bdf1a3acb6ad",
            AuthStartTime = DateTime.UtcNow
        };

        userData = new User
        {
            GivenName = "Deadpool",
            Name = "Wade",
            Surname = "Wilson",
            PersonalNumber = "888888-88"
        };
    }

    [Test]
    public async Task AuthenticationStartHandle_Success()
    {
        // Arrange
        var request = new StartRequest
        {
            ReturnUrl = "https://mypagesuise-s.azurewebsites.net/callback"
        };

        _httpContextAccessorMock
            .Setup(x => x.HttpContext.Connection.RemoteIpAddress)
            .Returns(System.Net.IPAddress.Parse("192.168.1.1"));

        var simplifiedResponse = new StartResponse(expectedResponse.OrderRef, expectedResponse.AutoStartToken, "https://mypagesuise-s.azurewebsites.net/callback");

        _clientMock.Setup(c => c.StartAuthenticationAsync(It.IsAny<BankIdStartRequest>()))
            .ReturnsAsync(expectedResponse);
        _cacheMock.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<BankIdStartResponse>(), It.IsAny<TimeSpan>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _bankIdStartHandler.Handle(request, new CancellationToken());

        Assert.NotNull(result);
        Assert.AreEqual(simplifiedResponse.OrderRef, result.OrderRef);
        Assert.AreEqual(simplifiedResponse.IosBankAutoStartUrl, result.IosBankAutoStartUrl);
        Assert.AreEqual(simplifiedResponse.BankIdAutoStartUrl, result.BankIdAutoStartUrl);
    }

    [Test]
    public async Task AuthenticationStartHandle_ThrowsBankIdException()
    {
        // Arrange
        var request = new StartRequest
        {
            ReturnUrl = "https://mypagesuise-s.azurewebsites.net/callback"
        };

        _httpContextAccessorMock
            .Setup(x => x.HttpContext.Connection.RemoteIpAddress)
            .Returns(System.Net.IPAddress.Parse("192.168.1.1"));

        // Act
        _clientMock.Setup(c => c.StartAuthenticationAsync(It.IsAny<BankIdStartRequest>()))
            .ThrowsAsync(new BankIdException("Unexpected BankID error. Code: invalidParameters. Details: invalidParameters"));

        // Assert
        Assert.ThrowsAsync<BankIdException>(async () => await _bankIdStartHandler.Handle(request, new CancellationToken()));
    }

    [TestCase(1)]
    [TestCase(5)]
    [TestCase(10)]
    [TestCase(25)]
    [TestCase(59)]
    public async Task GenerateQrCodeHandle_Success(int elapsedSeconds)
    {
        // Arrange
        var request = new QrCodeRequest { OrderRef = expectedResponse.OrderRef };
        expectedResponse.AuthStartTime = DateTime.UtcNow.AddSeconds(-elapsedSeconds);
        _cacheMock.Setup(x => x.GetAsync<BankIdStartResponse>(expectedResponse.OrderRef)).ReturnsAsync(expectedResponse);

        // Act
        var result = await _bankIdQrCodeHandler.Handle(request, new CancellationToken());

        var computedHash = _bankIdQrCodeHandler.HashHMAC(expectedResponse.QrStartSecret, elapsedSeconds.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual($"{qrPrefix}.{expectedResponse.QrStartToken}.{elapsedSeconds}.{computedHash}", result.QrData);
    }

    [TestCase(1)]
    [TestCase(5)]
    [TestCase(10)]
    [TestCase(25)]
    [TestCase(59)]
    public async Task GenerateQrCodeHandle_TimeMismatch_Failure(int elapsedSeconds)
    {
        // Arrange
        var request = new QrCodeRequest { OrderRef = expectedResponse.OrderRef };
        expectedResponse.AuthStartTime = DateTime.UtcNow.AddSeconds(-elapsedSeconds);
        _cacheMock.Setup(x => x.GetAsync<BankIdStartResponse>(expectedResponse.OrderRef)).ReturnsAsync(expectedResponse);

        // Act
        var result = await _bankIdQrCodeHandler.Handle(request, new CancellationToken());

        // add one more second in order to have a time mismatch
        var computedHash = _bankIdQrCodeHandler.HashHMAC(expectedResponse.QrStartSecret, (elapsedSeconds + 1).ToString());

        // Assert
        Assert.NotNull(result);
        Assert.AreNotEqual($"{qrPrefix}.{expectedResponse.QrStartToken}.{elapsedSeconds}.{computedHash}", result.QrData);
    }

    [Test]
    public async Task AuthenticationCancelHadnle_Success()
    {
        // Arrange
        var request = new CancelRequest { OrderRef = "826f8421-9c0a-4ad8-a8d9-3e5904280062" };

        // Mocking the response for CancelAuthenticationAsync
        _clientMock.Setup(x => x.CancelAuthenticationAsync(request)).Returns(Task.CompletedTask);

        // Act
        await _bankIdCancelHandler.Handle(request, new CancellationToken());

        // Assert
        _clientMock.Verify(x => x.CancelAuthenticationAsync(request), Times.Once);
        _cacheMock.Verify(x => x.Remove(request.OrderRef), Times.Once);
    }

    [Test]
    public async Task AuthenticationCollectHandle_Success()
    {
        // Arrange
        var request = new CollectRequest { OrderRef = expectedResponse.OrderRef };

        _cacheMock.Setup(x => x.GetAsync<BankIdStartResponse>(expectedResponse.OrderRef))
            .ReturnsAsync(expectedResponse);

        _clientMock.Setup(c => c.CollectAuthenticationAsync(request))
            .ReturnsAsync(new CollectResponse(userData.PersonalNumber,userData.Name,userData.Surname, userData.GivenName));

        // Act
        var result = await _bankIdCollectHandler.Handle(request, new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(BankIdStatus.Ok, result.Status);
        Assert.IsTrue(result.IsCompleted);
        Assert.AreEqual(userData.PersonalNumber, result.Ssn);
        Assert.AreEqual(userData.Name, result.Name);
        Assert.AreEqual(userData.GivenName, result.GivenName);
        Assert.AreEqual(userData.Surname, result.Surname);
    }
}