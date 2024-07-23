using MercadoPago.Resource.Payment;
using MercadoPago.Resource;
using MercadoPago.Resource.Payment;
using MercadoPago.Client.Payment;

public class PaymentGateway
{
    public PaymentGateway()
    {
        MercadoPago.Config.MercadoPagoConfig.AccessToken = "APP_USR-752803334614385-051813-42edd38212bbd9ea2ebc085d96cf0365-448038486";
    }

    public async Task<string> CreatePaymentAndGenerateQRCode(decimal amount, string description)
    {
        var paymentRequest = new PaymentCreateRequest
        {
            Order = new PaymentOrderRequest
            {
                Id = 1,
                Type = String.Empty,
            },
            TransactionAmount = amount,
            Description = description,
            PaymentMethodId = "pix", // Para gerar QR Code PIX
            Payer = new PaymentPayerRequest
            {
                Email = "test_user_123@testuser.com"
            }
        };

        var client = new PaymentClient();
        Payment payment = await client.CreateAsync(paymentRequest);


        if (payment.Status == PaymentStatus.Pending)
        {
            string qrCodeBase64 = payment.PointOfInteraction.TransactionData.QrCodeBase64;
            return qrCodeBase64;
        }

        throw new Exception("Falha ao criar o pagamento: " + payment.Status);
    }
}

class Program
{
    static void Main(string[] args)
    {
        PaymentGateway paymentGateway = new PaymentGateway();

        try
        {
            string qrCode = paymentGateway.CreatePaymentAndGenerateQRCode(100.00M, "Descrição do pagamento");
            Console.WriteLine("QR Code gerado com sucesso!");
            // Você pode salvar o QR Code base64 em um arquivo ou exibi-lo em uma interface
            System.IO.File.WriteAllBytes("qrcode.png", Convert.FromBase64String(qrCode));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao criar pagamento: " + ex.Message);
        }
    }
}