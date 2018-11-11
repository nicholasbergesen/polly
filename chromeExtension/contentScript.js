window.addEventListener('load', function () {
    var dailyDealItems = $('.daily-deal-item');

    for (var i = 0; i < dailyDealItems.length; i++) {
        updateProduceHtml(dailyDealItems[i]);
    }

    function updateProduceHtml(parentElement) {
        var productUrl = parentElement.childNodes[0].getAttribute("href");
        var splitUrl = productUrl.split('/');
        var productId = splitUrl[splitUrl.length - 1];
        var currentPrice = $(parentElement).find(".price").find("span")[1].innerText;
        currentPrice = currentPrice.replace(',', '');
        $.ajax({
            url: "https://nicholasb.ddns.net/api/products/" + productId + "/" + currentPrice,
            success: function (result) {
                var priceNode = document.createElement("span");
                var textNode = document.createTextNode("R " + result);
                priceNode.style.color = "blue";
                priceNode.style.cssFloat = "right";
                priceNode.appendChild(textNode);
                parentElement.childNodes[3].appendChild(priceNode);
            }
        });
    }
});