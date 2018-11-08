class Product {
    public Id: number;
    public UniqueIdentifierHash: number;
    public UniqueIdentifier: string;
    public Url: string;
    public LastChecked: Date;
    public Title: string;
    public Description: string;
    public Breadcrumb: string;
    public Category: string;
    public Image: string;
    public PriceHistory: PriceHistory[];
}

class PriceHistory {
    public Id: number;
    public Price: number;
    public OriginalPrice: number;
    public TimeStamp: Date;
    public ProductId: number;
    public Product: Product;
}

class ProductService {
    private api: string = "api/Products";

    public GetProduct(id: string): Product {
        return new Product();
    }
}