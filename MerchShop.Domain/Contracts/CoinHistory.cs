namespace MerchShop.Domain.Contracts;

public record CoinHistory(
    List<ReceivedCoins> Received,
    List<SendCoins> Sent
);