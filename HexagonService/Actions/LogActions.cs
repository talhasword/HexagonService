using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using NHibernate;
using NHibernate.Criterion;

namespace DigitalCustomer.Actions
{
    public class LogActions
    {
        #region declare
        private ISessionFactory SessionFactory { get; set; }
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private LogActions()
        {
            SessionFactory = HexagonService.Config.FluentlyConfig.Instance.CreateSessionFactory();
        }
        public static LogActions Instance
        {
            get
            {
                return LogActionsFactory.instance;
            }
        }
        private class LogActionsFactory
        {
            static LogActionsFactory() { }
            internal static readonly LogActions instance = new LogActions();
        }
        #endregion
        /*
        public Customer GetCustomer(string citizenshipNo, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<Customer>("cus")
                    .Add(Restrictions.Eq("cus.CitizenshipNumber", citizenshipNo))
                    .List<Customer>();

                return resp.SingleOrDefault();
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["customerinfoid"] = 0;
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                logger.Fatal("Customer tablosu veri alma hatası", ex);
                throw;
            }
        }

        public Customer GetCustomer(string citizenshipNo)
        {
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))
            {
                try
                {
                    var resp = uow.Session.CreateCriteria<Customer>("cus")
                        .Add(Restrictions.Eq("cus.CitizenshipNumber", citizenshipNo))
                        .List<Customer>();

                    uow.Commit();
                    return resp.SingleOrDefault();
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["customerinfoid"] = 0;
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    logger.Fatal("Customer tablosu veri alma hatası", ex);
                    throw;
                }
            }
        }

        public bool UpdateCustomer(string citizenshipNo, string externalClientNo, bool isCustomer, string recordStatus, UnitOfWork uow)
        {
            Customer customer;
            try
            {
                customer = GetCustomer(citizenshipNo, uow);
                if (customer == null)
                {
                    customer = new Customer();
                    customer.IsBlocked = false;
                    customer.CitizenshipNumber = citizenshipNo;
                    customer.BlockedTime = DateTime.Now;
                    customer.RegisterTime = DateTime.Now;
                    customer.RecordStatus = StatusType.A.ToString();
                    if (isCustomer)
                    {
                        customer.IsCustomer = true;
                        customer.CustomerNumber = externalClientNo;
                    }
                    else
                    {
                        customer.IsCustomer = false;
                        customer.ContactNumber = externalClientNo;
                    }
                }
                else
                {
                    if (isCustomer)
                    {
                        customer.IsCustomer = true;
                        customer.CustomerNumber = externalClientNo;
                    }
                    else
                    {
                        customer.IsCustomer = false;
                        customer.ContactNumber = externalClientNo;
                    }
                    customer.RecordStatus = recordStatus;
                }
                customer.UpdateTime = DateTime.Now;
                uow.Session.Save(customer);
                return true;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["customerinfoid"] = 0;
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                logger.Fatal("Customer tablosu kayıt/güncelleme hatası", ex);
                return false;
            }
        }

        public bool UpdateCustomer1(string citizenshipNo, string externalClientNo, bool? isCustomer, string recordStatus, UnitOfWork uow)
        {
            Customer customer;
            try
            {
                customer = GetCustomer(citizenshipNo, uow);
                if (customer == null)
                {
                    customer = new Customer();
                    customer.IsBlocked = false;
                    customer.CitizenshipNumber = citizenshipNo;
                    customer.BlockedTime = DateTime.Now;
                    customer.RegisterTime = DateTime.Now;
                    customer.RecordStatus = StatusType.A.ToString();
                    if (isCustomer.HasValue && isCustomer.Value)
                    {
                        customer.IsCustomer = true;
                        customer.ContactNumber = null;
                        customer.CustomerNumber = externalClientNo;
                    }
                    else
                    {
                        customer.IsCustomer = false;
                        customer.CustomerNumber = null;
                        customer.ContactNumber = externalClientNo;
                    }
                }
                else
                {
                    if (isCustomer.HasValue)
                    {
                        customer.IsCustomer = isCustomer.Value;
                        if (isCustomer.Value)
                            customer.CustomerNumber = externalClientNo;
                        else
                        {
                            customer.CustomerNumber = null;
                            customer.ContactNumber = externalClientNo;
                        }
                    }
                    customer.RecordStatus = recordStatus;
                }
                customer.UpdateTime = DateTime.Now;
                uow.Session.Save(customer);
                return true;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["customerinfoid"] = 0;
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                logger.Fatal("Customer tablosu kayıt/güncelleme hatası", ex);
                return false;
            }
        }

        public bool RegisterCustomer(string citizenshipNo, string externalClientNo, bool isCustomer, string recordStatus)
        {
            Customer customer;
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))
            {
                try
                {
                    customer = GetCustomer(citizenshipNo, uow);
                    if (customer == null)
                    {
                        customer = new Customer();
                        customer.IsBlocked = false;
                        customer.CitizenshipNumber = citizenshipNo;
                        customer.UpdateTime = DateTime.Now;
                        customer.BlockedTime = DateTime.Now;
                        customer.RegisterTime = DateTime.Now;
                        customer.RecordStatus = StatusType.A.ToString();
                        if (isCustomer)
                        {
                            //customer.ContactNumber = null;
                            customer.IsCustomer = true;
                            customer.CustomerNumber = externalClientNo;
                        }
                        else
                        {
                            //customer.CustomerNumber = null;
                            customer.IsCustomer = false;
                            customer.ContactNumber = externalClientNo;
                        }
                    }
                    else
                    {
                        if (isCustomer)
                        {
                            //customer.ContactNumber = null;
                            customer.IsCustomer = true;
                            customer.CustomerNumber = externalClientNo;
                        }
                        else
                        {
                            //customer.ContactNumber = null;
                            customer.IsCustomer = false;
                            customer.ContactNumber = externalClientNo;
                        }

                        if (!string.IsNullOrEmpty(recordStatus))
                            customer.RecordStatus = recordStatus;
                        customer.UpdateTime = DateTime.Now;
                    }
                    uow.Session.Save(customer);
                    uow.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["customerinfoid"] = 0;
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    logger.Fatal("CustomerProducts veri kayıt hatası", ex);
                    return false;
                }
            }
        }

        public bool RegisterCustomer1(string citizenshipNo, string externalClientNo, bool? isCustomer, string recordStatus)
        {
            Customer customer;
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))
            {
                try
                {
                    customer = GetCustomer(citizenshipNo, uow);
                    if (customer == null)
                    {
                        customer = new Customer();
                        customer.IsBlocked = false;
                        customer.CitizenshipNumber = citizenshipNo;
                        customer.UpdateTime = DateTime.Now;
                        customer.BlockedTime = DateTime.Now;
                        customer.RegisterTime = DateTime.Now;
                        customer.RecordStatus = StatusType.A.ToString();
                        if (isCustomer.HasValue && isCustomer.Value)
                        {
                            customer.IsCustomer = true;
                            customer.ContactNumber = null;
                            customer.CustomerNumber = externalClientNo;
                        }
                        else
                        {
                            customer.IsCustomer = false;
                            customer.CustomerNumber = null;
                            customer.ContactNumber = externalClientNo;
                        }
                    }
                    else
                    {
                        if (isCustomer.HasValue)
                        {
                            customer.IsCustomer = isCustomer;
                            //if (!string.IsNullOrEmpty(externalClientNo))
                            //{
                            if (isCustomer.Value)
                                customer.CustomerNumber = externalClientNo;
                            else
                            {
                                //customer.CustomerNumber = null;
                                customer.ContactNumber = externalClientNo;
                            }
                            //  }
                        }
                        //else
                        //{
                        //    customer.ContactNumber = null;
                        //    customer.CustomerNumber = null;
                        //}

                        if (!string.IsNullOrEmpty(recordStatus))
                            customer.RecordStatus = recordStatus;
                        customer.UpdateTime = DateTime.Now;
                    }
                    uow.Session.Save(customer);
                    uow.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["customerinfoid"] = 0;
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    logger.Fatal("CustomerProducts veri kayıt hatası", ex);
                    return false;
                }
            }
        }

        public Products GetProductByName(string product, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<Products>("p")
                    .Add(Restrictions.Eq("p.RecordStatus", "A"))
                    .Add(Restrictions.Eq("p.Product", product.ToUpper()))
                    .List<Products>();

                return resp.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.Fatal("Product Tablosundan Veri Alınırken Hata;" + product + "", ex);
                return null;
            }
        }

        public Products GetProductByNameUI(string product)
        {
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))//
            {
                try
                {
                    var resp = uow.Session.CreateCriteria<Products>("p")
                        .Add(Restrictions.Eq("p.RecordStatus", "A"))
                        .Add(Restrictions.Eq("p.Product", product.ToUpper()))
                        .List<Products>();

                    uow.Commit();
                    return resp.SingleOrDefault();
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    logger.Fatal("Product(" + product + ") tablosu veri alma hatası", ex);
                    return null;
                }
            }
        }

        public Products GetProductById(int productId, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<Products>("p")
                    .Add(Restrictions.Eq("p.RecordStatus", "A"))
                    .Add(Restrictions.Eq("p.Id", productId))
                    .List<Products>();

                return resp.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.Fatal("Product Tablosunda Veri Alınırken Hata;" + productId.ToString() + "", ex);
                return null;
            }
        }

        public CustomerInfo GetCustomerInfoOnlyCitizenshipNo(string citizenshipNo, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<CustomerInfo>("ci")
                    .CreateCriteria("ci.Customer", "c")
                    .Add(Restrictions.Eq("c.CitizenshipNumber", citizenshipNo))
                    .Add(Restrictions.Eq("ci.RecordStatus", "A"))
                    .List<CustomerInfo>();

                return resp.SingleOrDefault();
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                log4net.GlobalContext.Properties["customerinfoid"] = 0;
                logger.Fatal("CustomerInfo Tablosundan Kayıt Çekerken Hata", ex);
                //return null;
                throw;
            }
        }
        
        public CustomerInfo GetCustomerInfoByCitizenship(string citizenshipNo, bool isrecordFinish, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<CustomerInfo>("ci")
                    .CreateCriteria("ci.Customer", "c")
                    .Add(Restrictions.Eq("c.CitizenshipNumber", citizenshipNo))
                    .Add(Restrictions.Eq("ci.RecordStatus", "A"))
                    .Add(Restrictions.Eq("ci.IsRecordFinish", isrecordFinish))
                    .List<CustomerInfo>();

                //return resp.SingleOrDefault();
                return resp.LastOrDefault();
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["customerinfoid"] = 0;
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                logger.Fatal("CustomerInfo Tablosundan Kayıt Çekerken Hata", ex);
                throw;
            }
        }
        
        public CustomerInfo GetCustomerInfoByCitizenshipNoUI(string citizenshipNo, bool isrecordFinish)
        {
            CustomerInfo customerInfo;
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))//
            {
                try
                {
                    var resp = uow.Session.CreateCriteria<CustomerInfo>("ci")
                        .CreateCriteria("ci.Customer", "c")
                        .Add(Restrictions.Eq("c.CitizenshipNumber", citizenshipNo))
                        .Add(Restrictions.Eq("ci.RecordStatus", "A"))
                        .Add(Restrictions.Eq("ci.IsRecordFinish", isrecordFinish))
                        .List<CustomerInfo>();

                    if (resp != null && resp.Count > 0)
                        customerInfo = resp.Last();
                    else
                        customerInfo = null;

                    uow.Commit();
                    return customerInfo;
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    log4net.GlobalContext.Properties["customerinfoid"] = 0;
                    logger.Fatal("CustomerInfo Tablosundan Kayıt Çekerken Hata", ex);
                    throw;
                }
            }
        }
        
        public CustomerInfo GetCustomerInfoById(string citizenshipNo, int customerInfoId, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<CustomerInfo>("ci")
                    .CreateCriteria("ci.Customer", "c")
                    .Add(Restrictions.Eq("c.CitizenshipNumber", citizenshipNo))
                    .Add(Restrictions.Eq("ci.Id", customerInfoId))
                    .List<CustomerInfo>();

                return resp.SingleOrDefault();
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                log4net.GlobalContext.Properties["customerinfoid"] = customerInfoId;
                logger.Fatal("CustomerInfo Tablosundan Kayıt Çekerken Hata", ex);
                return null;
            }
        }

        public CustomerInfo GetCustomerInfoById(int customerInfoId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))
            {
                try
                {
                    var resp = uow.Session.CreateCriteria<CustomerInfo>("ci")
                     .Add(Restrictions.Eq("ci.Id", customerInfoId))
                     .List<CustomerInfo>();

                    uow.Commit();
                    return resp.SingleOrDefault();
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["citizenshipnumber"] = "SYSYTEM";
                    log4net.GlobalContext.Properties["customerinfoid"] = customerInfoId;
                    logger.Fatal("CustomerInfo Tablosundan Kayıt Çekerken Hata", ex);
                    throw;
                }
            }
        }

        public CustomerInfo GetCustomerInfoById(int customerInfoId, UnitOfWork uow)
        {

            try
            {
                var resp = uow.Session.CreateCriteria<CustomerInfo>("ci")
                 .CreateCriteria("ci.Customer", "c")
                 .Add(Restrictions.Eq("ci.Id", customerInfoId))
                 .List<CustomerInfo>();

                return resp.SingleOrDefault();
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["customerinfoid"] = customerInfoId;
                log4net.GlobalContext.Properties["citizenshipnumber"] = "SYSTEM";
                logger.Fatal("CustomerInfo Tablosundan Kayıt Çekerken Hata", ex);
                return null;
            }
        }

        public CustomerInfo GetCustomerInfoByReferenceNo(string citizenshipNo, string referenceNo, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<CustomerInfo>("ci")
                    .CreateCriteria("ci.Customer", "c")
                    .Add(Restrictions.Eq("c.CitizenshipNumber", citizenshipNo))
                    .Add(Restrictions.Eq("ci.ReferenceNumber", referenceNo))
                    .List<CustomerInfo>();

                return resp.SingleOrDefault();
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                log4net.GlobalContext.Properties["customerinfoid"] = 0;
                logger.Fatal("CustomerInfo Tablosundan " + referenceNo + " Referans Nolu Kayıt Çekerken Hata", ex);
                return null;
            }
        }
        
        public IList<CustomerInfo> GetCustomerInfoList(bool isrecordFinish, string recordStatus, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<CustomerInfo>("ci")
                    .CreateCriteria("ci.Customer", "c")
                    .Add(Restrictions.Eq("ci.RecordStatus", recordStatus))
                    .Add(Restrictions.Eq("ci.IsRecordFinish", isrecordFinish))
                    .Add(Restrictions.Lt("ci.UpdateTime", DateTime.Now.AddHours(-1)))
                    .List<CustomerInfo>();

                return resp;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["customerinfoid"] = 0;
                log4net.GlobalContext.Properties["citizenshipnumber"] = "SYSTEM";
                logger.Fatal("CustomerInfo Tablosundan Kayıt Çekerken Hata", ex);
                throw;
            }
        }

        public int RegisterCustomerInfoUI(string citizenshipNo, string customerData, bool recordState, string recordPage, string recordStatus, string masterProduct, string customerPhone, string applyChannel, string applybyUser, string ipaddress, string applyCampaignCode)
        {
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))//
            {
                try
                {
                    var channel = Intertech.Common.ParameterCache.SubesizWebMkChannelParameters.SubesizWebMkChannels[applyChannel];

                    Customer c = GetCustomer(citizenshipNo, uow);
                    if (c == null)
                        return -1;

                    Products product = GetProductByName(masterProduct, uow);
                    if (product == null)
                        throw new Exception("Product(" + masterProduct + ") bilgisi alınamadı");

                    CustomerInfo ci = new CustomerInfo();
                    ci.MasterProduct = product.Id;
                    ci.Customer = c;
                    ci.CustomerData = customerData;
                    ci.RecordPage = recordPage;
                    ci.IsRecordFinish = false;
                    ci.RecordStatus = recordStatus;
                    ci.CustomerPhone = customerPhone;
                    ci.ReferenceNumber = GenerateNumericKey();
                    ci.ApplyByUser = applybyUser;
                    ci.CampaignCode = applyCampaignCode;
                    ci.Channel = applyChannel;
                    ci.IPAddress = ipaddress;
                    ci.IsApplySuccessful = null;
                    ci.RegisterTime = DateTime.Now;
                    ci.UpdateTime = DateTime.Now;
                    uow.Session.Save(ci);
                    uow.Commit();//
                    return ci.Id;
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["customerinfoid"] = 0;
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    logger.Fatal("CustomerInfo tablosu kayıt hatası,CHANNEL(" + applyChannel + ")", ex);
                    return -1;
                }
            }
        }

        public bool UpdateCustomerInfoUI(string citizenshipNo, string customerData, bool? isrecordFinish, string recordPage, string recordStatus, string mobilePhone, int customerInfoId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))//
            {
                try
                {
                    CustomerInfo cusInfo = GetCustomerInfoById(citizenshipNo, customerInfoId, uow);
                    if (cusInfo == null)
                    {
                        logger.Fatal("CustomerInfo bilgisi bulunamadı");
                        return false;
                    }
                    if (isrecordFinish != null)
                        cusInfo.IsRecordFinish = isrecordFinish;
                    if (!string.IsNullOrEmpty(recordPage))
                        cusInfo.RecordPage = recordPage;
                    if (!string.IsNullOrEmpty(customerData))
                        cusInfo.CustomerData = customerData;
                    if (!string.IsNullOrEmpty(recordStatus))
                        cusInfo.RecordStatus = recordStatus;
                    if (!string.IsNullOrEmpty(mobilePhone))
                        cusInfo.CustomerPhone = mobilePhone;
                    cusInfo.UpdateTime = DateTime.Now;
                    uow.Session.Save(cusInfo);
                    uow.Commit();//
                    return true;
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["customerinfoid"] = customerInfoId;
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    logger.Fatal("CustomerInfo tablosunda güncelleme hatası", ex);
                    return false;
                }
            }
        }

        public bool UpdateCustomerToApplySuccessful(CustomerInfo customerInfo, bool? isApplySuccessful, string recordStatus, UnitOfWork uow)
        {
            try
            {
                if (!string.IsNullOrEmpty(recordStatus))
                    customerInfo.RecordStatus = recordStatus;
                if (isApplySuccessful != null)
                    customerInfo.IsApplySuccessful = isApplySuccessful;
                customerInfo.UpdateTime = DateTime.Now;
                uow.Session.Save(customerInfo);
                return true;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["customerinfoid"] = customerInfo.Id;
                log4net.GlobalContext.Properties["citizenshipnumber"] = customerInfo.Customer.CitizenshipNumber;
                logger.Fatal("CustomerInfo Tablosunda Güncelleme Yaparken Hata", ex);
                return false;
            }
        }

        public IList<CustomerProduct> GetCustomerProducts(string citizenshipNo, int customerInfoId, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<CustomerProduct>("cp")
                    .Add(Restrictions.Eq("cp.CustomerInfoId", customerInfoId))
                    .List<CustomerProduct>();

                //if (resp != null && resp.Count > 0)
                return resp;
                //else
                //    return new List<CustomerProduct>();
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                log4net.GlobalContext.Properties["customerinfoid"] = customerInfoId;
                logger.Fatal("CustomerProducts Tablosundan Kayıt Çekilirken Hata", ex);
                //return null;
                throw;
            }
        }

        public CustomerProduct GetCustomerProduct(string citizenshipNo, int customerInfoId, string product, UnitOfWork uow)
        {
            try
            {
                var resp = uow.Session.CreateCriteria<CustomerProduct>("cp")
                    .CreateCriteria("cp.Product", "p")
                   .Add(Restrictions.Eq("cp.CustomerInfoId", customerInfoId))
                   .Add(Restrictions.Eq("p.Product", product))
                   .List<CustomerProduct>();

                return resp.SingleOrDefault();
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                log4net.GlobalContext.Properties["customerinfoid"] = customerInfoId;
                logger.Fatal("CustomerProducts Tablosundan Kayıt Çekilirken Hata", ex);
                throw;
            }
        }

        public CustomerProduct GetCustomerProductUI(string citizenshipNo, int customerInfoId, string product)
        {

            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))//
            {
                try
                {
                    var resp = uow.Session.CreateCriteria<CustomerProduct>("cp")
                    .CreateCriteria("cp.Product", "p")
                    .Add(Restrictions.Eq("cp.CustomerInfoId", customerInfoId))
                    .Add(Restrictions.Eq("p.Product", product))
                    .List<CustomerProduct>();

                    uow.Commit();
                    return resp.SingleOrDefault();
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    log4net.GlobalContext.Properties["customerinfoid"] = customerInfoId;
                    logger.Fatal("CustomerProducts Tablosundan Kayıt Çekilirken Hata", ex);
                    throw;
                }
            }
        }

        public CustomerProduct GetCustomerProductByReferenceNo(string citizenshipNo, string referenceNo, string product, UnitOfWork uow)
        {
            try
            {
                CustomerInfo customerInfo = GetCustomerInfoByReferenceNo(citizenshipNo, referenceNo, uow);
                if (customerInfo == null)
                    throw new Exception("ReferansNo(" + referenceNo + ") ile CustomerInfo Bulunamadı");

                var resp = uow.Session.CreateCriteria<CustomerProduct>("cp")
                    .CreateCriteria("cp.Product", "p")
                    .Add(Restrictions.Eq("p.Product", product))
                   .Add(Restrictions.Eq("cp.CustomerInfoId", customerInfo.Id))
                   .List<CustomerProduct>();

                return resp.SingleOrDefault();
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                log4net.GlobalContext.Properties["customerinfoid"] = 0;
                logger.Fatal("CustomerProducts Tablosundan Kayıt Çekilirken Hata", ex);
                return null;
            }
        }

        public bool RegisterCustomerProduct(string citizenshipNo, string product, int statu, string extraDocument, int creditAmount, int paymentMaturity, int customerInfoId, string recordStatus, string workFlowId, UnitOfWork uow)
        {
            try
            {
                CustomerProduct cusProduct;
                Products getProduct = GetProductByName(product, uow);
                var flowState = Intertech.Common.ParameterCache.SubesizWebMkStatusParameters.SubesizWebMkStatus[statu];

                //if (flowState == null)
                //    throw new Exception("Akış Statusü(" + statu + ") Bulunamadı");
                if (getProduct == null)
                    throw new Exception("CustomerProduct(" + product + ") Bulunamadı");

                cusProduct = GetCustomerProduct(citizenshipNo, customerInfoId, product, uow);
                if (cusProduct != null)
                {
                    if (!string.IsNullOrEmpty(workFlowId))
                        cusProduct.WorkFlowId = workFlowId;
                    if (!string.IsNullOrEmpty(recordStatus))
                        cusProduct.RecordStatus = recordStatus;
                    if (!string.IsNullOrEmpty(extraDocument))
                        cusProduct.ExtraDocument = extraDocument;
                    if (statu != 0)
                    {
                        cusProduct.PreviousStatu = cusProduct.Statu;
                        cusProduct.Statu = statu;
                    }
                }
                else
                {
                    cusProduct = new CustomerProduct();
                    cusProduct.Product = getProduct;
                    cusProduct.WorkFlowId = workFlowId;
                    cusProduct.CustomerInfoId = customerInfoId;
                    cusProduct.RecordStatus = recordStatus;
                    cusProduct.ExtraDocument = extraDocument;
                    cusProduct.RegisterTime = DateTime.Now;
                    cusProduct.PreviousStatu = statu;
                    cusProduct.Statu = statu;
                    cusProduct.LastModifiedChannel = "SUBESIZ";
                    cusProduct.LastModifiedBy = "SYSTEM";
                }
                if (creditAmount != 0)
                    cusProduct.CreditAmount = creditAmount;
                if (paymentMaturity != 0)
                    cusProduct.PaymentMaturity = paymentMaturity;
                cusProduct.UpdateTime = DateTime.Now;
                uow.Session.Save(cusProduct);
                return true;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                log4net.GlobalContext.Properties["customerinfoid"] = customerInfoId;
                logger.Fatal("CustomerProducts Tablosuna Kayıt İşlemi Yapılırken Hata", ex);
                return false;
            }
        }

        public bool RegisterCustomerProductUI(string citizenshipNo, string product, int statu, string extraDocument, int creditAmount, int paymentMaturity, int customerInfoId, string recordStatus, string workFlowId)
        {
            CustomerProduct cusProduct;
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))//
            {
                try
                {
                    Products getProduct = GetProductByName(product, uow);
                    var flow = Intertech.Common.ParameterCache.SubesizWebMkStatusParameters.SubesizWebMkStatus[statu];
                    if (getProduct == null)
                        throw new Exception("CustomerProduct(" + product + ") bulunamadı");

                    cusProduct = GetCustomerProduct(citizenshipNo, customerInfoId, product, uow);
                    if (cusProduct != null)
                    {
                        if (!string.IsNullOrEmpty(workFlowId))
                            cusProduct.WorkFlowId = workFlowId;
                        if (!string.IsNullOrEmpty(recordStatus))
                            cusProduct.RecordStatus = recordStatus;
                        if (!string.IsNullOrEmpty(extraDocument))
                            cusProduct.ExtraDocument = extraDocument;
                        if (statu != 0)
                        {
                            cusProduct.PreviousStatu = cusProduct.Statu;
                            cusProduct.Statu = statu;
                        }
                    }
                    else
                    {
                        cusProduct = new CustomerProduct();
                        cusProduct.Product = getProduct;
                        cusProduct.WorkFlowId = workFlowId;
                        cusProduct.CustomerInfoId = customerInfoId;
                        cusProduct.RecordStatus = recordStatus;
                        cusProduct.ExtraDocument = extraDocument;
                        cusProduct.RegisterTime = DateTime.Now;
                        cusProduct.PreviousStatu = statu;
                        cusProduct.Statu = statu;
                        cusProduct.LastModifiedBy = "SYSTEM";
                        cusProduct.LastModifiedChannel = "SUBESIZ";
                    }
                    if (creditAmount != 0)
                        cusProduct.CreditAmount = creditAmount;
                    if (paymentMaturity != 0)
                        cusProduct.PaymentMaturity = paymentMaturity;

                    cusProduct.UpdateTime = DateTime.Now;
                    uow.Session.Save(cusProduct);
                    uow.Commit();//
                    return true;
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["customerinfoid"] = customerInfoId;
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    logger.Fatal("CustomerProducts tablosuna kayıt işleme hatası", ex);
                    return false;
                }
            }
        }

        public bool UpdateCustomerProductUI(string citizenshipNo, string workFlowId, int statu, string product, string lastModifiedBy, string lastModifiedChannel, string recordStatus, int customerInfoId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))//
            {
                try
                {
                    var flow = Intertech.Common.ParameterCache.SubesizWebMkStatusParameters.SubesizWebMkStatus[statu];
                    var cusProduct = GetCustomerProduct(citizenshipNo, customerInfoId, product, uow);
                    if (cusProduct == null)
                        throw new Exception("CustomerProduct(" + product + ") bulunamadı");

                    cusProduct.UpdateTime = DateTime.Now;
                    cusProduct.RecordStatus = recordStatus;
                    if (!string.IsNullOrEmpty(workFlowId))
                        cusProduct.WorkFlowId = workFlowId;
                    if (!string.IsNullOrEmpty(lastModifiedBy))
                        cusProduct.LastModifiedBy = lastModifiedBy;
                    if (!string.IsNullOrEmpty(lastModifiedChannel))
                        cusProduct.LastModifiedChannel = lastModifiedChannel;
                    if (statu != 0)
                    {
                        cusProduct.PreviousStatu = cusProduct.Statu;
                        cusProduct.Statu = statu;
                    }
                    uow.Session.Save(cusProduct);
                    uow.Commit();//
                    return true;
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["customerinfoid"] = customerInfoId;
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    logger.Fatal("CustomerProducts tablosu güncelleme hatası", ex);
                    return false;
                }
            }
        }

        public bool UpdateCustomerProduct(string citizenshipNo, string workFlowId, int statu, string product, string lastModifiedBy, string lastModifiedChannel, string recordStatus, int customerInfoId, UnitOfWork uow)
        {
            try
            {
                var flow = Intertech.Common.ParameterCache.SubesizWebMkStatusParameters.SubesizWebMkStatus[statu];
                var cusProduct = GetCustomerProduct(citizenshipNo, customerInfoId, product, uow);
                if (cusProduct == null)
                    throw new Exception("CustomerProduct(" + product + ") Bulunamadı");

                cusProduct.UpdateTime = DateTime.Now;
                cusProduct.RecordStatus = recordStatus;
                if (!string.IsNullOrEmpty(workFlowId))
                    cusProduct.WorkFlowId = workFlowId;
                if (!string.IsNullOrEmpty(lastModifiedBy))
                    cusProduct.LastModifiedBy = lastModifiedBy;
                if (!string.IsNullOrEmpty(lastModifiedChannel))
                    cusProduct.LastModifiedChannel = lastModifiedChannel;
                if (statu != 0)
                {
                    cusProduct.PreviousStatu = cusProduct.Statu;
                    cusProduct.Statu = statu;
                }
                uow.Session.Save(cusProduct);
                return true;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                log4net.GlobalContext.Properties["customerinfoid"] = customerInfoId;
                logger.Fatal("CustomerProducts Tablosunda Kayıt Güncellerken Hata", ex);
                return false;
            }
        }

        public bool IsActiveRecordUI(string citizenshipNo)
        {
            bool response = false;
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))//
            {
                try
                {
                    var resp = uow.Session.CreateCriteria<CustomerInfo>("cp")
                        .CreateCriteria("cp.Customer", "c")
                        .Add(Restrictions.Eq("c.CitizenshipNumber", citizenshipNo))
                        .Add(Restrictions.Eq("cp.RecordStatus", "A"))
                        .Add(Restrictions.Eq("cp.IsRecordFinish", true))
                        .List();

                    if (resp != null && resp.Count > 0)
                        response = true;
                    else
                        response = false;

                    uow.Commit();
                    return response;
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    log4net.GlobalContext.Properties["customerinfoid"] = 0;
                    logger.Fatal("Aktif Kullanıcı Kaydı SOrgulanırken Hata", ex);
                    return true;
                }
            }
        }

        public bool BlockAccountUI(string citizenshipNo, string parameterCode)
        {
            double limit = 0;
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))//
            {
                try
                {
                    var rule = GetParameterValueByFortuna(parameterCode);
                    limit = Convert.ToDouble(rule);
                    var cus = GetCustomer(citizenshipNo, uow);
                    if (cus == null)
                        throw new Exception("Customer bulunamadı");

                    cus.BlockedTime = DateTime.Now;
                    cus.IsBlocked = true;
                    cus.BlockTimeInterval = limit;
                    uow.Session.Save(cus);
                    uow.Commit();//
                    return true;
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["customerinfoid"] = 0;
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    logger.Fatal("Kullanıcı hesabı bloklama hatası", ex);
                    return false;
                }
            }
        }

        public bool UnBlockAccount(string citizenshipNo, UnitOfWork uow)
        {
            try
            {
                Customer cus = GetCustomer(citizenshipNo, uow);
                if (cus == null)
                    throw new Exception("Customer Bilgisi Bulunamadı");

                TimeSpan date = DateTime.Now - Convert.ToDateTime(cus.BlockedTime);

                double firstDate = date.Days * 24 + date.Hours;
                double lastDate = cus.BlockTimeInterval * 24;

                if (firstDate > lastDate)
                {
                    cus.UpdateTime = DateTime.Now;
                    cus.IsBlocked = false;
                    cus.BlockTimeInterval = 0;
                    uow.Session.Save(cus);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                logger.Fatal("Kullanıcı Kaydının Blok'unu Kaldırırken Hata", ex);
                //return false;
                throw;
            }
        }

        public bool IsBlockAccountUI(string citizenshipNo)
        {
            bool response = false;
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))//
            {
                try
                {
                    UnBlockAccount(citizenshipNo, uow);
                    var cus = GetCustomer(citizenshipNo, uow);
                    response = cus.IsBlocked.Value;
                    uow.Commit();//
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["customerinfoid"] = 0;
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    logger.Fatal("Kullanıcı hesabı blok kontrolü hatası", ex);
                    return true;
                }
            }
            return response;
        }

        public bool CitizenshipNoAndMobileNumberDayLimitUI(string citizenshipNo, string mobileNumber)
        {
            int limit = 0;
            bool response = false;
            using (UnitOfWork uow = new UnitOfWork(this.SessionFactory))//
            {
                try
                {
                    var rule = GetParameterValueByFortuna("rule1");
                    if (rule != null)
                        limit = Convert.ToInt32(rule);
                    else
                        throw new Exception("Rule değeri(rule1) Alınamadı");

                    var resp = uow.Session.CreateCriteria<CustomerInfo>("ci")
                        .CreateCriteria("ci.Customer", "c", NHibernate.SqlCommand.JoinType.InnerJoin)
                        .Add(Restrictions.Eq("c.CitizenshipNumber", citizenshipNo))
                        .Add(Restrictions.Ge("ci.RegisterTime", DateTime.Now.Date))
                        .List<CustomerInfo>();

                    var resp1 = uow.Session.CreateCriteria<CustomerInfo>("ci")
                       .CreateCriteria("ci.Customer", "c", NHibernate.SqlCommand.JoinType.InnerJoin)
                       .Add(Restrictions.Eq("ci.CustomerPhone", mobileNumber))
                       .Add(Restrictions.Ge("ci.RegisterTime", DateTime.Now.Date))
                       .List<CustomerInfo>();

                    if (resp.Count < limit && resp1.Count < limit)
                        response = true;
                    else
                        response = false;

                    uow.Commit();
                    return response;
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    log4net.GlobalContext.Properties["citizenshipnumber"] = citizenshipNo;
                    log4net.GlobalContext.Properties["customerinfoid"] = 0;
                    logger.Fatal("Aynı TC Kimlik No ve Telefon İle Kayıt Sayısını Sorgularken Hata", ex);
                    return false;
                }
            }
        }

        public int GetBarcodeCounter(UnitOfWork uow)
        {
            BarcodeCounter barcod;
            try
            {
                var resp = uow.Session.CreateCriteria<BarcodeCounter>("cp")
                    .Add(Restrictions.Ge("cp.Date", DateTime.Now.Date))
                    .List<BarcodeCounter>();

                if (resp != null && resp.Count > 0)
                {
                    resp.First().Counter++;
                    uow.Session.Save(resp.First());
                    return resp.First().Counter;
                }
                else
                {
                    barcod = new BarcodeCounter();
                    barcod.Counter = 1;
                    barcod.Date = DateTime.Now;
                    uow.Session.Save(barcod);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("Barkod Numarası Oluştururken Hata", ex);
                return -1;
            }
        }

        public string GetParameterValueByFortuna(string parameterCode)
        {
            try
            {
                var parameter = Intertech.Common.ParameterCache.SubesizWebMkParamsParameters.SubesizWebMkParams[parameterCode].m_ParameterValue;
                if (string.IsNullOrEmpty(parameter))
                    throw new Exception("Parametre(" + parameterCode + ") değerine ulaşılamadı");
                return parameter;
            }
            catch (Exception ex)
            {
                logger.Fatal("Parametre alınırken hata", ex);
                return null;
            }
        }

        public Dictionary<string, string> GetParameterArrayByFortuna(string[] parameterCode)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            try
            {
                list = new Dictionary<string, string>();
                var resp = Intertech.Common.ParameterCache.SubesizWebMkParamsParameters.GetAsDataTable();
                if (resp.Rows.Count > 0)
                {
                    foreach (DataRow row in resp.Rows)
                        list.Add(row["ParameterCode"].ToString(), row["ParameterValue"].ToString());
                }
                else
                    throw new Exception("Parametre değerlerine ulaşılamadı");
                return list;
            }
            catch (Exception ex)
            {
                logger.Fatal("Hata", ex);
                return null;
            }
        }
        
        public Dictionary<string, string> GetCampaignCodesByChannel(string channel)
        {
            Dictionary<string, string> list;
            try
            {
                list = new Dictionary<string, string>();
                var campaignList = Intertech.Common.ParameterCache.SubesizWebMkCampaignParameters.SubesizWebMkCampaign.GetCampaignByChannel(channel);
                if (campaignList.Count() > 0)
                {
                    foreach (var resp in campaignList)
                        list.Add(resp.CampaignCode, resp.CampaignName);
                }
                return list;
            }
            catch (Exception ex)
            {
                logger.Fatal("CampaignCode'lar Channel'dan(" + channel + ") çekilemedi.", ex);
                return null;
            }
        }
        //??yeni kanal yapısı
        public bool CampaignCodeControl(string channel, string campaignCode)
        {
            try
            {
                //??silinecek tablet yeni kanal yapısına geçince
                if (channel == "CALLCENTER")
                    return true;
                //??silinecek tablet yeni kanal yapısına geçince

                var campaignList = Intertech.Common.ParameterCache.SubesizWebMkCampaignParameters.SubesizWebMkCampaign.GetCampaignByChannel(channel);
                if (campaignList.Count() > 0)
                {
                    foreach (var resp in campaignList)
                    {
                        if (resp.CampaignCode == campaignCode)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.Fatal("CampaignCode'lar Channel'dan(" + channel + ") çekilemedi.", ex);
                throw;
            }
        }
        //yeni kanal yapısı
        public bool SmsSendFromChannel(string channel)
        {
            try
            {
                var resp = Intertech.Common.ParameterCache.SubesizWebMkChannelParameters.SubesizWebMkChannels[channel];
                return resp.SmsVerification;
            }
            catch (Exception ex)
            {
                logger.Fatal("Hata,Channel(" + channel + ")", ex);
                throw;
            }
        }

        public static string GenerateNumericKey()
        {
            Random rnd = new Random();
            string ticks = DateTime.Now.Ticks.ToString();
            int ticksInt = Convert.ToInt32(ticks.Substring(ticks.Length - 6, 6));
            string randomKey = new Random(ticksInt).Next(1000000, 10000000).ToString().Substring(1);
            string randomKey2 = Convert.ToInt64(randomKey).ToString();
            while (randomKey2.Length < 6)
            {
                int suffix_number = rnd.Next(0, 10);
                randomKey2 = randomKey2 + suffix_number.ToString();
            }
            return randomKey2;
        }

        */
    }
}