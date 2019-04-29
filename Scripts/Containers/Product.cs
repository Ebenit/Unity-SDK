using System;

namespace Ebenit.Containers
{
    [Serializable]
    public class Product
    {
        public uint pt_id {
            get; protected set;
        }

        public string pt_name {
            get; protected set;
        }

        public float pt_price {
            get; protected set;
        }

        public Currency pt_currency {
            get; protected set;
        }

        public int pt_vat {
            get; protected set;
        }

        public float pt_price_vat {
            get; protected set;
        }

        public string pt_description_small {
            get; protected set;
        }

        public string pt_description {
            get; protected set;
        }

        public float pt_quantity {
            get; protected set;
        }

        public bool pt_hidden {
            get; protected set;
        }

        public bool pt_storable {
            get; protected set;
        }

        public Unit pt_unit {
            get; protected set;
        }

        public Category pt_category {
            get; protected set;
        }
        
        public bool pt_bought {
            get; protected set;
        }

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

        public void setPrice(float price) {
            pt_price = price;
        }

        public void setCurrency(Currency currency) {
            pt_currency = currency;
        }

        public void setVat(int vat) {
            pt_vat = vat;
        }

        public void setPriceVat(float price_vat) {
            pt_price_vat = price_vat;
        }

        public void setDescriptionSmall(string description_small) {
            pt_description_small = description_small;
        }

        public void setDescription(string description) {
            pt_description = description;
        }

        public void setQuantity(float quantity) {
            pt_quantity = quantity;
        }

        public void setHidden(bool hidden = true) {
            pt_hidden = hidden;
        }

        public void setStorable(bool storable = true) {
            pt_storable = storable;
        }

        public void setUnit(Unit unit) {
            pt_unit = unit;
        }

        public void setCategory(Category category) {
            pt_category = category;
        }

        public void setBought(bool bought = true, float sum = 0) {
            pt_bought = bought;
            pt_bought_sum += sum;
        }

    }
}
