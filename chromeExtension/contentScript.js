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
            url: "https://www.pollychron.com/api2/products/" + productId + "/" + currentPrice,
            success: function (result) {
                console.log(result);
                var priceLink = document.createElement("a");
                priceLink.setAttribute('href', result.Url);
                priceLink.setAttribute('target', '_blank');
                var priceNode = document.createElement("span");
                if(result.Price != 0) {
                    var discount = (result.Price - currentPrice) / result.Price * 100;
                    var textNode = document.createTextNode("R " + result.Price + " (" + Math.round(discount) + "%)");
                    priceLink.setAttribute('style', 'top:200; left: 130px; z-index:1000; display: block; position:absolute;');
                }
                else {
                    var textNode = document.createTextNode("No recent change");
                    priceLink.setAttribute('style', 'top:200; left: 110px; z-index:1000; display: block; position:absolute;');
                }
                priceNode.style.color = "blue";
                priceNode.style.cssFloat = "right";
                priceNode.setAttribute('onMouseOver', "this.style.color='green'");
                priceNode.setAttribute('onMouseOut', "this.style.color='blue'");
                priceNode.appendChild(textNode);
                priceLink.appendChild(priceNode);
                parentElement.childNodes[3].appendChild(priceLink);
            }
        });
    }
});