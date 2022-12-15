﻿using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using WebBriliFresh.Models;
namespace WebBriliFresh.Models.DAO
{
    public class ProductServices
    {

        private BriliFreshDbContext db = new BriliFreshDbContext();
        public Product GetProductByID(int ID)
        {
            return db.Products.Find(ID);
        }
        public Product checkNewProduct(){

            return (Product)db.Products.Where(x => (DateTime.Now - (DateTime)x.StartDate).TotalDays <= 3);
        }
        public Product checkDealProduct() {
            return (Product)db.Products.Where(x => db.DiscountProducts.Select(y => y.ProId).Contains(x.ProId));
        }
        public Product checkBriliProduct() {

            return (Product)db.Products.Where(x => x.Source.Equals("Sản phẩm của Brili"));
        }
        public Product checkImportProduct() {

            return (Product)db.Products.Where(x => x.Source.Equals("Hàng Nhập khẩu"));
        }



        public async Task<List<Product>> SearchProducts(string? search,int? minimumPrice, int? maximumPrice, int? typeID, int? storeID, List<string> selected, int? sortBy, int pageNo, int pageSize)
        {


            var products = await db.Products.ToListAsync();
            var store = db.Stocks.Where(x => x.StoreId == storeID);
            if (storeID.HasValue) {
                products = products.Where(x => store.Select(y => y.ProId).Contains(x.ProId)).ToList();
            }
           
            if (typeID.HasValue)
            {
                products = products.Where(x => x.TypeId == typeID.Value).ToList();
            }
            if (search != null && search !="0")
            {
                products = products.Where(x => x.ProName.ToLower().Contains(search.ToLower())).ToList();
            }
            else
            {

                products = products.ToList();

            }

            if (minimumPrice.HasValue)
            {
                products = products.Where(x => x.Price >= minimumPrice.Value).ToList();
            }
            if (maximumPrice.HasValue)
            {
                products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case 1:
                        products = products.OrderBy(x => x.Price).ToList();
                        break;
                    case 2:
                        products = products.OrderByDescending(x => x.Price).ToList();
                        break;
                    default:
                        products = products.OrderByDescending(x => x.Price).ToList();
                        break;
                }
            }
            //selectd {1 : deal soc ; 2: hang moi ; 3 : (source) hang Brili ; 4: (source) hang nhap khau}
            if (selected.Count()!=0)
            {

                for (int i = 0; i < selected.Count; i++) {
                    if (selected[i] == "1") {
                        products = products.Where(x => (DateTime.Now - (DateTime)x.StartDate).TotalDays <= 3).ToList();

                    }
                    if (selected[i] == "2") {
                        products = products.Where(x => db.DiscountProducts.Select(y => y.ProId).Contains(x.ProId)).ToList();

                    }
                    if (selected[i] == "3") {
                        products = products.Where(x => x.Source.Equals("Sản phẩm của Brili")).ToList();

                    }
                    if (selected[i] == "4") {
                        products = products.Where(x => x.Source.Equals("Hàng Nhập khẩu")).ToList();

                    }
                }

            }
            return products.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        }

        public async Task<int> SearchProductsCount(string? search, int? minimumPrice, int? maximumPrice, int? typeID, int? storeID, List<string> selected, int? sortBy)
        {
            var products = await db.Products.ToListAsync();

            var store = db.Stocks.Where(x => x.StoreId == storeID);
            if (storeID.HasValue)
            {
                products = products.Where(x => store.Select(y => y.ProId).Contains(x.ProId)).ToList();
            }

            if (typeID.HasValue)
            {
                products = products.Where(x => x.TypeId == typeID.Value).ToList();
            }

            if (search != null && search != "0")
            {
                products = products.Where(x => x.ProName.ToLower().Contains(search.ToLower())).ToList();
            }
            else
            {

                products = products.ToList();

            }

            if (minimumPrice.HasValue)
            {
                products = products.Where(x => x.Price >= minimumPrice.Value).ToList();
            }
            if (maximumPrice.HasValue)
            {
                products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case 1:
                        products = products.OrderBy(x => x.Price).ToList();
                        break;
                    case 2:
                        products = products.OrderByDescending(x => x.Price).ToList();
                        break;
                    default:
                        products = products.OrderByDescending(x => x.Price).ToList();
                        break;
                }
            }
            //selectd {1 : deal soc ; 2: hang moi ; 3 : (source) hang Brili ; 4: (source) hang nhap khau}
            if (selected.Count() != 0)
            {

                for (int i = 0; i < selected.Count; i++)
                {
                    if (selected[i] == "1")
                    {
                        products = products.Where(x => (DateTime.Now - (DateTime)x.StartDate).TotalDays <= 3).ToList();

                    }
                    if (selected[i] == "2")
                    {
                        products = products.Where(x => db.DiscountProducts.Select(y => y.ProId).Contains(x.ProId)).ToList();

                    }
                    if (selected[i] == "3")
                    {
                        products = products.Where(x => x.Source.Equals("Sản phẩm của Brili Fresh")).ToList();

                    }
                    if (selected[i] == "4")
                    {
                        products = products.Where(x => x.Source.Equals("Hàng Nhập khẩu")).ToList();

                    }
                }

            }

            return products.Count();
        }

        public List<string> getImg(int? ProId) {
            var rs = db.Products.Where(x => x.ProId == ProId && x.IsDeleted == 0).FirstOrDefault();//.Include(x => x.ProductImages).Select(y => y.ProductImages.ImgData).ToList();
            var list_img = db.ProductImages.Where(x => x.ProId == rs.ProId).Select(x => x.ImgData).ToList();
            return list_img as List<string>;

        }

        public IEnumerable<Feedback> getFeedback(int? ProId) {
            var rs = db.Products.Where(x => x.ProId == ProId && x.IsDeleted == 0).FirstOrDefault();
            var fb = db.Feedbacks.Where(x => x.ProId == rs.ProId).ToList();
            return fb;

        }
        public List<string> getImgFB(int? fbID) {

            var fbImg = db.FeedbackImages.Where(x => x.FbId == fbID).Select(x => x.ImgData).ToList();

            return fbImg as List<string>;
        
        }
        public DiscountProduct getDiscount(int? ProId) {
            var rs = db.Products.Where(x => x.ProId == ProId && x.IsDeleted == 0).FirstOrDefault();
            var discount = db.DiscountProducts.Where(x => x.ProId == rs.ProId && x.Status==true).FirstOrDefault();
            return discount;
        }

        public Stock getStock(int? ProId, int? storeID) {


            var rs = db.Products.Where(x => x.ProId == ProId && x.IsDeleted == 0).FirstOrDefault();
            var stock = db.Stocks.Where(x => x.ProId == rs.ProId && x.StoreId==storeID).FirstOrDefault();
            return stock;
        }





        public bool SaveProduct(Product product)
        {
            db.Products.Add(product);
            return db.SaveChanges() > 0;
        }

      
    }
}

