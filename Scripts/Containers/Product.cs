using System;

namespace Ebenit.Containers
{
    /// <summary>
    /// Informations about product.
    /// </summary>
    [Serializable]
    public class Product
    {
        /// <summary>
        /// Product ID in Ebenit API.
        /// </summary>
        public uint pt_id {
            get; protected set;
        }

        /// <summary>
        /// Product name.
        /// </summary>
        public string pt_name {
            get; protected set;
        }

        /// <summary>
        /// Product price (without VAT).
        /// </summary>
        public float pt_price {
            get; protected set;
        }

        /// <summary>
        /// Product Currency.
        /// </summary>
        public Currency pt_currency {
            get; protected set;
        }

        /// <summary>
        /// Product VAT.
        /// </summary>
        public int pt_vat {
            get; protected set;
        }

        /// <summary>
        /// Product price with VAT. If the product is not using VAT, please use the pt_price property.
        /// </summary>
        public float pt_price_vat {
            get; protected set;
        }

        /// <summary>
        /// Product small description.
        /// </summary>
        public string pt_description_small {
            get; protected set;
        }

        /// <summary>
        /// Product description.
        /// </summary>
        public string pt_description {
            get; protected set;
        }

        /// <summary>
        /// Quantity of Units in Product.
        /// </summary>
        public float pt_quantity {
            get; protected set;
        }

        /// <summary>
        /// True if the Product is hidden.
        /// </summary>
        public bool pt_hidden {
            get; protected set;
        }

        /// <summary>
        /// True if the Product is storable.
        /// </summary>
        public bool pt_storable {
            get; protected set;
        }

        /// <summary>
        /// Product Unit.
        /// </summary>
        public Unit pt_unit {
            get; protected set;
        }

        /// <summary>
        /// Product Category.
        /// </summary>
        public Category pt_category {
            get; protected set;
        }
        
        /// <summary>
        /// True if the Product has been, atleast once, bought by current User (ApiManager.pt_user).
        /// </summary>
        public bool pt_bought {
            get; protected set;
        }

        /// <summary>
        /// How many times were the product bought by current User (ApiManager.pt_user).
        /// </summary>
        public float pt_bought_sum {
            get; protected set;
        }

        public Product(uint id, string name) {
            pt_id = id;
            pt_name = name;

            pt_price = 0;
            pt_currency = null;
            pt_vat = 0;
            pt_price_vat = 0;
            pt_description_small = string.Empty;
            pt_description = string.Empty;
            pt_quantity = 0;
            pt_hidden = false;
            pt_storable = false;
            pt_unit = null;
            pt_category = null;
            pt_bought = false;
            pt_bought_sum = 0;
        }

        /// <summary>
        /// Sets price (without VAT).
        /// </summary>
        /// <param name="price"></param>
        public void setPrice(float price) {
            pt_price = price;
        }

        /// <summary>
        /// Sets Currency.
        /// </summary>
        /// <param name="currency"></param>
        public void setCurrency(Currency currency) {
            pt_currency = currency;
        }

        /// <summary>
        /// Sets VAT.
        /// </summary>
        /// <param name="vat"></param>
        public void setVat(int vat) {
            pt_vat = vat;
        }

        /// <summary>
        /// Sets price with VAT.
        /// </summary>
        /// <param name="price_vat"></param>
        public void setPriceVat(float price_vat) {
            pt_price_vat = price_vat;
        }

        /// <summary>
        /// Sets small description.
        /// </summary>
        /// <param name="description_small"></param>
        public void setDescriptionSmall(string description_small) {
            pt_description_small = description_small;
        }

        /// <summary>
        /// Sets description.
        /// </summary>
        /// <param name="description"></param>
        public void setDescription(string description) {
            pt_description = description;
        }

        /// <summary>
        /// Sets quantity of units in product.
        /// </summary>
        /// <param name="quantity"></param>
        public void setQuantity(float quantity) {
            pt_quantity = quantity;
        }

        /// <summary>
        /// Sets the hidden status of product. Defaults to true.
        /// </summary>
        /// <param name="hidden"></param>
        public void setHidden(bool hidden = true) {
            pt_hidden = hidden;
        }

        /// <summary>
        /// Sets the storable status of product. Defaults to true.
        /// </summary>
        /// <param name="storable"></param>
        public void setStorable(bool storable = true) {
            pt_storable = storable;
        }

        /// <summary>
        /// Sets the product Units.
        /// </summary>
        /// <param name="unit"></param>
        public void setUnit(Unit unit) {
            pt_unit = unit;
        }

        /// <summary>
        /// Sets the product Category.
        /// </summary>
        /// <param name="category"></param>
        public void setCategory(Category category) {
            pt_category = category;
        }

        /// <summary>
        /// Sets the product bought status. Default to true.
        /// 
        /// Second parameter is the number of product currently bought. Defaults to 0.
        /// </summary>
        /// <param name="bought"></param>
        /// <param name="sum"></param>
        public void setBought(bool bought = true, float sum = 0) {
            pt_bought = bought;
            pt_bought_sum += sum;
        }

    }
}
