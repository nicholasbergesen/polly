const apiUrl = "https://priceboar.com/api/";
const apiProducts = apiUrl + "products/";

window.addEventListener('load', function () {
    setTimeout(() => {
        checkPage();
    }, 1000);
});

function checkPage() {
    let dailyDealItems = $('.daily-deal-item');
    let isProductPage = this.isProductPage();
    let wishlist = $("#wishlist");    

    if(dailyDealItems.length > 0) {
        for (let i = 0; i < dailyDealItems.length; i++) {
            showDailyDealPrice(dailyDealItems[i]);
        }
    }
    else if(wishlist.length > 0) {
        addPriceColumnToWishlist(wishlist);
    }
    else if(isProductPage) {
        let buyBox = $('div.pdp-module_sidebar-buybox_1m6Sm');
        updateProductHtml(buyBox);
    }
}

///DAILY DEALS
function showDailyDealPrice(parentElement) {
    let productId = getProductIdFromUrl(parentElement.childNodes[0].getAttribute("href"));
    let currentPrice = $(parentElement).find(".price").find("span")[1].innerText;
    currentPrice = currentPrice.replace(',', '');
    let udpatesRequired = new Array();
    $.ajax({
        url: apiProducts + productId + "/" + currentPrice,
        success: function (result) {
            let priceLink = createPriceNode(result, currentPrice);
            parentElement.childNodes[3].appendChild(priceLink);
            if(result.Status == "UpdateRequeired") {
                udpatesRequired.push("https://api.takealot.com/rest/v-1-8-0/product-details/" + productId + "?platform=desktop")
            }
        }
    });

    udpatesRequired.forEach(updateUrl => {
        $.ajax({
            url: updateUrl,
            success: function (takealotJSON) {
                $.ajax({
                    url: apiUrl,
                    type: "POST",
                    data: takealotJSON
                });
            }
        });
    });
}

function createPriceNode(result, currentPrice) {
    let priceLink = document.createElement("a");
    priceLink.setAttribute('href', result.Url);
    priceLink.setAttribute('target', '_blank');
    let priceNode = document.createElement("span");
    let textNode = document.createTextNode("No recent change");
    if (result.Price != 0) {
        let discount = (result.Price - currentPrice) / result.Price * 100;
        textNode = document.createTextNode("R " + result.Price + " (" + Math.round(discount) + "%)");
        priceLink.setAttribute('style', 'top:200; left: 130px; z-index:1000; display: block; position:absolute;');
    }
    else {
        priceLink.setAttribute('style', 'top:200; left: 110px; z-index:1000; display: block; position:absolute;');
    }
    priceNode.style.color = "blue";
    priceNode.style.cssFloat = "right";
    priceNode.setAttribute('onMouseOver', "this.style.color='green'");
    priceNode.setAttribute('onMouseOut', "this.style.color='blue'");
    priceNode.appendChild(textNode);
    priceLink.appendChild(priceNode);
    return priceLink;
}

///PRODUCT
function updateProductHtml(parentElement) {
    addChartToPage();
    let productId = getProductIdFromUrl(window.location.href);
    $(parentElement).attr("id", "#realPrice"); //workaround for takealot being shit, allows me to append to the parent element
    let currentPrice = $(parentElement).find(".buybox-module_price_2YUFa").children("span.currency").text();
    currentPrice = currentPrice.replace(',', '').replace('R', '');
    let udpatesRequired = new Array();
    $.ajax({
        url: apiProducts + productId + "/" + currentPrice,
        success: function (result) {
            let priceLink = createSimplePriceNode(result, currentPrice);
            let realPrice = document.getElementById("#realPrice");
            realPrice.insertBefore(priceLink, realPrice.childNodes[1]);
            if(result.Status == "UpdateRequeired") {
                udpatesRequired.push("https://api.takealot.com/rest/v-1-8-0/product-details/" + productId + "?platform=desktop")
            }
        }
    });

    udpatesRequired.forEach(updateUrl => {
        $.ajax({
            url: updateUrl,
            success: function (takealotJSON) {
                $.ajax({
                    url: apiUrl,
                    type: "POST",
                    data: takealotJSON
                });
            }
        });
    });
}

