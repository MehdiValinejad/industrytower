using IndustryTower.DAL;
using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.Helpers
{
    public class UserDynamicNodeProvider : DynamicNodeProviderBase
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {

            var reader = unitOfWork.ReaderRepository.GetSPDataReader("SiteMap");
            //USers
            while (reader.Read())
            {
                DynamicNode dynamicNode = new DynamicNode("User_" + reader.GetInt32(0), reader[1] as string);
                // Preserve our route parameter explicitly
                dynamicNode.RouteValues.Add("UId", reader.GetInt32(0));
                dynamicNode.RouteValues.Add("UName", StringHelper.URLName(String.Concat(reader[1] as string," ",reader[2] as string)));
                dynamicNode.Controller = "UserProfile";
                dynamicNode.Action = "UProfile";
                yield return dynamicNode;
            }
            reader.NextResult();

            //Company
            while (reader.Read())
            {
                DynamicNode dynamicNode = new DynamicNode("Company_" + reader.GetInt32(0), reader[1] as string);
                // Preserve our route parameter explicitly
                dynamicNode.RouteValues.Add("CoId", reader.GetInt32(0));
                dynamicNode.RouteValues.Add("CoName", StringHelper.URLName(reader[1] as string));
                dynamicNode.Controller = "Company";
                dynamicNode.Action = "CProfile";
                yield return dynamicNode;
            }
            reader.NextResult();

            //Store
            while (reader.Read())
            {
                DynamicNode dynamicNode = new DynamicNode("Store_" + reader.GetInt32(0), reader[1] as string);
                // Preserve our route parameter explicitly
                dynamicNode.RouteValues.Add("StId", reader.GetInt32(0));
                dynamicNode.RouteValues.Add("StName", StringHelper.URLName(reader[1] as string));
                dynamicNode.Controller = "Store";
                dynamicNode.Action = "SProfile";
                yield return dynamicNode;
            }
            reader.NextResult();

            //Product
            while (reader.Read())
            {
                DynamicNode dynamicNode = new DynamicNode("Product_" + reader.GetInt32(0), reader[1] as string);
                // Preserve our route parameter explicitly
                dynamicNode.RouteValues.Add("PrId", reader.GetInt32(0));
                dynamicNode.RouteValues.Add("PrName", StringHelper.URLName(reader[1] as string));
                dynamicNode.Controller = "Product";
                dynamicNode.Action = "Detail";
                yield return dynamicNode;
            }
            reader.NextResult();

            //Service
            while (reader.Read())
            {
                DynamicNode dynamicNode = new DynamicNode("Service_" + reader.GetInt32(0), reader[1] as string);
                // Preserve our route parameter explicitly
                dynamicNode.RouteValues.Add("SrId", reader.GetInt32(0));
                dynamicNode.RouteValues.Add("SrName", StringHelper.URLName(reader[1] as string));
                dynamicNode.Controller = "Service";
                dynamicNode.Action = "Detail";
                yield return dynamicNode;
            }
            reader.NextResult();

            //Question
            while (reader.Read())
            {
                DynamicNode dynamicNode = new DynamicNode("Question_" + reader.GetInt32(0), reader[1] as string);
                // Preserve our route parameter explicitly
                dynamicNode.RouteValues.Add("QId", reader.GetInt32(0));
                dynamicNode.RouteValues.Add("QName", StringHelper.URLName(reader[1] as string));
                dynamicNode.Controller = "Question";
                dynamicNode.Action = "Detail";
                dynamicNode.LastModifiedDate = reader.GetDateTime(2);
                yield return dynamicNode;
            }
            reader.NextResult();

            //Groups
            while (reader.Read())
            {
                DynamicNode dynamicNode = new DynamicNode("Group_" + reader.GetInt32(0), reader[1] as string);
                // Preserve our route parameter explicitly
                dynamicNode.RouteValues.Add("GId", reader.GetInt32(0));
                dynamicNode.RouteValues.Add("GName", StringHelper.URLName(reader[1] as string));
                dynamicNode.Controller = "Group";
                dynamicNode.Action = "GroupPage";
                dynamicNode.LastModifiedDate = reader.GetDateTime(2);
                yield return dynamicNode;
            }
            reader.NextResult();

            //GroupSessions
            while (reader.Read())
            {
                DynamicNode dynamicNode = new DynamicNode("GroupSession_" + reader.GetInt32(0), reader[1] as string);
                // Preserve our route parameter explicitly
                dynamicNode.RouteValues.Add("SsId", reader.GetInt32(0));
                dynamicNode.RouteValues.Add("GSName", StringHelper.URLName(reader[1] as string));
                dynamicNode.Controller = "GroupSession";
                dynamicNode.Action = "Detail";
                dynamicNode.LastModifiedDate = reader.GetDateTime(2);
                yield return dynamicNode;
            }
            reader.NextResult();

            //Seminars
            while (reader.Read())
            {
                DynamicNode dynamicNode = new DynamicNode("Seminar_" + reader.GetInt32(0), reader[1] as string);
                // Preserve our route parameter explicitly
                dynamicNode.RouteValues.Add("SnId", reader.GetInt32(0));
                dynamicNode.RouteValues.Add("SnName", StringHelper.URLName(reader[1] as string));
                dynamicNode.Controller = "Seminar";
                dynamicNode.Action = "Detail";
                dynamicNode.LastModifiedDate = reader.GetDateTime(2);
                yield return dynamicNode;
            }
            reader.NextResult();

            while (reader.Read())
            {
                DynamicNode dynamicNode = new DynamicNode("Book_" + reader.GetInt32(0), reader[1] as string);
                // Preserve our route parameter explicitly
                dynamicNode.RouteValues.Add("BId", reader.GetInt32(0));
                dynamicNode.RouteValues.Add("BName", StringHelper.URLName(reader[1] as string));
                dynamicNode.Controller = "Book";
                dynamicNode.Action = "Detail";
                dynamicNode.LastModifiedDate = reader.GetDateTime(2);
                yield return dynamicNode;
            }
            reader.Close();
        }
    }
}