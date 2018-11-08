var Product = /** @class */ (function () {
    function Product() {
    }
    return Product;
}());
var PriceHistory = /** @class */ (function () {
    function PriceHistory() {
    }
    return PriceHistory;
}());
var ProductService = /** @class */ (function () {
    function ProductService() {
        this.api = "api/Products";
    }
    ProductService.prototype.GetProduct = function (id) {
        return new Product();
    };
    return ProductService;
}());
//# sourceMappingURL=script.js.map