using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Model
{
    public class CommonMessages
    {
        public string Error { get; set; }
        public Organization Organization { get; set; }
        public OrganizationTile OrganizationTile { get; set; }
        public ContentModule ContentModule { get; set; }
        public OrgnaizationPage OrgnaizationPage { get; set; }
        public TileMaster TileMaster { get; set; }
        public JourneyMapper JourneyMapper { get; set; }        
        public Country Country { get; set; }
        public MarketPlace MarketPlace { get; set; }
        public Menu Menu { get; set; }
        public Provider Provider { get; set; }
        public Region Region { get; set; }
        public State State { get; set; }
        public Supplier Supplier { get; set; }
        public SupplierProduction SupplierProduction { get; set; }
        public User User { get; set; }
        public Brand Brand { get; set; }
        public EventType EventType { get; set; }
        public RoleRights RoleRights { get; set; }
        public Currency Currency { get; set; }
        public Role Role { get; set; }
        public Customer Customer { get; set; }
        public ItemCategory ItemCategory { get; set; }
        public Item Item { get; set; }
        public UOM UOM { get; set; }
        public Quote Quote { get; set; }
        public QuoteDetail QuoteDetail { get; set; }
        public ItemCollection ItemCollection { get; set; }
        public ItemCollectionDetail ItemCollectionDetail { get; set; }
        public Tag Tag { get; set; }
        public SettingOptions SettingOptions { get; set; }

        public LeadTime LeadTime { get; set; }

        public LeadTimeStatus LeadTimeStatus { get; set; }

        public LeadTimeStatusDetails LeadTimeStatusDetails { get; set; }
        public PaymentTermsDetails PaymentTermsDetails { get; set; }
        public Product Product { get; set; }
        public ShopifyProduct ShopifyProduct { get; set; }

        public ShopifyOrders ShopifyOrders { get; set; }

        public ServiceType ServiceType { get; set; }
        public Enquiry Enquiry { get; set; }
        public EnquiryDetail EnquiryDetail { get; set; }
        public string CreateCommonMessage(string strmethod, string strData)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine(strmethod);
            s.AppendLine("ERROR");
            s.AppendLine(strData);
            return s.ToString();
        }

    }
    public class Organization : Messages{}
    public class ContentModule : Messages { }
    public class OrgnaizationPage : Messages { }
    public class OrganizationTile : Messages { }
    public class TileMaster : Messages { }
    public class JourneyMapper : Messages{}
    public class Country : Messages{}
    public class MarketPlace : Messages{}
    public class Menu : Messages{}
    public class Provider : Messages{}
    public class Region : Messages{}
    public class State : Messages{}
    public class Supplier : Messages{}
    public class SupplierProduction : Messages{}
    public class User : Messages{}
    public class Brand : Messages{}
    public class EventType : Messages { }
    public class RoleRights : Messages { }
    public class Currency : Messages { }
    public class SettingOptions : Messages { }
    public class Role : Messages { }
    public class ItemCategory : Messages { }
    public class Item : Messages { }
    public class UOM : Messages { }
    public class Quote : Messages { };
    public class QuoteDetail : Messages { };
    public class ItemCollection : Messages { }
    public class ItemCollectionDetail : Messages { }
    public class Tag : Messages { }
    public class LeadTimeStatus : Messages { }
    public class LeadTime : Messages { }
    public class Customer : Messages { }

    public class LeadTimeStatusDetails : Messages { }
    public class PaymentTermsDetails : Messages { }
    public class Product : Messages { }
    public class ShopifyProduct : Messages { }
    public class ShopifyOrders : Messages { }



    public class ServiceType : Messages { }
    public class Enquiry : Messages { }
    public class EnquiryDetail : Messages { }
    public class Messages
    {
        public string SaveSuccess { get; set; }
        public string SaveError { get; set; }
        public string DeleteSuccess { get; set; }
        public string DeleteError { get; set; }
        public string ExcelSaveError { get; set; }
        public string AlreadyExists { get; set; }

    }
    
}