function addChartToPage() {
    let productId = getProductIdFromUrl(window.location.href);

    let chartElement = document.createElement("canvas");
    chartElement.setAttribute("id", "priceboarChart");
    chartElement.setAttribute("height", "80");
    chartElement.setAttribute("class", "150");
    $(".pdp-main-panel").attr("id", "#mainPagePricceBoar");
    let mainPage = document.getElementById("#mainPagePricceBoar");
    mainPage.insertBefore(chartElement, mainPage.childNodes[1]);

    $.ajax({
        url: apiProducts + "pricehistory/" + productId,
        success: function (result) {
            createChartNode(result.Price, result.Date);
        }
    });
}

function createChartNode(prices, dates) {
    let chartElement = document.getElementById("priceboarChart");
    let priceBoarChart = new Chart(chartElement, {
        type: 'line',
        lineTension: 1,
        showLine: true,
        spanGaps: true,
        fill: false,
        data: {
            labels: dates,
            datasets: [{
                label: 'Price',
                data: prices,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            hover: {
                mode: 'nearest',
            },
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        labelString: 'Date'
                    }
                }],
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        labelString: 'Price'
                    },
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}

function createSimplePriceNode(result, currentPrice) {
    let priceLink = document.createElement("a");
    priceLink.setAttribute('href', result.Url);
    priceLink.setAttribute('target', '_blank');
    let priceNode = document.createElement("span");
    let textNode = document.createTextNode("No recent change");
    if (result.Price != 0) {
        let discount = (result.Price - currentPrice) / result.Price * 100;
        textNode = document.createTextNode("R " + result.Price + " (" + Math.round(discount) + "%)");
    }
    priceNode.style.color = "blue";
    priceNode.setAttribute('onMouseOver', "this.style.color='green'");
    priceNode.setAttribute('onMouseOut', "this.style.color='blue'");
    priceNode.appendChild(textNode);
    priceLink.appendChild(priceNode);

    let newDiv = document.createElement("div");
    newDiv.appendChild(priceLink);
    return newDiv;
}

///WISHLIST
function addPriceColumnToWishlist(wishlistTable) {
    let realTable = wishlistTable[0];
    //header
    let tr = realTable.rows[0];
    let th = document.createElement('th');
    th.setAttribute('class', 'col-price');
    th.innerHTML = "Price Boar";
    tr.appendChild(th);

    //get product Id and display the last price
    for (let i = 1; i < realTable.rows.length; i++) {
        let productId = getProductIdFromUrl($(realTable.rows[i].cells[2]).find("a")[0].getAttribute("href"));
        let currentPrice = realTable.rows[i].cells[5].innerText.replace(',', '').replace('R', '').replace(' ', '');
        $.ajax({
            url: apiProducts + productId + "/" + currentPrice,
            success: function (result) {
                let display = "unchanged";
                if(result.Price > 0){
                    let discount = (result.Price - currentPrice) / result.Price * 100;
                    display = "R " + result.Price + " (" + Math.round(discount) + "%)";
                }
                createCell(realTable.rows[i].insertCell(realTable.rows[i].cells.length), display, 'col-price');
            }
        });
    }
}

function createCell(cell, text, style) {
    txt = document.createTextNode(text);
    cell.setAttribute('class', style);
    cell.style = "color:blue";
    cell.appendChild(txt);
}

function getProductIdFromUrl(url) {
    let splitUrl = url.split('/');
    let productId = splitUrl[splitUrl.length - 1];
    return productId;
}

function isProductPage() {
    let splitUrl = window.location.href.split('/');
    let productId = splitUrl[splitUrl.length - 1];
    return productId.startsWith("PLID");
}
